/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; version 2 of the License only.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
* USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Vha.Net
{
    public class Proxy
    {
        private UriBuilder _serverURI = null;
        private Socket _socket = null;

        public Socket Socket { get { return this._socket; } }

        public Proxy(Uri proxy, string dstAddress, int dstPort)
        {
            this._serverURI = new UriBuilder(proxy);
            switch (this._serverURI.Scheme)
            {
                case "http": // HTTP proxy.
                    this.ConnectHttp(dstAddress, dstPort);
                    break;
                case "socks4": // SOCKS v4 proxy.
                    this.ConnectSocks4(dstAddress, dstPort);
                    break;
                case "socks5": // SOCKS v5 proxy.
                    throw new NotImplementedException("Socks5 has not been implemented by this proxy client");
                default:
                    throw new ArgumentException("Invalid proxy type");
            }
        }

        /// <summary>
        /// Connect to a HTTP proxy and use CONNECT method.
        /// </summary>
        /// <param name="dstHost">Which host do we tell the proxy to connect to?</param>
        /// <param name="dstPort">Which port do we tell the proxy to connect to?</param>
        private void ConnectHttp(string dstHost, int dstPort)
        {
            Socket proxySocket;
            IPHostEntry proxyhost = Dns.GetHostEntry(this._serverURI.Host);
            IPEndPoint ipe = new IPEndPoint(proxyhost.AddressList[0], this._serverURI.Port);
            proxySocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            proxySocket.Connect(ipe);
            if (proxySocket.Connected) // We have a connection to the proxy..
            {
                string request = "CONNECT " + dstHost + ":" + dstPort + " HTTP/1.0\r\n\r\n";
                if (proxySocket.Connected)
                    proxySocket.Send(NetString.Encoding.GetBytes(request));
                bool still_talking_to_proxy = proxySocket.Connected; // Only talk to it if we're connected. :)
                List<byte> proxy_communication = new List<byte>();
                bool have_return = false;
                int num_rn = 0; // Number of \r\n we have. We should wait for two before assessing the situation and tossing the socket along to the rest of the class.
                try
                {
                    while (still_talking_to_proxy)
                    {
                        byte[] buff = new byte[1];
                        proxySocket.Receive(buff, 0, 1, SocketFlags.Partial);
                        switch (buff[0])
                        {
                            case 13: // \r
                                have_return = true;
                                proxy_communication.Add(buff[0]);
                                break;
                            case 10: // \n
                                proxy_communication.Add(buff[0]);
                                if (have_return)
                                    num_rn++;
                                if (num_rn == 2)
                                    still_talking_to_proxy = false;
                                break;
                            default:
                                have_return = false;
                                proxy_communication.Add(buff[0]);
                                num_rn = 0;
                                break;
                        }
                    }
                }
                catch (SocketException) { }
                string msg = NetString.Encoding.GetString(proxy_communication.ToArray());
                string[] msg2 = msg.Split("\r\n".ToCharArray());
                foreach (string m in msg2)
                {
                    if (m.StartsWith("HTTP/1.0 200"))
                    {
                        this._socket = proxySocket;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Connect to a Socks v4 server. This method is experimental/untested. - Demoder
        /// </summary>
        /// <param name="dstHost">Which host do we tell the proxy to connect to?</param>
        /// <param name="dstPort">Which port do we tell the proxy to connect to?</param>
        private void ConnectSocks4(string dstHost, int dstPort)
        {
            Socket proxySocket;
            IPHostEntry proxyhost = Dns.GetHostEntry(this._serverURI.Host);
            IPEndPoint ipe = new IPEndPoint(proxyhost.AddressList[0], this._serverURI.Port);
            proxySocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            proxySocket.Connect(ipe);
            if (proxySocket.Connected) // We have a connection to the proxy..
            {
                List<byte> sendbytes = new List<byte>();
                sendbytes.Add(4); // Socks version number.
                sendbytes.Add(1); // Command request. 1= connect
                // Add destination port.
                byte[] tmpbytes = BitConverter.GetBytes(dstPort);
                foreach (byte b in tmpbytes)
                    sendbytes.Add(b);
                // Add destination IP.
                tmpbytes = ((Dns.GetHostEntry(dstHost)).AddressList[0]).GetAddressBytes();
                foreach (byte b in tmpbytes)
                    sendbytes.Add(b);
                //Add our userid.
                if (this._serverURI.UserName != string.Empty)
                {
                    byte[] bytes = NetString.Encoding.GetBytes(this._serverURI.UserName);
                    foreach (byte b in bytes)
                        sendbytes.Add(b);
                }
                // Add final nullbyte.
                sendbytes.Add(0);
                // Send our bytes!
                proxySocket.Send(sendbytes.ToArray());

                bool accepted = false;
                // Now receive the reply.
                for (int i = 0; i < 8; i++)
                {
                    byte[] buff = new byte[1];
                    try
                    {
                        proxySocket.Receive(buff, 0, 1, SocketFlags.Partial);
                    }
                    catch { }
                    if (i == 1)
                    {
                        switch (buff[0])
                        {
                            case 90: // Request granted.
                                accepted = true;
                                return;
                            case 91: // Request rejected or failed
                            case 92: // Request rejected becasue SOCKS server cannot connect to identd on the client
                            case 93: // Request rejected because the client program and identd report different user-ids
                            default: // Unhandled
                                return;
                        }
                    }
                }
                if (accepted)
                    this._socket = proxySocket;
            }
        }

        public override string ToString()
        {
            return this._serverURI.Scheme + ":// " + this._serverURI.Host + ":" + this._serverURI.Port; // Manually construct, as we don't want to risk echoing user/pass into debug stream.
        }
    }
}