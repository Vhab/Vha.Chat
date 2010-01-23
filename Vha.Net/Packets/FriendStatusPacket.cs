/*
* Vha.Net
* Copyright (C) 2005-2009 Remco van Oosterhout
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
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popInteger(ref data, ref offset));
            this.AddData(popData(ref data, ref offset));
        }

        internal UInt32 CharacterID { get { return (UInt32)this.Data[0]; } }
        internal bool Online { get { return Convert.ToBoolean(this.Data[1]); } }
        internal bool Temporary
        {
            get
            {
                byte[] buddyData = (byte[])this.Data[2];
                return ((buddyData[0] & 0x01) != 0 ? false : true);
            }
        }
    }
}
