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
	internal class EmptyPacket : Packet
	{
		internal EmptyPacket(Packet.Type type): base(type) {}
		internal override byte[] GetBytes()
		{
			if (this.PacketType == Packet.Type.PRIVATE_CHANNEL_KICKALL)
				return new byte[0];
			else
				return (new NetString(string.Empty)).GetBytes();
		}

	}
}
