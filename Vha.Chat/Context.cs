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
using Vha.Chat.Events;

namespace Vha.Chat
{
    public delegate void Handler(Context context);
    public delegate void Handler<T>(Context context, T args);

    public class Context
    {
        #region Components
        /// <summary>
        /// Returns the initial read-only application configuration
        /// </summary>
        public Configuration Configuration { get { return null; } }
        /// <summary>
        /// Returns the user customizable options
        /// </summary>
        public Options Options { get { return null; } }
        /// <summary>
        /// Returns the input processing toolset
        /// </summary>
        public Input Input { get { return null; } }
        /// <summary>
        /// Returns the toolset to manage the users on the ignore list
        /// </summary>
        public Ignore Ignore { get { return null; } }
        /// <summary>
        /// Returns direct access to the chat instance
        /// </summary>
        public Net.Chat Chat { get { return null; } }
        #endregion

        #region Events
        /// <summary>
        /// Fires when the internal state of the Context changed
        /// </summary>
        public event Handler<ContextState> StateEvent;
        /// <summary>
        /// Fires when the user is required to select a character
        /// </summary>
        public event Handler<SelectCharacterEventArgs> SelectCharacterEvent;
        /// <summary>
        /// Fires whenever a new message is available
        /// </summary>
        public event Handler<MessageEventArgs> MessageEvent;
        /// <summary>
        /// Fires when a friend was first seen or was just added to the friends list
        /// </summary>
        public event Handler<Friend> FriendAddedEvent;
        /// <summary>
        /// Fires when a user is removed from the friends list
        /// </summary>
        public event Handler<Friend> FriendRemovedEvent;
        /// <summary>
        /// Fires when a friend who is already on the friends list changes status
        /// </summary>
        public event Handler<Friend> FriendUpdatedEvent;
        /// <summary>
        /// Fires when a channel is seen for the first time
        /// </summary>
        public event Handler<Channel> ChannelAddedEvent;
        /// <summary>
        /// Fires when an already known channel changes status
        /// </summary>
        public event Handler<Channel> ChannelUpdatedEvent;
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
        #endregion

        #region Attributes
        public ContextState State { get { return this._state; } }
        /// <summary>
        /// Returns the current dimension.
        /// Will throw an error if the current state is Disconnected.
        /// </summary>
        public string Dimension
        {
            get
            {
                if (this.State == ContextState.Disconnected)
                    throw new InvalidOperationException("Context.Dimension is not available while in state " + this.State);
                return this._dimension;
            }
        }
        /// <summary>
        /// Returns the current account.
        /// Will throw an error if the current state is Disconnected.
        /// </summary>
        public string Account
        {
            get
            {
                if (this.State == ContextState.Disconnected)
                    throw new InvalidOperationException("Context.Account is not available while in state " + this.State);
                return this._account;
            }
        }
        /// <summary>
        /// Returns the current character.
        /// Will throw an error if the current state is not Connected or Reconnecting.
        /// </summary>
        public string Character
        {
            get
            {
                if (this.State != ContextState.Connected &&
                    this.State != ContextState.Reconnecting &&
                    this.State != ContextState.CharacterSelection)
                    throw new InvalidOperationException("Context.Character is not available while in state " + this.State);
                return this._character;
            }
        }
        #endregion

        #region 'Preperation' and 'state' methods
        public void Connect(string dimension, string account, string password);
        public void Disconnect();
        #endregion

        #region 'Showtime' methods
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
        /// <summary>
        /// Write output to one of the listening output targets
        /// </summary>
        /// <param name="target">Describes the original/target of the message</param>
        /// <param name="messageClass">Describes the class of the message</param>
        /// <param name="message">The message itself</param>
        public void Write(MessageTarget target, MessageClass messageClass, string message);
        #endregion

        #region Internal
        private Configuration _configuration;
        private Options _options;
        private Input _input;
        private Net.Chat _chat = null;
        private string _dimension = null;
        private string _account = null;
        private string _character = null;
        private ContextState _state = ContextState.Disconnected;

        private Dictionary<string, Channel> _channels;
        private Dictionary<string, Friend> _friends;

        internal Context()
        {
            // Read initial configuration
            Base configuration = Base.Load("Configuration.xml");
            if (configuration == null) configuration = new Data.ConfigurationV1();
            while (configuration.CanUpgrade) configuration = configuration.Upgrade();
            this._configuration = new Configuration(configuration);

            // Read options
            Base options = Base.Load(this.Configuration.OptionsPath + this.Configuration.OptionsFile);
            if (options == null) options = new Data.OptionsV1();
            while (options.CanUpgrade) options = options.Upgrade();
            this._options = new Options(options);

            // 
        }

        #region Chat callbacks

        #endregion
        #endregion // Internal
    }
}
