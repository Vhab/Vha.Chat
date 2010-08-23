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
using System.IO;
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
        /// Returns the toolset to manage the characters on the ignore list
        /// </summary>
        public Ignores Ignores { get { return this._ignores; } }
        /// <summary>
        /// Returns direct access to the chat instance
        /// </summary>
        public Net.Chat Chat { get { return this._chat; } }
        #endregion

        #region Events
        /// <summary>
        /// Fires when the internal state of the Context changed
        /// </summary>
        public event Handler<StateEventArgs> StateEvent;
        /// <summary>
        /// Fires when the user is required to select a character
        /// </summary>
        public event Handler<SelectCharacterEventArgs> SelectCharacterEvent;
        /// <summary>
        /// Fires when a recoverable error has occured, like incorrect arguments or failing to connect.
        /// </summary>
        public event Handler<Events.ErrorEventArgs> ErrorEvent;
        /// <summary>
        /// Fires whenever a new message is available
        /// </summary>
        public event Handler<MessageEventArgs> MessageEvent;
        /// <summary>
        /// Fires when a friend was first seen or was just added to the friends list
        /// </summary>
        public event Handler<FriendEventArgs> FriendAddedEvent;
        /// <summary>
        /// Fires when a character is removed from the friends list
        /// </summary>
        public event Handler<FriendEventArgs> FriendRemovedEvent;
        /// <summary>
        /// Fires when a friend who is already on the friends list changes status
        /// </summary>
        public event Handler<FriendEventArgs> FriendUpdatedEvent;
        /// <summary>
        /// Fires when a channel is seen for the first time
        /// </summary>
        public event Handler<ChannelEventArgs> ChannelJoinEvent;
        /// <summary>
        /// Fires when an already known channel changes status
        /// </summary>
        public event Handler<ChannelEventArgs> ChannelUpdatedEvent;
        /// <summary>
        /// Fires when this client joins a remote private channel
        /// </summary>
        public event Handler<PrivateChannelEventArgs> PrivateChannelJoinEvent;
        /// <summary>
        /// Fires when this client leaves a remote private channel
        /// </summary>
        public event Handler<PrivateChannelEventArgs> PrivateChannelLeaveEvent;
        /// <summary>
        /// Fires when this client is invited to join a remote private channel
        /// </summary>
        public event Handler<PrivateChannelInviteEventArgs> PrivateChannelInviteEvent;
        /// <summary>
        /// Fires when a character joins our local private channel
        /// </summary>
        public event Handler<PrivateChannelEventArgs> CharacterJoinEvent;
        /// <summary>
        /// Fires when a character leaves our local private channel
        /// </summary>
        public event Handler<PrivateChannelEventArgs> CharacterLeaveEvent;
#if !DEBUG
        /// <summary>
        /// Occures when an unexpected exception occured.
        /// Only use this to gracefully shut down the application.
        /// Don't make an attempt at recovering the application.
        /// </summary>
        public event Handler<Exception> ExceptionEvent;
