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

namespace Vha.Net.Packets
{
    internal class ChatCommandPacket : Packet
    {
        internal ChatCommandPacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal ChatCommandPacket(UInt32 windowId, params string[] commands)
            : base(Packet.Type.CHAT_COMMAND)
        {
            this.AddData(IPAddress.HostToNetworkOrder((short)commands.Length));
            foreach (string command in commands)
            {
                this.AddData(new NetString(command));
            }
            this.AddData(NetConvert.HostToNetworkOrder(windowId));
        }
    }
}
