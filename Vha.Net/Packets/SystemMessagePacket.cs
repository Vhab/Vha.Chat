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
using System.Net;
using System.Collections.Generic;
using Vha.Common;

namespace Vha.Net.Packets
{
    public class SystemMessagePacket : Packet
    {
        internal SystemMessagePacket(Packet.Type type, byte[] data) : base(type, data) { }

        internal List<string> _arguments = new List<string>();

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 6) { return; }
            int offset = 0;
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));

            //Parse remaining data.
            this.DataToArguments(popData(ref data, ref offset));
            this.AddData(popString(ref data, ref offset).Value);

        }

        private void DataToArguments(byte[] data)
        {
            // Early out
            if (data == null || data.Length == 0) return;
            // Some setup
            int oldOffset; // Used to prevent getting stuck in an endless loop.
            int offset = 0;
            while (offset < data.Length)
            {
                switch (data[offset])
                {
                    case (Byte)'S':
                    case (Byte)'s':
                        offset++; // Bump offset by one
                        oldOffset = offset;
                        this._arguments.Add(popString(ref data, ref offset).Value);
                        if (offset == oldOffset) return; // return, or we'd be stuck in an endless loop.
                        break;
                    case (Byte)'I':
                        offset++;
                        this._arguments.Add(
                            IPAddress.NetworkToHostOrder(
                                BitConverter.ToInt32(data, offset)
                            ).ToString()
                        );
                        offset += 4;
                        break;
                    default:
                        offset = 0;
                        this._arguments.Add(NetString.Encoding.GetString(data)); // Add the entire "to-be-decoded" message as the last entry, for debugging purposes.
                        return; // Bail out due to failure
                }
            }
        }

        internal UInt32 ClientID { get { return (UInt32)this.Data[0]; } }
        internal UInt32 WindowID { get { return (UInt32)this.Data[1]; } }
        internal UInt32 MessageID { get { return (UInt32)this.Data[2]; } }
        internal string[] Arguments { get { return ((List<string>)this._arguments).ToArray(); } }
        internal string Notice { get { return (string)this.Data[3]; } }
    }
}