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
using Vha.Common;

namespace Vha.Net.Packets
{
    internal class ChannelStatusPacket : Packet
    {
        internal ChannelStatusPacket(Packet.Type type, byte[] data) : base(type, data) { }
        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 10) { return; }

            int offset = 0;
            this.AddData(PopChannelID(ref data, ref offset));
            this.AddData(PopString(ref data, ref offset).ToString());
            this.AddData(PopUnsignedShort(ref data, ref offset));
            this.AddData(PopShort(ref data, ref offset));
        }

        internal BigInteger ID { get { return (BigInteger)this.Data[0]; } }
        internal String Name { get { return (String)this.Data[1]; } }
        internal byte ChannelType { get { return ((BigInteger)this.Data[0]).GetBytes()[0]; } }
        internal UInt16 Flags { get { return (UInt16)this.Data[2]; } }
        internal bool Muted { get { return Convert.ToBoolean(this.Flags & 0x0100); } }
    }
}