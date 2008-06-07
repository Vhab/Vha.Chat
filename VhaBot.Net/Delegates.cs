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

namespace VhaBot.Net
{
    public delegate void AmdMuxInfoEventHandler(Chat chat, AmdMuxInfoEventArgs e);
    public delegate void AnonVicinityEventHandler(Chat chat, AnonVicinityEventArgs e);
    public delegate void FriendStatusEventHandler(Chat chat, FriendStatusEventArgs e);
    public delegate void FriendRemovedEventHandler(Chat chat, CharacterIDEventArgs e);
    public delegate void CharacterIDEventHandler(Chat chat, CharacterIDEventArgs e);
    public delegate void PrivateChannelRequestEventHandler(Chat chat, PrivateChannelRequestEventArgs e);
    public delegate void NameLookupEventHandler(Chat chat, NameLookupEventArgs e);
    public delegate void ForwardEventHandler(Chat chat, ForwardEventArgs e);
    public delegate void ChannelJoinEventHandler(Chat chat, ChannelJoinEventArgs e);
    public delegate void ChannelMessageEventHandler(Chat chat, ChannelMessageEventArgs e);
    public delegate void SimpleMessageEventHandler(Chat chat, SimpleStringPacketEventArgs e);
    public delegate void SystemMessageEventHandler(Chat chat, SystemMessagePacketEventArgs e);
    public delegate void LoginOKEventHandler(Chat chat, EventArgs e);
    public delegate void UnknownPacketEventHandler(Chat chat, UnknownPacketEventArgs e);
    public delegate void PrivateChannelStatusEventHandler(Chat chat, PrivateChannelStatusEventArgs e);
    public delegate void PrivateChannelMessageEventHandler(Chat chat, PrivateChannelMessageEventArgs e);
    public delegate void TellEventHandler(Chat chat, TellEventArgs e);
    public delegate void LoginSeedEventHandler(Chat chat, LoginMessageEventArgs e);
    public delegate void LoginCharlistEventHandler(Chat chat, LoginChararacterListEventArgs e);
    public delegate void StatusChangeEventHandler(Chat chat, StatusChangeEventArgs e);
}