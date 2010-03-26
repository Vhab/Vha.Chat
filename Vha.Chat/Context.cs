/*
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
using Vha.Net;
using Vha.Chat.Data;
using Vha.Chat.Events;

namespace Vha.Chat
{
    public delegate void Handler(Context context);
    public delegate void Handler<T>(Context context, T args);

    public class Context
    {
        /// <summary>
        /// Returns the user customizable options
        /// </summary>
        public Options Options { get { return null; } }
        /// <summary>
        /// Returns the input processing toolset
        /// </summary>
        public Input Input { get { return null; } }
        /// <summary>
        /// Returns direct access to the chat instance
        /// </summary>
        public Net.Chat Chat { get { return null; } }
        /// <summary>
        /// Returns the toolset to manage the users on the ignore list
        /// </summary>
        public Ignore Ignore { get { return null; } }

        /// <summary>
        /// Fires whenever a new message is available
        /// </summary>
        public event Handler<MessageEventArgs> MessageEvent;
        /// <summary>
        /// Fires when a friend was first seen or was just added to the friends list
        /// </summary>
        public event Handler FriendAddedEvent;
        /// <summary>
        /// Fires when a user is removed from the friends list
        /// </summary>
        public event Handler FriendRemovedEvent;
        /// <summary>
        /// Fires when a friend who is already on the friends list changes status
        /// </summary>
        public event Handler FriendUpdatedEvent;
        /// <summary>
        /// Fires when a channel is seen for the first time
        /// </summary>
        public event Handler ChannelAddedEvent;
        /// <summary>
        /// Fires when an already known channel changes status
        /// </summary>
        public event Handler ChannelUpdatedEvent;
        /// <summary>
        /// Fires when this client joins a remote private channel
        /// </summary>
        public event Handler PrivateChannelJoinEvent;
        /// <summary>
        /// Fires when this client leaves a remote private channel
        /// </summary>
        public event Handler PrivateChannelLeaveEvent;
        /// <summary>
        /// Fires when this client is invited to join a remote private channel
        /// </summary>
        public event Handler PrivateChannelInviteEvent;
        /// <summary>
        /// Fires when a user joins our local private channel
        /// </summary>
        public event Handler UserJoinEvent;
        /// <summary>
        /// Fires when a user leaves our local private channel
        /// </summary>
        public event Handler UserLeaveEvent;

        /// <summary>
        /// Whether this context contains the given channel
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>true if the given channel is known, false if not</returns>
        public bool HasChannel(string channel);
        /// <summary>
        /// Returns information about a channel by name
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>An instance of Channel or null on failure</returns>
        public Channel GetChannel(string channel);
        /// <summary>
        /// Whether this context contains the given user as friend
        /// </summary>
        /// <param name="friend">The name of the user (case-insensitive)</param>
        /// <returns>true if the given user is a friend, false if not</returns>
        public bool HasFriend(string user);
        /// <summary>
        /// Returns information about the given friend
        /// </summary>
        /// <param name="user">The name of the user (case-insensitive)</param>
        /// <returns>An isntance of Friend or null on failure</returns>
        public Friend GetFriend(string user);

        #region Internal
        private Options _options;
        private Input _input;
        private Net.Chat _chat;

        private Dictionary<string, Channel> _channels;
        private Dictionary<string, Friend> _friends;

        internal Context(Net.Chat chat)
        {
            this._chat = chat;
            // Hook events
        }
        #endregion
    }
}
