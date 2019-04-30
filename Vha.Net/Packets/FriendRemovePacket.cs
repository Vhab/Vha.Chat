﻿/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
    internal class FriendRemovePacket : Packet
    {
        internal FriendRemovePacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal FriendRemovePacket(UInt32 characterID)
            : base(Packet.Type.FRIEND_REMOVE)
        {
            this.AddData(NetConvert.HostToNetworkOrder(characterID));
        }
    }
}
