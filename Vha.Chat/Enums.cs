/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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

namespace Vha.Chat
{
    public enum ContextState
    {
        /// <summary>
        /// The context is currently not connected.
        /// The next state is Connecting.
        /// </summary>
        Disconnected,
        /// <summary>
        /// The context is in temporary idle state before attempting to connect again.
        /// The next state is Connecting or Disconnected.
        /// </summary>
        Reconnecting,
        /// <summary>
        /// The context is trying to connect.
        /// The next state is CharacterSelection or Disconnected.
        /// </summary>
        Connecting,
        /// <summary>
        /// The context is connected and the user just selected a character to login with.
        /// The next state is Disconnected or Connected.
        /// </summary>
        CharacterSelection,
        /// <summary>
        /// The context is fully connected.
        /// The next state is Disconnected or Reconnecting.
        /// </summary>
        Connected
    }

    public enum ErrorType
    {
        /// <summary>
        /// An error triggered while attempting to enstablish a connection
        /// </summary>
        Login
    }

    public enum MessageClass
    {
        None, // No particular class
        Internal, // Internal status/info messages
        Error, // Error messages
        Text, // Echo'd messages
        SystemMessage,
        BroadcastMessage,
        PrivateMessage,
        PrivateChannel,
        // Public channels
        UnknownChannel,
        AdminChannel,
        TeamChannel,
        OrganizationChannel,
        LeadersChannel,
        GMChannel,
        ShoppingChannel,
        GeneralChannel,
        TowersChannel,
        AnnouncementsChannel,
        RaidChannel,
        BattlestationChannel
    }

    public enum MessageType
    {
        /// <summary>
        /// The message has no particular origin.
        /// This often indicates an internal status or error message.
        /// </summary>
        None,
        /// <summary>
        /// Indicates a private message
        /// </summary>
        Character,
        /// <summary>
        /// Indicates a public message in a public channel
        /// </summary>
        Channel,
        /// <summary>
        /// Indicates a public message in a private channel
        /// </summary>
        PrivateChannel
    }

    public enum TextStyle
    {
        /// <summary>
        /// Display the text as it is received
        /// </summary>
        Default,
        /// <summary>
        /// Invert all the colors contained within the text
        /// </summary>
        Invert,
        /// <summary>
        /// Strip all colors from the text
        /// </summary>
        Strip
    }

    public enum IgnoreMethod
    {
        /// <summary>
        /// Ignores are disable
        /// </summary>
        None,
        /// <summary>
        /// Per dimension
        /// </summary>
        Dimension,
        /// <summary>
        /// Per dimension+account
        /// </summary>
        Account,
        /// <summary>
        /// Per dimension+account+character
        /// </summary>
        Character
    }

    public enum IgnoreResult
    {
        /// <summary>
        /// Removed entry
        /// </summary>
        Removed,
        /// <summary>
        /// Added entry
        /// </summary>
        Added,
        /// <summary>
        /// Error occured
        /// </summary>
        Error
    }

    public enum HorizontalPosition
    {
        /// <summary>
        /// Indicates an element is on the left side
        /// </summary>
        Left,
        /// <summary>
        /// Indicates an element is on the right side
        /// </summary>
        Right
    }

    public enum VerticalPosition
    {
        /// <summary>
        /// Indicates an element is on the top side
        /// </summary>
        Top,
        /// <summary>
        /// Indicates an element is on the bottom side
        /// </summary>
        Bottom
    }

    public enum ProxyType
    {
        Disabled,
        HTTP,
        Socks4,
        Socks4a,
        Socks5
    }
}
