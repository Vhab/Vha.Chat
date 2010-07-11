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

namespace Vha.Net.Packets
{
    internal class PrivateChannelStatusPacket : Packet
    {
        internal PrivateChannelStatusPacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal PrivateChannelStatusPacket(UInt32 channelID, UInt32 characterID, bool join)
            : base((join ? Packet.Type.PRIVATE_CHANNEL_CLIENTJOIN : Packet.Type.PRIVATE_CHANNEL_CLIENTPART))
        {
            this.AddData(NetConvert.HostToNetworkOrder(channelID));
            this.AddData(NetConvert.HostToNetworkOrder(characterID));
        }

        internal PrivateChannelStatusPacket(UInt32 channelID, bool join)
            : base((join ? Packet.Type.PRIVATE_CHANNEL_JOIN : Packet.Type.PRIVATE_CHANNEL_PART))
        {
            this.AddData(NetConvert.HostToNetworkOrder(channelID));
        }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 4) { return; }

            int offset = 0;
            this.AddData(PopUnsignedInteger(ref data, ref offset));
            this.AddData(PopUnsignedInteger(ref data, ref offset));
        }
        internal UInt32 ChannelID { get { return (UInt32)this.Data[0]; } }
        internal UInt32 CharacterID { get { return (UInt32)this.Data[1]; } }
        internal bool Joined { get { return (this.PacketType == Packet.Type.PRIVATE_CHANNEL_CLIENTJOIN || this.PacketType == Packet.Type.PRIVATE_CHANNEL_INVITE || this.PacketType == Packet.Type.PRIVATE_CHANNEL_JOIN); } }
    }
}