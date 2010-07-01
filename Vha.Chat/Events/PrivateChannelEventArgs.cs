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
using Vha.Chat;
using Vha.Net;

namespace Vha.Chat.Events
{
    public class PrivateChannelEventArgs
    {
        /// <summary>
        /// The channel in which this event occurred
        /// </summary>
        public readonly PrivateChannel Channel;
        /// <summary>
        /// The character who left or joined the channel
        /// </summary>
        public readonly string Character;
        /// <summary>
        /// Whether the character joined the channel
        /// </summary>
        public readonly bool Joined;
        /// <summary>
        /// Whether this event occurred in our local channel
        /// </summary>
        public readonly bool Local;

        public PrivateChannelEventArgs(PrivateChannel channel, string character, bool joined, bool local)
        {
            this.Channel = channel;
            this.Character = character;
            this.Joined = joined;
            this.Local = local;
        }
    }
}
