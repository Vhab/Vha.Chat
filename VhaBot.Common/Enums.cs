/*
* VhaBot - Barbaric Edition
* Copyright (C) 2005-2008 Remco van Oosterhout
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
using System.Collections.Generic;
using System.Text;

namespace VhaBot.Common
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
        Playfield = 0x87,
        Trade = 0x86,
        Guild = 0x03
    }

    public enum CharacterClass
    {
        Unknown = 0,
        Conqueror = 22,
        DarkTemplar = 31,
        Guardian = 20,
        BearShaman = 29,
        PriestOfMitra = 24,
        TempestOfSet = 28,
        Assassin = 34,
        Barbarian = 18,
        Ranger = 39,
        Demonologist = 44,
        HeraldOfXotli = 43,
        Necromancer = 41
    }

    public enum CharacterState
    {
        Offline = 0,
        Online = 1,
        LookingForTeam = 2,
        AwayFromKeyboard = 3,
        Unknown = 999
    }

    public enum SystemMessageType
    {
        Unknown = 0,
        IncommingOfflineMessage = 172363154,
        OutgoingOfflineMessage = 158601204
    }
}