#endif
        #endregion

        #region Attributes
        public Platform Platform { get { return this._platform; } }

        public OS OS { get { return this._os; } }

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

        public UInt32 CharacterID
        {
            get
            {
                if (this.State != ContextState.Connected &&
                    this.State != ContextState.CharacterSelection)
                    throw new InvalidOperationException("Context.CharacterID is not available while in state " + this.State);
                return this._characterID;
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

        public UInt32 OrganizationID
        {
            get
            {
                if (this.State != ContextState.Connected)
                    throw new InvalidOperationException("Context.OrganizationID is not available while in state " + this.State);
                return this._organizationID;
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
                        this.ErrorEvent(this, new Events.ErrorEventArgs(
                            ErrorType.Login,
                            "Attempting to connect to invalid dimension: '" + dimension + "'"));
                    return;
                }

                // Create chat connection
                OptionsProxy proxy = this.Options.Proxy;
                if (proxy == null || proxy.Type == ProxyType.Disabled)
                {
                    this._chat = new Vha.Net.Chat(dim.Address, dim.Port, account, password);
                }
                else
                {
                    // Construct proxy URI
                    UriBuilder uri = new UriBuilder();
                    uri.Scheme = proxy.Type.ToString().ToLower();
                    uri.Host = proxy.Address;
                    uri.Port = proxy.Port;
                    uri.UserName = proxy.Username;
                    uri.Password = proxy.Password;
                    // Create proxyfied chat
                    this._chat = new Vha.Net.Chat(dim.Address, dim.Port, account, password, uri.Uri);
                }
                // Set initial settings
                this._chat.AutoReconnect = false;
                this._chat.IgnoreCharacterLoggedIn = true;
                this._chat.UseThreadPool = false;
                this._chat.Tag = this;
                this._chat.LookupTimeout = 10000;

                // Hook events (including exceptions)
                // - State changes and more
                this._chat.StateChangeEvent += new StateChangeEventHandler(_chat_StateChangeEvent);
                this._chat.LoginErrorEvent += new LoginErrorEventHandler(_chat_LoginErrorEvent);
                this._chat.LoginCharlistEvent += new LoginCharlistEventHandler(_chat_LoginCharlistEvent);
                this._chat.LoginOKEvent += new LoginOKEventHandler(_chat_LoginOKEvent);
#if !DEBUG
                this._chat.ExceptionEvent += new ExceptionEventHandler(_chat_ExceptionEvent);
#endif
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
                this._chat.BroadcastMessageEvent += new BroadcastMessageEventHandler(_chat_BroadcastMessageEvent);
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
            if (this._chat == null) return false;
            BigInteger id = this._chat.GetChannelID(channel);
            if (id == 0) return false;
            lock (this._channels)
            {
                return this._channels.ContainsKey(id);
            }
        }
        /// <summary>
        /// Returns information about a channel by name
        /// </summary>
        /// <param name="channel">The full channel name (case-insensitive)</param>
        /// <returns>An instance of Channel or null on failure</returns>
        public Channel GetChannel(string channel)
        {
            if (this._chat == null) return null;
            BigInteger id = this._chat.GetChannelID(channel);
            if (id == 0) return null;
            lock (this._channels)
            {
                if (!this._channels.ContainsKey(id))
                    return null;
                return this._channels[id];
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
            string type = c.Type.ToString() + "Channel";
            MessageClass messageClass = (MessageClass)Enum.Parse(typeof(MessageClass), type, true);
            if (!Enum.IsDefined(typeof(MessageClass), messageClass)) return MessageClass.None;
            return messageClass;
        }
        /// <summary>
        /// Whether this context contains the given character as friend
        /// </summary>
        /// <param name="friend">The name of the character (case-insensitive)</param>
        /// <returns>true if the given character is a friend, false if not</returns>
        public bool HasFriend(string character)
        {
            if (this._chat == null) return false;
            UInt32 id = this._chat.GetCharacterID(character);
            if (id == 0) return false;
            lock (this._friends)
            {
                return this._friends.ContainsKey(id);
            }
        }
        /// <summary>
        /// Returns information about a friend by name
        /// </summary>
        /// <param name="character">The name of the character (case-insensitive)</param>
        /// <returns>An isntance of Friend or null on failure</returns>
        public Friend GetFriend(string character)
        {
            if (this._chat == null) return null;
            UInt32 id = this._chat.GetCharacterID(character);
            if (id == 0) return null;
            lock (this._friends)
            {
                if (!this._friends.ContainsKey(id))
                    return null;
                return this._friends[id];
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
            if (channel == this.Character)
                return true;
            if (this._chat == null) return false;
            UInt32 id = this._chat.GetCharacterID(channel);
            if (id == 0) return false;
            lock (this._privateChannels)
            {
                return this._privateChannels.ContainsKey(id);
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
            if (channel == this.Character)
                return new PrivateChannel(this.CharacterID, this.Character, true);
            if (this._chat == null) return null;
            UInt32 id = this._chat.GetCharacterID(channel);
            if (id == 0) return null;
            lock (this._privateChannels)
            {
                if (!this._privateChannels.ContainsKey(id))
                    return null;
                return this._privateChannels[id];
            }
        }
        /// <summary>
        /// Whether this context contains the given character as guest
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HasGuest(string character)
        {
            character = Format.UppercaseFirst(character);
            if (character == this.Character)
                return true;
            if (this._chat == null) return false;
            UInt32 id = this._chat.GetCharacterID(character);
            if (id == 0) return false;
            lock (this._guests)
            {
                return this._guests.Contains(id);
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
            MessageEventArgs args = new MessageEventArgs(DateTime.Now, source, messageClass, message);
            // Log message in history buffer
            MessageTarget target = source.GetTarget();
            if (target.Valid)
            {
                lock (this._messageHistory)
                {
                    if (!this._messageHistory.ContainsKey(target))
                        this._messageHistory.Add(target, new List<MessageEventArgs>());
                    this._messageHistory[target].Add(args);
                    while (this._messageHistory[target].Count > this.Options.MessageBuffer)
                        this._messageHistory[target].RemoveAt(0);
                }
            }
            // Dispatch event
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

        /// <summary>
        /// Returns the last X incoming and outgoing messages for a given message target.
        /// The returned array starts with the oldest message first.
        /// </summary>
        /// <param name="target">A character, private channel or channel target</param>
        /// <returns>A list of X most recent messages to the given target</returns>
        public MessageEventArgs[] GetHistory(MessageTarget target)
        {
            lock (this._messageHistory)
            {
                // Early out
                if (!this._messageHistory.ContainsKey(target))
                    return new MessageEventArgs[] {};
                // Return messages
                return this._messageHistory[target].ToArray();
            }
        }
        #endregion

        #region Internal
        private Platform _platform;
        private OS _os;
        private Configuration _configuration;
        private Options _options;
        private Input _input = null;
        private Ignores _ignores = null;
        private Net.Chat _chat = null;
        private string _dimension = null;
        private string _account = null;
        private string _character = null;
        private UInt32 _characterID = 0;
        private string _organization = null;
        private UInt32 _organizationID = 0;
        private ContextState _state = ContextState.Disconnected;
        private bool _disconnecting = false;

        private Dictionary<BigInteger, Channel> _channels;
        private Dictionary<UInt32, Friend> _friends;
        private Dictionary<UInt32, PrivateChannel> _privateChannels;
        private List<UInt32> _guests;
        private Dictionary<MessageTarget, List<MessageEventArgs>> _messageHistory;

        internal Context()
        {
            // Detect platform
            this._platform = Platform.DotNet;
            if (Type.GetType("Mono.Runtime") != null)
                this._platform = Platform.Mono;

            // Detect OS
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    this._os = OS.MacOS;
                    break;
                case PlatformID.Unix:
                    this._os = OS.Unix;
                    break;
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    this._os = OS.Windows;
                    break;
                default:
                    this._os = OS.Unknown;
                    break;
            } 

            // Read initial configuration
            Data.Base configuration = Data.Base.Load("Configuration.xml");
            if (configuration == null) configuration = new Data.ConfigurationV1();
            while (configuration.CanUpgrade) configuration = configuration.Upgrade();
            this._configuration = new Configuration(configuration);

            // Ensure the options directory exists
            if (!Directory.Exists(this.Configuration.OptionsPath))
            {
                Directory.CreateDirectory(this.Configuration.OptionsPath);
            }

            // Initialize objects
            this._options = new Options(this);
            this._channels = new Dictionary<BigInteger, Channel>();
            this._friends = new Dictionary<UInt32, Friend>();
            this._privateChannels = new Dictionary<UInt32, PrivateChannel>();
            this._guests = new List<UInt32>();
            this._messageHistory = new Dictionary<MessageTarget, List<MessageEventArgs>>();
            this._ignores = new Ignores(this);

            // Create input
            this._input = new Input(this, "/");
            this.Input.RegisterCommand(new AwayCommand(this));
            this.Input.RegisterCommand(new CCCommand());
            this.Input.RegisterCommand(new ColorsCommand());
            this.Input.RegisterCommand(new CrashCommand());
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
            this.Input.RegisterCommand(new ReplyCommand(this));
            this.Input.RegisterCommand(new TextCommand());
            this.Input.RegisterCommand(new UnignoreCommand());
            this.Input.RegisterCommand(new UnmuteCommand());
            this.Input.RegisterCommand(new WhoisCommand());
        }

        #region Chat callbacks
        void _chat_LoginCharlistEvent(Vha.Net.Chat chat, LoginChararacterListEventArgs e)
        {
            // Only fire the character selection event when no character has been pre-selected
            if (string.IsNullOrEmpty(chat.Character))
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
                    this._characterID = args.Character.ID;
                    this._chat.SendLoginCharacter(charactersMap[args.Character]);

                    // Mark as 'recently used'
                    OptionsAccount acc = this.Options.GetAccount(this.Account, true);
                    acc.Name = this.Options.LastAccount = this._account;
                    acc.Dimension = this.Options.LastDimension = this._dimension;
                    acc.Character = this._character;
                    this.Options.Save();
                }
                else
                {
                    // If no character was selected, there's no reason to remain connected
                    this._chat.Disconnect(true);
                    return;
                }
            }
            // Dispatch state change event
            ContextState previousState = this._state;
            this._state = ContextState.CharacterSelection;
            if (this.StateEvent != null)
                this.StateEvent(this, new StateEventArgs(this._state, previousState));
        }

        void _chat_LoginErrorEvent(Vha.Net.Chat chat, LoginErrorEventArgs e)
        {
            // Notify listeners of the error
            if (this.ErrorEvent != null)
                this.ErrorEvent(this, new Events.ErrorEventArgs(ErrorType.Login, e.Error));
        }

        void _chat_LoginOKEvent(Vha.Net.Chat chat, EventArgs e)
        {
            // Prevent duplicate events
            if (this._state == ContextState.Connected)
                return;
            // Update state
            ContextState previousState = this._state;
            this._state = ContextState.Connected;
            // This tastes like more
            this._chat.AutoReconnect = true;
            // Notify state change
            if (this.StateEvent != null)
                this.StateEvent(this, new StateEventArgs(this._state, previousState));
        }

        void _chat_StateChangeEvent(Vha.Net.Chat chat, StateChangeEventArgs e)
        {
            ContextState state = this._state;
            ContextState previousState = this._state;
            lock (this)
            {
                // Translate state
                switch (e.State)
                {
                    case ChatState.CharacterSelect:
                        state = ContextState.CharacterSelection;
                        break;
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
                // No state change? We stopped caring!
                if (previousState == state)
                    return;
                // Handle state change
                switch (state)
                {
                    case ContextState.Disconnected:
                        lock (this._messageHistory)
                            this._messageHistory.Clear();
                        this._chat.AutoReconnect = false;
                        this._chat.ClearEvents();
                        this._chat = null;
                        this._dimension = null;
                        this._account = null;
                        this._character = null;
                        // Fall through to the next case
                        goto case ContextState.Reconnecting;
                    case ContextState.Reconnecting:
                        this._organization = null;
                        this._organizationID = 0;
                        this._characterID = 0;
                        lock (this._friends)
                            this._friends.Clear();
                        lock (this._channels)
                            this._channels.Clear();
                        lock (this._privateChannels)
                            this._privateChannels.Clear();
                        lock (this._guests)
                            this._guests.Clear();
                        break;
                }
            }
            // Notify state change
            this._state = state;
            if (this.StateEvent != null)
                this.StateEvent(this, new StateEventArgs(this._state, previousState));
        }

#if !DEBUG
        void _chat_ExceptionEvent(Vha.Net.Chat chat, Exception e)
        {
            if (this.ExceptionEvent != null)
                this.ExceptionEvent(this, e);
        }
#endif

        void _chat_ChannelStatusEvent(Vha.Net.Chat chat, ChannelStatusEventArgs e)
        {
            Channel channel = e.GetChannel();
            bool joined, updated;
            lock (this._channels)
            {
                joined = !this._channels.ContainsKey(e.ID);
                if (joined) this._channels.Add(e.ID, channel);
                updated = !this._channels[e.ID].Equals(channel);
                this._channels[e.ID] = channel;
            }
            // Detect organization
            if (e.Type == ChannelType.Organization)
            {
                this._organization = e.Name;
                this._organizationID = (UInt32)e.ID.IntValue();
            }
            // Fire events
            if (this.ChannelJoinEvent != null && joined)
                this.ChannelJoinEvent(this, new ChannelEventArgs(channel, true));
            if (this.ChannelUpdatedEvent != null && updated)
                this.ChannelUpdatedEvent(this, new ChannelEventArgs(channel, false));
        }

        void _chat_PrivateChannelStatusEvent(Vha.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            PrivateChannel channel = e.GetPrivateChannel();
            // Handle locally
            if (e.Local)
            {
                bool joined = false;
                bool left = false;
                lock (this._guests)
                {
                    if (this._guests.Contains(e.CharacterID) && !e.Join)
                    {
                        left = true;
                        this._guests.Remove(e.CharacterID);
                    }
                    else if (!this._guests.Contains(e.CharacterID) && e.Join)
                    {
                        joined = true;
                        this._guests.Add(e.CharacterID);
                    }
                }
                // Dispatch events
                if (this.CharacterJoinEvent != null && joined)
                {
                    this.CharacterJoinEvent(this, new PrivateChannelEventArgs(channel, e.Character, true, true));
                }
                if (this.CharacterLeaveEvent != null && left)
                {
                    this.CharacterLeaveEvent(this, new PrivateChannelEventArgs(channel, e.Character, false, true));
                }
            }
            // Handle remote
            else
            {
                // Handle other characters
                if (e.CharacterID != this.CharacterID)
                {

                }
                // Handle the local character
                {
                    bool joined = false;
                    bool left = false;
                    lock (this._privateChannels)
                    {
                        if (this._privateChannels.ContainsKey(e.ChannelID) && !e.Join)
                        {
                            left = true;
                            this._privateChannels.Remove(e.ChannelID);
                        }
                        else if (!this._privateChannels.ContainsKey(e.ChannelID) && e.Join)
                        {
                            joined = true;
                            this._privateChannels.Add(e.ChannelID, channel);
                        }
                    }
                    // Dispatch events
                    if (joined)
                    {
                        if (this.PrivateChannelJoinEvent != null)
                            this.PrivateChannelJoinEvent(this, new PrivateChannelEventArgs(channel, e.Character, true, false));
                    }
                    else if (left)
                    {
                        if (this.PrivateChannelLeaveEvent != null)
                            this.PrivateChannelLeaveEvent(this, new PrivateChannelEventArgs(channel, e.Character, false, false));
                    }
                    else
                    {
                        // Nothing happened, don't bother writing that message!
                        return;
                    }
                }
            }
            // Send messages
            if (e.Join)
            {
                this.Write(new MessageSource(MessageType.PrivateChannel, e.Channel, null, false), MessageClass.PrivateChannel,
                    e.Character + " joined the channel");
            }
            else
            {
                this.Write(new MessageSource(MessageType.PrivateChannel, e.Channel, null, false), MessageClass.PrivateChannel,
                    e.Character + " left the channel");
            }
        }

        void _chat_PrivateChannelRequestEvent(Vha.Net.Chat chat, PrivateChannelRequestEventArgs e)
        {
            // Check for ignores
            if (this.Ignores.Contains(e.Character))
                return;
            // Some sensible checks
            PrivateChannel channel = e.GetPrivateChannel();
            bool error = false;
            lock (this._privateChannels)
            {
                error = this._privateChannels.ContainsKey(e.CharacterID);
            }
            if (error)
            {
                this.Write(MessageClass.Error, "Unexpected invite to private channel from: " + e.Character);
                return;
            }
            // Dispatch invite event
            PrivateChannelInviteEventArgs args = new PrivateChannelInviteEventArgs(e, channel);
            if (this.PrivateChannelInviteEvent != null)
                this.PrivateChannelInviteEvent(this, args);
        }

        void _chat_FriendRemovedEvent(Vha.Net.Chat chat, CharacterIDEventArgs e)
        {
            Friend friend = null;
            bool removed = false;
            lock (this._friends)
            {
                if (this._friends.ContainsKey(e.CharacterID))
                {
                    removed = true;
                    friend = this._friends[e.CharacterID];
                    this._friends.Remove(e.CharacterID);
                }
            }
            if (this.FriendRemovedEvent != null && removed)
                this.FriendRemovedEvent(this, new FriendEventArgs(friend, false));
        }

        void _chat_FriendStatusEvent(Vha.Net.Chat chat, FriendStatusEventArgs e)
        {
            // Ignore temporary friends
            if (e.Temporary) return;
            // Ignore invalid friends
            if (string.IsNullOrEmpty(e.Character)) return;
            // Handle friend update
            Friend friend = e.GetFriend();
            bool added, updated;
            lock (this._friends)
            {
                added = !this._friends.ContainsKey(e.CharacterID);
                if (added) this._friends.Add(e.CharacterID, friend);
                updated = !this._friends[e.CharacterID].Equals(friend);
                this._friends[e.CharacterID] = friend;
            }
            if (this.FriendAddedEvent != null && added)
                this.FriendAddedEvent(this, new FriendEventArgs(friend, true));
            if (this.FriendUpdatedEvent != null && updated)
                this.FriendUpdatedEvent(this, new FriendEventArgs(friend, false));
        }

        void _chat_PrivateChannelMessageEvent(Vha.Net.Chat chat, PrivateChannelMessageEventArgs e)
        {
            // Check for ignores
            if (this.Ignores.Contains(e.Character))
                return;
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.PrivateChannel, e.Channel, e.Character, e.Character == this.Character);
            this.Write(source, MessageClass.PrivateChannel, e.Message);
        }

        void _chat_ChannelMessageEvent(Vha.Net.Chat chat, ChannelMessageEventArgs e)
        {
            Channel channel = this.GetChannel(e.Channel);
            // Check if channel is muted
            if (channel != null && channel.Muted)
                return;
            // Check for ignores
            if (this.Ignores.Contains(e.Character))
                return;
            // Descramble
            string message = e.Message;
            if (e.Message.StartsWith("~"))
            {
                MDB.Message parsedMessage = null;
                try { parsedMessage = MDB.Parser.Decode(e.Message); }
                catch (Exception ex)
                {
                    this.Write(MessageClass.Error, "Error while decoding message: " + ex.Message);
                }
                if (parsedMessage != null && !string.IsNullOrEmpty(parsedMessage.Value))
                    message = parsedMessage.Value;
            }
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.Channel, e.Channel, e.Character, e.Character == this.Character);
            this.Write(source, this.GetChannelClass(e.Channel), message);
        }

        void _chat_PrivateMessageEvent(Vha.Net.Chat chat, PrivateMessageEventArgs e)
        {
            // Check for ignores
            if (this.Ignores.Contains(e.Character))
                return;
            // Treat 'null-characters' as broadcasts
            if (e.CharacterID == 0)
            {
                this.Write(MessageClass.BroadcastMessage, e.Message);
                return;
            }
            // Dispatch message
            MessageSource source = new MessageSource(MessageType.Character, null, e.Character, e.Outgoing);
            this.Write(source, MessageClass.PrivateMessage, e.Message);
        }

        void _chat_SystemMessageEvent(Vha.Net.Chat chat, SystemMessageEventArgs e)
        {
            // Descramble message
            MDB.Message message = null;
            try { message = MDB.Parser.Decode((int)e.CategoryID, (int)e.MessageID, e.Arguments, null); }
            catch (Exception ex)
            {
                this.Write(MessageClass.Error, "Error while decoding message: " + ex.Message);
            }
            // Apply ignore filter to "received offline message from" messages
            if (e.MessageID == (uint)SystemMessageType.IncommingOfflineMessage)
            {
                string character = message.Arguments[(int)IncomingOfflineMessageArgs.Name].ToString();
                // Check ignored characters list
                if (this.Ignores.Contains(character))
                    return;
            }
            // Failed to get the entry
            if (string.IsNullOrEmpty(message.Value))
            {
                this.Write(MessageClass.Error, "Unknown system message " + e.CategoryID + ":" + e.MessageID);
                return;
            }
            // Dispatch message
            this.Write(MessageClass.SystemMessage, message.Value);
        }

        void _chat_SimpleMessageEvent(Vha.Net.Chat chat, SimpleMessageEventArgs e)
        {
            // Dispatch message
            this.Write(MessageClass.BroadcastMessage, e.Message);
        }

        void _chat_BroadcastMessageEvent(Vha.Net.Chat chat, BroadcastMessageEventArgs e)
        {
            // Dispatch message
            this.Write(MessageClass.BroadcastMessage, e.Message);
        }

        #endregion
        #endregion // Internal
    }
}
