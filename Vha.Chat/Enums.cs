﻿/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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

    public enum MessageClass
    {
        None,
        Internal,
        Organization,
        General,
        Announcements,
        Towers,
        Shopping,
        Vicinity,
        Text,
        PM,
        PG,
        Error,
        System,
    }

    public enum MessageType
    {
        None,
        Character,
        Channel,
        PrivateChannel
    }

    public enum IgnoreMethod
    {
        /// <summary>
        /// No method - Ignore system is disabled
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
}
