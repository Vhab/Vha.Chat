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
    internal class FriendStatusPacket : Packet
    {
        internal FriendStatusPacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 10) { return; }

            int offset = 0;
            this.AddData(PopUnsignedInteger(ref data, ref offset));
            this.AddData(PopInteger(ref data, ref offset));
            // Originally the last part of this packet was identified as a set of data,
            // prefixed with the size and followed by a value.
            // Unfortunately some errors have proven this to be incorrect,
            // leaving us with 2 bytes of unidentified data om this.Data[2].
            this.AddData(PopUnsignedShort(ref data, ref offset));
            this.AddData(PopByte(ref data, ref offset));
        }

        internal UInt32 CharacterID { get { return (UInt32)this.Data[0]; } }
        internal bool Online { get { return Convert.ToBoolean(this.Data[1]); } }
        internal bool Temporary
        {
            get
            {
                return (((Byte)this.Data[3] & 0x01) != 0 ? false : true);
            }
        }
    }
}
