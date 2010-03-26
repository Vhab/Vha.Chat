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
using Starksoft.Net.Proxy;

namespace Vha.Net
{
    public class Proxy
    {

        private UriBuilder _serverURI = null;
        private Socket _socket = null;
        ProxyClientFactory _proxyclientfactory = new ProxyClientFactory();
        public Protocol[] SupportedProtocols = new Protocol[] { 
            new Protocol("http", Protocol.SupportLevel.None, Protocol.SupportLevel.None, Protocol.SupportLevel.None),
            new Protocol("socks4", Protocol.SupportLevel.Optional, Protocol.SupportLevel.None, Protocol.SupportLevel.None),
            new Protocol("socks4a", Protocol.SupportLevel.Optional, Protocol.SupportLevel.None, Protocol.SupportLevel.None),
            new Protocol("socks5", Protocol.SupportLevel.Optional, Protocol.SupportLevel.None, Protocol.SupportLevel.Optional)
        };

        /// <summary>
        /// Returns a connection to the target through the proxy.
        /// Returns null if no successful connection was enstablished.
        /// </summary>
        public Socket Socket { get { return this._socket; } }

        /// <summary>
        /// Initialize a new connection to the desired target through the supplied proxy
        /// </summary>
        /// <param name="proxy">An URI that contains all required proxy details</param>
        /// <param name="dstHost">The target host address to connect to</param>
        /// <param name="dstPort">The target host port to connect to</param>
        /// <exception cref="ArgumentException">Thrown when an invalid argument is supplied</exception>
        /// <exception cref="Exception">Thrown when the object failed to initialize. See InnerException for furthur details</exception>
        public Proxy(Uri proxy, string dstHost, int dstPort)
        {
            try
            {
                this._serverURI = new UriBuilder(proxy);
                switch (this._serverURI.Scheme)
                {
                    case "http": // HTTP proxy.
                        this.ConnectHttp(dstHost, dstPort);
                        break;
                    case "socks4": // SOCKS v4 proxy.
                        this.ConnectSocks4(dstHost, dstPort, false);
                        break;
                    case "socks4a":
                        this.ConnectSocks4(dstHost, dstPort, true);
                        break;
                    case "socks5": // SOCKS v5 proxy.
                        this.ConnectSocks5(dstHost, dstPort);
                        break;
                    default:
                        throw new ArgumentException("Invalid proxy type");
                }
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize proxy", ex);
            }
        }

        /// <summary>
        /// Use the HTTP proxy client
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="dstPort"></param>
        private void ConnectHttp(string dstHost, int dstPort)
        {
            // Retreive a proxyclient. the HTTP proxy client does not support user/pass, so no point checking for those variables.
            if (this._serverURI.Port == 0) this._serverURI.Port = 8080; // default port in Starksoft.Net.Proxy.Socks4ProxyClient.cs
            IProxyClient proxyclient = proxyclient = this._proxyclientfactory.CreateProxyClient(ProxyType.Http, this._serverURI.Host, this._serverURI.Port);
            TcpClient tc = proxyclient.CreateConnection(dstHost, dstPort);
            if (tc.Connected)
                this._socket = tc.Client;
        }

        /// <summary>
        /// Use the Socks4 proxy client.
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="dstPort"></param>
        /// <param name="socks4a">if true, we should use the socks4a client</param>
        private void ConnectSocks4(string dstHost, int dstPort, bool socks4a)
        {
            IProxyClient proxyclient;
            // Socks4 or Socks4a?
            ProxyType pt;
            if (socks4a) pt = ProxyType.Socks4a;
            else pt = ProxyType.Socks4;

            // If we don't have a port definition
            if (this._serverURI.Port == 0) this._serverURI.Port = 1080; // default port in Starksoft.Net.Proxy.Socks4ProxyClient.cs. No override in Socks4a.

            // Connect
            if (this._serverURI.UserName != string.Empty) // If the user provided an username
                proxyclient = this._proxyclientfactory.CreateProxyClient(pt, this._serverURI.Host, this._serverURI.Port, this._serverURI.UserName, string.Empty); // Socks v4 only supports UserID as auth
            else
                proxyclient = this._proxyclientfactory.CreateProxyClient(pt, this._serverURI.Host, this._serverURI.Port);
            TcpClient tc = proxyclient.CreateConnection(dstHost, dstPort);
            if (tc.Connected)
                this._socket = tc.Client;
        }

        /// <summary>
        /// Use the Socks5 client.
        /// </summary>
        /// <param name="dstHost"></param>
        /// <param name="dstPort"></param>
        private void ConnectSocks5(string dstHost, int dstPort)
        {
            if (this._serverURI.Port == 0) this._serverURI.Port = 1080; // default port in Starksoft.Net.Proxy.Socks5ProxyClient.cs
            IProxyClient proxyclient;
            if (this._serverURI.UserName != string.Empty && this._serverURI.Password != string.Empty) // If the user provided an username
                proxyclient = this._proxyclientfactory.CreateProxyClient(ProxyType.Socks5, this._serverURI.Host, this._serverURI.Port, this._serverURI.UserName, this._serverURI.Password); // Socks v5 supports user+pass auth.
            else
                proxyclient = this._proxyclientfactory.CreateProxyClient(ProxyType.Socks5, this._serverURI.Host, this._serverURI.Port);
            TcpClient tc = proxyclient.CreateConnection(dstHost, dstPort);
            if (tc.Connected)
                this._socket = tc.Client;
        }

        public override string ToString()
        {
            return this._serverURI.Scheme + ":// " + this._serverURI.Host + ":" + this._serverURI.Port; // Manually construct, as we don't want to risk echoing user/pass into debug stream.
        }

        #region Structures
        /// <summary>
        /// Contains information about a supported protocol. This may be used to create configuration GUIs.
        /// </summary>
        public struct Protocol
        {
            public Protocol(string scheme, SupportLevel username, SupportLevel password, SupportLevel user_and_pass)
            {
                this.Scheme = scheme;
                this.Username = username;
                this.Password = password;
                this.UserAndPassword = user_and_pass;
            }
            // The idea here is to know what we need to supply.
            public string Scheme;
            /// <summary>
            /// Can we provide only username?
            /// </summary>
            public SupportLevel Username;
            /// <summary>
            /// Can we provide only password?
            /// </summary>
            public SupportLevel Password;
            /// <summary>
            /// Can we provide username+password?
            /// </summary>
            public SupportLevel UserAndPassword;
            public enum SupportLevel
            {
                None,
                Optional,
                Required
            }
        }
        #endregion
    }
}