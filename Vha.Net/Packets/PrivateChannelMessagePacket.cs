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

namespace Vha.Net.Packets
{
    internal class PrivateChannelMessagePacket : Packet
    {
        internal PrivateChannelMessagePacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal PrivateChannelMessagePacket(UInt32 channelID, String formattedText)
            : base(Packet.Type.PRIVGRP_MESSAGE)
        {
            this.AddData(NetConvert.HostToNetworkOrder(channelID));
            this.AddData(new NetString(formattedText));
            this.AddData(new NetString("\0"));
        }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 4) { return; }

            int offset = 0;
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popString(ref data, ref offset).ToString());
        }

        internal UInt32 ChannelID { get { return (UInt32)this.Data[0]; } }
        internal UInt32 CharacterID { get { return (UInt32)this.Data[1]; } }
        internal string Message { get { return (String)this.Data[2]; } }
    }
}