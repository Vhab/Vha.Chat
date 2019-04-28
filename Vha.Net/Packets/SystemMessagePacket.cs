/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
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
    internal class SystemMessagePacket : Packet
    {
        internal SystemMessagePacket(Packet.Type type, byte[] data) : base(type, data) { }

        internal List<string> _arguments = new List<string>();

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 6) { return; }
            int offset = 0;
            this.AddData(PopUnsignedInteger(ref data, ref offset));
            this.AddData(PopUnsignedInteger(ref data, ref offset));
            this.AddData(PopUnsignedInteger(ref data, ref offset));
            this.AddData(PopData(ref data, ref offset));
            this.AddData(PopString(ref data, ref offset).Value);
        }

        internal UInt32 ClientID { get { return (UInt32)this.Data[0]; } }
        internal UInt32 WindowID { get { return (UInt32)this.Data[1]; } }
        internal UInt32 MessageID { get { return (UInt32)this.Data[2]; } }
        internal Byte[] Arguments { get { return (Byte[])this.Data[3]; } }
        internal string Notice { get { return (string)this.Data[4]; } }
    }
}
