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

namespace Vha.Net
{
    public enum ChatState
    {
        Disconnected,
        Connecting,
        Login,
        CharacterSelect,
        Connected,
        Reconnecting,
        Error
    }

    public enum ChannelType
    {
        Unknown = 0,
        Announcements = 12,
        General = 135,
        Organization = 3,
        Shopping = 356,
        Towers = 10,
        Leaders = 4
    }

    [Flags]
    public enum ChannelFlags : uint
    {
        None = 0,
        CantIgnore = 0x1,
        CantSend = 0x2,
        NoInternational = 0x10,
        NoVoice = 0x20,
        SendCriteria = 0x40,
        GroupOnName = 0x80,
        Muted = 0x1000000,
    }

    public enum SystemMessageType
    {
        Other = 0,
        IncommingOfflineMessage = 172363154,
        OutgoingOfflineMessage = 158601204
    }

    public enum IncomingOfflineMessageArgs
    {
        Name = 0,
        Date = 1
    }

    public enum PacketPriority
    {
        Urgent = 1,
        Standard,
        Low
    }
}
