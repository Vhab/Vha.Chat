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
using Vha.Net.Events;
using Vha.Chat.Events;
using Vha.Chat.Commands;
using Vha.Common;

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
        public Configuration Configuration { get { return this._configuration; } }
        /// <summary>
        /// Returns the user customizable options
        /// </summary>
        public Options Options { get { return this._options; } }
        /// <summary>
        /// Returns the input processing toolset
        /// </summary>
        public Input Input { get { return this._input; } }
        /// <summary>
        /// Returns the toolset to manage the users on the ignore list
        /// </summary>
        public Ignore Ignore { get { return this._ignore; } }
        /// <summary>
        /// Returns direct access to the chat instance
        /// </summary>
        public Net.Chat Chat { get { return this._chat; } }
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
        /// Fires when a recoverable error has occured, like incorrect arguments or failing to connect.
        /// </summary>
        public event Handler<ErrorEventArgs> ErrorEvent;
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
        public ContextState State
        {
            get
            {
                // All state changing operations lock 'this'
                lock (this) return this._state;
            }
        }
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

        public string Organization
        {
            get
            {
                if (this.State != ContextState.Connected)
                    throw new InvalidOperationException("Context.Organization is not available while in state " + this.State);
                return this._organization;
            }
        }

        public UInt32 OrganizationId
        {
            get
            {
                if (this.State != ContextState.Connected)
                    throw new InvalidOperationException("Context.OrganizationId is not available while in state " + this.State);
                return this._organizationId;
            }
        }

        public bool HasOrganization
        {
            get
            {
                return !string.IsNullOrEmpty(this.Organization);
            }
        }
        #endregion

        #region 'Preperation' and 'state' methods
        /// <summary>
        /// Connect to an Anarchy Online chat server.
        /// This method will throw an error if the state is not Disconnected.
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void Connect(string dimension, string account, string password)
        {
            lock (this)
            {
                // Check state
                if (this.State != ContextState.Disconnected)
                    throw new InvalidOperationException("Not expecting a call to Context.Connect");
                // Obtain dimension information
                Dimension dim = this.Configuration.GetDimension(dimension);
                if (dim == null)
                {
                    if (this.ErrorEvent != null)
                        this.ErrorEvent(this, new ErrorEventArgs(
                            ErrorType.Login,
                            "Attempting to connect to invalid dimension: '" + dimension + "'"));
                    return;
                }

                // Create chat connection
                this._chat = new Vha.Net.Chat(dim.Address, dim.Port, account, password);
                // Set initial settings
                this._chat.AutoReconnect = false;
                this._chat.IgnoreCharacterLoggedIn = true;
                this._chat.UseThreadPool = false;
                this._chat.Tag = this;

                // Hook events (including exceptions)
                // - State changes and more
                this._chat.StatusChangeEvent += new StatusChangeEventHandler(_chat_StatusChangeEvent);
                this._chat.LoginErrorEvent += new LoginErrorEventHandler(_chat_LoginErrorEvent);
                this._chat.LoginCharlistEvent += new LoginCharlistEventHandler(_chat_LoginCharlistEvent);
                // - Friends
                this._chat.FriendStatusEvent += new FriendStatusEventHandler(_chat_FriendStatusEvent);
                this._chat.FriendRemovedEvent += new FriendRemovedEventHandler(_chat_FriendRemovedEvent);
                // - Channels
                this._chat.PrivateChannelRequestEvent += new PrivateChannelRequestEventHandler(_chat_PrivateChannelRequestEvent);
                this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent);
                this._chat.ChannelStatusEvent += new ChannelStatusEventHandler(_chat_ChannelStatusEvent);
                // - Messages
                this._chat.PrivateMessageEvent += new PrivateMessageEventHandler(_chat_PrivateMessageEvent);
                this._chat.ChannelMessageEvent += new ChannelMessageEventHandler(_chat_ChannelMessageEvent);
                this._chat.PrivateChannelMessageEvent += new PrivateChannelMessageEventHandler(_chat_PrivateChannelMessageEvent);
                this._chat.SystemMessageEvent += new SystemMessageEventHandler(_chat_SystemMessageEvent);
                this._chat.SimpleMessageEvent += new SimpleMessageEventHandler(_chat_SimpleMessageEvent);

                // Store values
                this._account = account.ToLower();
                this._dimension = dim.Name;

                // Connect async
                this._chat.Connect(true);
            }
        }

        /// <summary>
        /// Disconnect from the chat server.
        /// </summary>
        public void Disconnect()
        {
            lock (this)
            {
                if (this.State == ContextState.Disconnected)
                    throw new InvalidOperationException("Not expecting a call to Context.Disconnect");
                if (!this._disconnecting)
                {
                    this._chat.Disconnect(true);
                    this._disconnecting = true;
                }
            }
        }
        #endregion

        #region 'Showtime' methods
        /// <summary>
        /// Whether this context contains the given channel
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>true if the given channel is known, false if not</returns>
        public bool HasChannel(string channel)
        {
            lock (this._channels)
            {
                return this._channels.ContainsKey(channel.ToLower());
            }
        }
        /// <summary>
        /// Returns information about a channel by name
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>An instance of Channel or null on failure</returns>
        public Channel GetChannel(string channel)
        {
            channel = channel.ToLower();
            lock (this._channels)
            {
                if (!this._channels.ContainsKey(channel))
                    return null;
                return this._channels[channel];
            }
        }
        /// <summary>
        /// Returns an appropriate MessageClass for the given channel
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>The appropriate class for the channel or None on failure</returns>
        public MessageClass GetChannelClass(string channel)
        {
            Channel c = this.GetChannel(channel);
            if (c == null) return MessageClass.None;
            string type = c.Type.ToString();
            MessageClass messageClass = (MessageClass)Enum.Parse(typeof(MessageClass), type, true);
            if (!Enum.IsDefined(typeof(MessageClass), messageClass)) return MessageClass.None;
            return messageClass;
        }
        /// <summary>
        /// Whether this context contains the given user as friend
        /// </summary>
        /// <param name="friend">The name of the user (case-insensitive)</param>
        /// <returns>true if the given user is a friend, false if not</returns>
        public bool HasFriend(string user)
        {
            user = Format.UppercaseFirst(user);
            lock (this._friends)
            {
                return this._friends.ContainsKey(user);
            }
        }
        /// <summary>
        /// Returns information about a friend by name
        /// </summary>
        /// <param name="user">The name of the user (case-insensitive)</param>
        /// <returns>An isntance of Friend or null on failure</returns>
        public Friend GetFriend(string user)
        {
            user = Format.UppercaseFirst(user);
            lock (this._friends)
            {
                if (!this._friends.ContainsKey(user))
                    return null;
                return this._friends[user];
            }
        }
        /// <summary>
        /// Whether this context contains the given private channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool HasPrivateChannel(string channel)
        {
            channel = Format.UppercaseFirst(channel);
            lock (this._privateChannels)
            {
                return this._privateChannels.ContainsKey(channel);
            }
        }
        /// <summary>
        /// Returns information about a private channel by name
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public PrivateChannel GetPrivateChannel(string channel)
        {
            channel = Format.UppercaseFirst(channel);
            lock (this._privateChannels)
            {
                if (!this._privateChannels.ContainsKey(channel))
                    return null;
                return this._privateChannels[channel];
            }
        }
        /// <summary>
        /// Write output to one of the listening output targets
        /// </summary>
        /// <param name="source">Describes the origin/target of the message</param>
        /// <param name="messageClass">Describes the class of the message</param>
        /// <param name="message">The message itself</param>
        public void Write(MessageSource source, MessageClass messageClass, string message)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            MessageEventArgs args = new MessageEventArgs(source, messageClass, message);
            if (this.MessageEvent != null)
                this.MessageEvent(this, args);
        }
        /// <summary>
        /// Write output to one of the listening output targets
        /// </summary>
        /// <param name="messageClass">Describes the class of the message</param>
        /// <param name="message">The message itself</param>
        public void Write(MessageClass messageClass, string message)
        {
            MessageSource source = new MessageSource();
            Write(source, messageClass, message);
        }
        #endregion

        #region Internal
        private Configuration _configuration;
        private Options _options;
        private Input _input = null;
        private Ignore _ignore = null;
        private Net.Chat _chat = null;
        private string _dimension = null;
        private string _account = null;
        private string _character = null;
        private string _organization = null;
        private UInt32 _organizationId = 0;
        private ContextState _state = ContextState.Disconnected;
        private bool _disconnecting = false;

        private Dictionary<string, Channel> _channels;
        private Dictionary<string, Friend> _friends;
        private Dictionary<string, PrivateChannel> _privateChannels;

        internal Context()
        {
            // Read initial configuration
            Data.Base configuration = Data.Base.Load("Configuration.xml");
            if (configuration == null) configuration = new Data.ConfigurationV1();
            while (configuration.CanUpgrade) configuration = configuration.Upgrade();
            this._configuration = new Configuration(configuration);

            // Read options
            Data.Base options = Data.Base.Load(this.Configuration.OptionsPath + this.Configuration.OptionsFile);
            if (options == null) options = new Data.OptionsV1();
            while (options.CanUpgrade) options = options.Upgrade();
            this._options = new Options(this, options);

            // Initialize objects
            this._channels = new Dictionary<string, Channel>();
            this._friends = new Dictionary<string, Friend>();
            this._privateChannels = new Dictionary<string, PrivateChannel>();

            // Create input
            this._input = new Input(this, "/");
            this.Input.RegisterCommand(new CCCommand());
            this.Input.RegisterCommand(new FriendCommand());
            this.Input.RegisterCommand(new HelpCommand());
            this.Input.RegisterCommand(new IgnoreCommand());
            this.Input.RegisterCommand(new InviteCommand());
            this.Input.RegisterCommand(new KickAllCommand());
            this.Input.RegisterCommand(new KickCommand());
            this.Input.RegisterCommand(new LeaveCommand());
            this.Input.RegisterCommand(new MuteCommand());
            this.Input.RegisterCommand(new OrganizationCommand());
            this.Input.RegisterCommand(new TellCommand());
            this.Input.RegisterCommand(new TextCommand());
            this.Input.RegisterCommand(new UnignoreCommand());
            this.Input.RegisterCommand(new UnmuteCommand());
            this.Input.RegisterCommand(new WhoisCommand());
        }

        #region Chat callbacks
        void _chat_LoginCharlistEvent(Vha.Net.Chat chat, LoginChararacterListEventArgs e)
        {
            // Create event arguments
            List<Character> characters = new List<Character>();
            Dictionary<Character, LoginCharacter> charactersMap = new Dictionary<Character, LoginCharacter>();
            foreach (LoginCharacter c in e.CharacterList)
            {
                Character character = new Character(c.Name, c.ID, c.Level, c.IsOnline);
                characters.Add(character);
                charactersMap.Add(character, c);
            }
            SelectCharacterEventArgs args = new SelectCharacterEventArgs(characters.ToArray());
            // Fire event
            if (this.SelectCharacterEvent != null)
                this.SelectCharacterEvent(this, args);
            // Login
            if (args.Character != null && charactersMap.ContainsKey(args.Character))
            {
                this._character = args.Character.Name;
                this._chat.SendLoginCharacter(charactersMap[args.Character]);
            }
            else
            {
                // If no character was selected, there's no reason to remain connected
                this._chat.Disconnect(true);
            }
        }

        void _chat_LoginErrorEvent(Vha.Net.Chat chat, LoginErrorEventArgs e)
        {
            // Notify listeners of the error
            if (this.ErrorEvent != null)
                this.ErrorEvent(this, new ErrorEventArgs(ErrorType.Login, e.Error));
        }

        void _chat_StatusChangeEvent(Vha.Net.Chat chat, StatusChangeEventArgs e)
        {
            lock (this)
            {
                // Translate state
                ContextState state = this._state;
                switch (e.State)
                {
                    case ChatState.CharacterSelect:
                    case ChatState.Connecting:
                    case ChatState.Login:
                        state = ContextState.Connecting;
                        break;
                    case ChatState.Connected:
                        state = ContextState.Connected;
                        break;
                    case ChatState.Disconnected:
                    case ChatState.Error:
                        state = ContextState.Disconnected;
                        this._disconnecting = false;
                        break;
                    case ChatState.Reconnecting:
                        state = ContextState.Reconnecting;
                        break;
                }
                if (this._state == state) return;
                // Handle state change
                switch (state)
                {
                    case ContextState.Connecting:
                        // Nothing in particular here :)
                        break;
                    case ContextState.CharacterSelection:
                        this._ignore = new Ignore(this);
                        break;
                    case ContextState.Connected:
                        break;
                    case ContextState.Reconnecting:
                        this._organization = null;
                        break;
                    case ContextState.Disconnected:
                        this._chat.ClearEvents();
                        this._chat = null;
                        this._dimension = null;
                        this._account = null;
                        this._character = null;
                        this._organization = null;
                        this._organizationId = 0;
                        this._friends.Clear();
                        this._channels.Clear();
                        this._ignore = null;
                        break;
                }
                // Notify state change
                this._state = state;
            }
            if (this.StateEvent != null)
                this.StateEvent(this, this._state);
        }

        void _chat_ChannelStatusEvent(Vha.Net.Chat chat, ChannelStatusEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _chat_PrivateChannelStatusEvent(Vha.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _chat_PrivateChannelRequestEvent(Vha.Net.Chat chat, PrivateChannelRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _chat_FriendRemovedEvent(Vha.Net.Chat chat, CharacterIDEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _chat_FriendStatusEvent(Vha.Net.Chat chat, FriendStatusEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _chat_PrivateChannelMessageEvent(Vha.Net.Chat chat, PrivateChannelMessageEventArgs e)
        {
            // Check for ignores
            if (this.Ignore.IsIgnored(e.Character))
                return;
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.PrivateChannel, e.Channel, e.Character);
            this.Write(source, MessageClass.PG, e.Message);
        }

        void _chat_ChannelMessageEvent(Vha.Net.Chat chat, ChannelMessageEventArgs e)
        {
            // Check if channel is muted
            if (this.GetChannel(e.Channel).Muted)
                return;
            // Check for ignores
            if (this.Ignore.IsIgnored(e.Character))
                return;
            // Descramble
            string message = e.Message;
            if (e.Message.StartsWith("~"))
            {
                MDB.Message parsedMessage = null;
                try { parsedMessage = MDB.Parser.Decode(e.Message); }
                catch { }
                if (parsedMessage != null && !string.IsNullOrEmpty(parsedMessage.Value))
                    message = parsedMessage.Value;
            }
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.Channel, e.Channel, e.Character);
            this.Write(source, this.GetChannelClass(e.Channel), message);
        }

        void _chat_PrivateMessageEvent(Vha.Net.Chat chat, PrivateMessageEventArgs e)
        {
            // Check for ignores
            if (this.Ignore.IsIgnored(e.Character))
                return;
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.Character, null, e.Character);
            this.Write(source, MessageClass.PM, e.Message);
        }

        void _chat_SystemMessageEvent(Vha.Net.Chat chat, SystemMessageEventArgs e)
        {
            // Apply ignore filter to "received offline message from" messages
            if (e.MessageID == (uint)SystemMessageType.IncommingOfflineMessage)
            {
                string character = (string)e.Arguments[(int)IncomingOfflineMessageArgs.Name];
                // Check ignored users list
                if (this.Ignore.IsIgnored(character))
                    return;
            }
            // Descramble message
            MDB.Reader reader = new MDB.Reader();
            MDB.Entry entry = reader.SpeedRead((int)e.CategoryID, (int)e.MessageID);
            // Failed to get the entry
            if (entry == null)
            {
                this.Write(MessageClass.Error, "Unknown system message " + e.CategoryID + ":" + e.MessageID);
                return;
            }
            // Dispatch message
            string template = MDB.Parser.PrintfToFormatString(entry.Message);
            string message = string.Format(template, e.Arguments);
            this.Write(MessageClass.System, message);
        }

        void _chat_SimpleMessageEvent(Vha.Net.Chat chat, SimpleMessageEventArgs e)
        {
            // Dispatch message
            this.Write(MessageClass.System, e.Message);
        }
        #endregion
        #endregion // Internal
    }
}
