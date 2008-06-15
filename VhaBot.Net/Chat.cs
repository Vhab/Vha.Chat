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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using VhaBot.Common;

namespace VhaBot.Net
{
    public class Chat
    {
        public bool AutoReconnect = true;
        public int ReconnectDelay = 5000;
        public int PingInterval = 60000;
        public double FastPacketDelay = 10;
        public double SlowPacketDelay = 2200;

        // Events
        public event AmdMuxInfoEventHandler AmdMuxInfoEvent;
        public event AnonVicinityEventHandler AnonVicinityEvent;
        public event FriendStatusEventHandler FriendStatusEvent;
        public event FriendRemovedEventHandler FriendRemovedEvent;
        public event CharacterIDEventHandler CharacterIDEvent;
        public event PrivateChannelRequestEventHandler PrivateChannelRequestEvent;
        public event NameLookupEventHandler NameLookupEvent;
        public event ForwardEventHandler ForwardEvent;
        public event ChannelJoinEventHandler ChannelJoinEvent;
        public event ChannelMessageEventHandler ChannelMessageEvent;
        public event SystemMessageEventHandler SystemMessageEvent;
        public event SimpleMessageEventHandler SimpleMessageEvent;
        public event LoginOKEventHandler LoginOKEvent;
        public event UnknownPacketEventHandler UnknownPacketEvent;
        public event PrivateChannelStatusEventHandler PrivateChannelStatusEvent;
        public event PrivateChannelMessageEventHandler PrivateChannelMessageEvent;
        public event TellEventHandler TellEvent;
        public event LoginSeedEventHandler LoginSeedEvent;
        public event LoginCharlistEventHandler LoginCharlistEvent;
        public event StatusChangeEventHandler StatusChangeEvent;

        protected string _account = string.Empty;
        protected string _password = string.Empty;
        protected string _character = string.Empty;
        protected ChatState _state = ChatState.Disconnected;
        protected UInt32 _id = 0;
        protected BigInteger _guildid = 0;
        protected string _guild = string.Empty;
        protected bool _ignoreOfflineTells = true;
        protected bool _ignoreAfkTells = true;
        protected bool _ignoreCharacterLoggedIn = true;
        protected bool _removeTempFriends = true;
        protected Thread _receiveThread;
        protected Thread _sendThread;
        protected Socket _socket;
        protected Dictionary<UInt32, String> _users;
        protected Dictionary<BigInteger, Channel> _channels;
        protected string _serverAddress;
        protected int _port;
        protected PacketQueue _fastQueue;
        protected PacketQueue _slowQueue;
        protected bool _closing = false;
        protected System.Timers.Timer _reconnectTimer;
        protected ManualResetEvent _lookupReset;
        protected System.Timers.Timer _pingTimer;
        protected List<UInt32> _offlineTells;
        protected DateTime _lastPong = DateTime.Now;

        public UInt32 ID { get { return this._id; } }
        public string Account { get { return this._account; } }
        public string Character { get { return this._character; } }
        public string Server { get { return this._serverAddress; } }
        public int Port { get { return this._port; } }
        public string Guild { get { return this._guild; } }
        public BigInteger GuildID { get { lock (this) { return this._guildid; } } }
        public ChatState State { get { return this._state; } }
        public bool IgnoreOfflineTells
        {
            get { return this._ignoreOfflineTells; }
            set { this._ignoreOfflineTells = value; }
        }
        public bool IgnoreAfkTells
        {
            get { return this._ignoreAfkTells; }
            set { this._ignoreAfkTells = value; }
        }
        public bool IgnoreCharacterLoggedIn
        {
            get { return this._ignoreCharacterLoggedIn; }
            set { this._ignoreCharacterLoggedIn = value; }
        }
        public int SlowQueueCount { get { return this._slowQueue.Count; } }
        public int FastQueueCount { get { return this._fastQueue.Count; } }

        public Chat(string server, int port, string account, string password)
        {
            this._serverAddress = server;
            this._port = port;
            this._account = account;
            this._password = password;
        }
        public Chat(string server, int port, string account, string password, string character)
        {
            this._serverAddress = server;
            this._port = port;
            this._account = account;
            this._password = password;
            this._character = character;
        }

        // Get this thing ready for running
        protected virtual void PrepareChat()
        {
            lock (this)
            {
                if (this._receiveThread != null)
                {
                    if (this._receiveThread.ThreadState == System.Threading.ThreadState.Running)
                    {
                        this._receiveThread.Abort();
                        this._receiveThread.Join(500);
                    }
                }
                if (this._sendThread != null)
                {
                    if (this._sendThread.ThreadState == System.Threading.ThreadState.Running)
                    {
                        this._sendThread.Abort();
                        this._sendThread.Join(500);
                    }
                }
                if (this._socket != null && this._socket.Connected)
                {
                    this._socket.Close();
                }
                this._lookupReset = new ManualResetEvent(false);
                this._receiveThread = new Thread(new ThreadStart(this.RunReceiver));
                this._receiveThread.IsBackground = true;
                this._sendThread = new Thread(new ThreadStart(this.RunSender));
                this._sendThread.IsBackground = true;
                this._users = new Dictionary<UInt32, String>();
                this._channels = new Dictionary<BigInteger, Channel>();
                this._offlineTells = new List<UInt32>();
                this._fastQueue = new PacketQueue();
                this._fastQueue.delay = this.FastPacketDelay;
                this._slowQueue = new PacketQueue();
                this._slowQueue.delay = this.SlowPacketDelay;
                this._reconnectTimer = new System.Timers.Timer();
                this._reconnectTimer.AutoReset = false;
                this._reconnectTimer.Interval = this.ReconnectDelay;
                this._reconnectTimer.Elapsed += new ElapsedEventHandler(OnReconnectEvent);
                this._pingTimer = new System.Timers.Timer();
                this._pingTimer.AutoReset = true;
                this._pingTimer.Interval = this.PingInterval;
                this._pingTimer.Elapsed += new ElapsedEventHandler(OnPingTimerEvent);
                this._lastPong = DateTime.Now;
            }
        }

        public virtual bool Connect()
        {
            lock (this)
            {
                if (this._socket != null && this._socket.Connected)
                {
                    this.Debug("Already Connected", "[Error]");
                    return false;
                }
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Connecting));
                this._closing = false;
                this.PrepareChat();

                this.Debug("Connecting to dimension: " + this._serverAddress + ":" + this._port, "[Auth]");
                try
                {
                    IPHostEntry host = Dns.GetHostEntry(this._serverAddress);
                    foreach (IPAddress addy in host.AddressList)
                    {
                        IPEndPoint ipe = new IPEndPoint(addy, this._port);
                        Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        tempSocket.Connect(ipe);

                        if (tempSocket.Connected)
                        {
                            this.Debug("Connected to " + ipe.ToString(), "[Socket]");
                            this._socket = tempSocket;
                            this._receiveThread.Start();
                            this._sendThread.Start();
                            this._pingTimer.Start();
                            return true;
                        }
                        this.Debug("Failed connecting to " + ipe.ToString(), "[Socket]");
                    }
                }
                catch
                {
                    this.Debug("Unknown error during connecting", "[Error]");
                }
            }
            this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
            return false;
        }

        public virtual void Disconnect()
        {
            this._closing = true;
            if (this._reconnectTimer != null) { this._reconnectTimer.Stop(); }
            if (this._pingTimer != null) { this._pingTimer.Stop(); }
            if (this._receiveThread != null)
            {
                if (this._receiveThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    this._receiveThread.Abort();
                    this._receiveThread.Join(new TimeSpan(0, 0, 5));
                }
                this._receiveThread = null;
            }
            if (this._sendThread != null)
            {
                if (this._sendThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    this._sendThread.Abort();
                    this._sendThread.Join(new TimeSpan(0, 0, 5));
                }
                this._sendThread = null;
            }
            if (this._socket != null && this._socket.Connected) { this._socket.Close(); }
            this._socket = null;
            this._lookupReset = null;
            this._users.Clear();
            this._users = null;
            this._channels.Clear();
            this._channels = null;
            this._offlineTells.Clear();
            this._offlineTells = null;
            this._fastQueue = null;
            this._slowQueue = null;
            this._reconnectTimer = null;
            this._pingTimer = null;

            this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
            this._state = ChatState.Disconnected;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Receive Thread
        protected void RunReceiver()
        {
            this.Debug("Started", "[ReceiveThread]");
            try
            {
                while (true)
                {
                    if (!_socket.Connected)
                    {
                        throw new Exception("Connection Lost");
                    }
                    byte[] buffer = new byte[4];
                    int receivedBytes = this._socket.Receive(buffer, buffer.Length, 0);
                    if (receivedBytes == 0 || !this._socket.Connected)
                    {
                        throw new Exception("Connection Lost");
                    }
                    Packet.Type type = (Packet.Type)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 0));
                    short lenght = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 2));
                    if (lenght == 0)
                    {
                        ParsePacketData packetData = new ParsePacketData(type, lenght, null);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ParsePacket), packetData);
                    }
                    else
                    {
                        buffer = new byte[lenght];
                        int bytesLeft = lenght;
                        while (bytesLeft > 0)
                        {
                            receivedBytes = this._socket.Receive(buffer, lenght - bytesLeft, bytesLeft, 0);
                            bytesLeft -= receivedBytes;
                            Thread.Sleep(10);
                        }
                        ParsePacketData packetData = new ParsePacketData(type, lenght, buffer);
                        if (packetData.type == Packet.Type.MESSAGE_SYSTEM || packetData.type == Packet.Type.NAME_LOOKUP)
                            this.ParsePacket(packetData);
                        else
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ParsePacket), packetData);
                    }
                    Thread.Sleep(10);
                }
            }
            catch { }
            finally
            {
                this.Debug("Stopped!", "[ReceiveThread]");
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
            }
        }
        // Send Thread
        protected void RunSender()
        {
            this.Debug("Started", "[SendThread]");
            try
            {
                while (true)
                {
                    if (this._socket == null || this._socket.Connected == false)
                    {
                        throw new Exception("Disconnected");
                    }
                    if (this._fastQueue.Available || this._slowQueue.Available)
                    {
                        Packet packet;
                        if (this._slowQueue.Available)
                        {
                            packet = this._slowQueue.Dequeue();
                        }
                        else
                        {
                            packet = this._fastQueue.Dequeue();
                        }
                        byte[] data = packet.GetBytes();
                        short len = (short)data.Length;
                        byte[] buffer = new byte[len + 4];
                        BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)packet.PacketType)).CopyTo(buffer, 0);
                        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(len)).CopyTo(buffer, 2);
                        data.CopyTo(buffer, 4);
                        _socket.Send(buffer, buffer.Length, 0);
                        if (packet.PacketType == Packet.Type.PRIVATE_MESSAGE)
                        {
                            try
                            {
                                TellPacket tell = (TellPacket)packet;
                                this.OnTellEvent(new TellEventArgs(tell.CharacterID, this.GetUserName(tell.CharacterID), tell.Message, true));
                            }
                            catch { }
                        }
                        Thread.Sleep((int)this.FastPacketDelay);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch { }
            finally
            {
                this.Debug("Stopped!", "[SendThread]");
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
            }
        }

        protected virtual void ParsePacket(Object o)
        {
            ParsePacketData packetData = (ParsePacketData)o;
            Packet packet = null;

            // figure out the packet type and raise an event.
            switch (packetData.type)
            {
                case Packet.Type.PING:
                    OnPongEvent();
                    break;
                case Packet.Type.LOGIN_SEED:
                    packet = new LoginMessagePacket(packetData.type, packetData.data);
                    OnLoginSeedEvent(
                        new LoginMessageEventArgs(
                        ((LoginMessagePacket)packet).Seed
                        ));
                    break;
                case Packet.Type.SYSTEM_MESSAGE:
                case Packet.Type.LOGIN_ERROR:
                    packet = new SimpleStringPacket(packetData.type, packetData.data);
                    OnSimpleMessageEvent(
                        new SimpleStringPacketEventArgs(
                        ((SimpleStringPacket)packet).Message
                        ));
                    break;
                case Packet.Type.LOGIN_CHARACTERLIST:
                    packet = new LoginCharacterListPacket(packetData.type, packetData.data);
                    OnLoginCharacterListEvent(
                        new LoginChararacterListEventArgs(
                        ((LoginCharacterListPacket)packet).Characters
                        ));
                    break;
                case Packet.Type.FRIEND_REMOVED:
                    packet = new SimpleIdPacket(packetData.type, packetData.data);
                    OnFriendRemovedEvent(
                        new CharacterIDEventArgs(
                        ((SimpleIdPacket)packet).CharacterID,
                        this.GetUserName(((SimpleIdPacket)packet).CharacterID)
                        ));
                    break;
                case Packet.Type.CLIENT_UNKNOWN:
                    packet = new SimpleIdPacket(packetData.type, packetData.data);
                    OnCharacterIDEvent(
                        new CharacterIDEventArgs(
                        ((SimpleIdPacket)packet).CharacterID,
                        this.GetUserName(((SimpleIdPacket)packet).CharacterID)
                        ));
                    break;
                case Packet.Type.PRIVATE_CHANNEL_INVITE:
                case Packet.Type.PRIVATE_CHANNEL_KICK:
                case Packet.Type.PRIVATE_CHANNEL_PART:
                    System.Diagnostics.Trace.WriteLine(BitConverter.ToString(packetData.data));
                    packet = new PrivateChannelStatusPacket(packetData.type, packetData.data);
                    OnPrivateChannelRequestEvent(
                        new PrivateChannelRequestEventArgs(
                        ((PrivateChannelStatusPacket)packet).CharacterID,
                        this.GetUserName(((PrivateChannelStatusPacket)packet).CharacterID),
                        ((PrivateChannelStatusPacket)packet).Joined
                        ));
                    break;
                case Packet.Type.LOGIN_OK:
                    packet = new EmptyPacket(packetData.type);
                    OnLoginOKEvent();
                    break;
                case Packet.Type.CLIENT_NAME:
                    packet = new ClientNamePacket(packetData.type, packetData.data);
                    OnNameLookupEvent(
                        new NameLookupEventArgs(
                        ((ClientNamePacket)packet).CharacterID,
                        ((ClientNamePacket)packet).CharacterName
                        ));
                    break;
                case Packet.Type.NAME_LOOKUP:
                    packet = new NameLookupPacket(packetData.type, packetData.data);
                    OnNameLookupEvent(
                        new NameLookupEventArgs(
                        ((NameLookupPacket)packet).CharacterID,
                        ((NameLookupPacket)packet).CharacterName
                        ));
                    break;
                case Packet.Type.PRIVATE_MESSAGE:
                case Packet.Type.VICINITY_MESSAGE:
                    packet = new TellPacket(packetData.type, packetData.data);
                    OnTellEvent(
                        new TellEventArgs(
                        ((TellPacket)packet).CharacterID,
                        this.GetUserName(((TellPacket)packet).CharacterID),
                        ((TellPacket)packet).Message,
                        false
                        ));
                    break;
                case Packet.Type.ANON_MESSAGE:
                    packet = new AnonVicinityPacket(packetData.type, packetData.data);
                    OnAnonVicinityEvent(
                        new AnonVicinityEventArgs(
                        ((AnonVicinityPacket)packet).UnknownString,
                        ((AnonVicinityPacket)packet).Message
                        ));
                    break;
                case Packet.Type.FRIEND_STATUS:
                    packet = new FriendStatusPacket(packetData.type, packetData.data);
                    OnFriendStatusEvent(
                        new FriendStatusEventArgs(
                        ((FriendStatusPacket)packet).CharacterID,
                        this.GetUserName(((FriendStatusPacket)packet).CharacterID),
                        ((FriendStatusPacket)packet).Status,
                        ((FriendStatusPacket)packet).Level,
                        ((FriendStatusPacket)packet).ID2,
                        ((FriendStatusPacket)packet).Class
                        ));
                    break;
                case Packet.Type.CHANNEL_JOIN:
                    packet = new ChannelJoinPacket(packetData.type, packetData.data);
                    OnChannelJoinEvent(
                        new ChannelJoinEventArgs(
                        ((ChannelJoinPacket)packet).ID,
                        ((ChannelJoinPacket)packet).Name,
                        ((ChannelJoinPacket)packet).Mute,
                        ((ChannelJoinPacket)packet).Logging,
                        ((ChannelJoinPacket)packet).Type
                        ));
                    break;
                case Packet.Type.PRIVATE_CHANNEL_CLIENTJOIN:
                case Packet.Type.PRIVATE_CHANNEL_CLIENTPART:
                    packet = new PrivateChannelStatusPacket(packetData.type, packetData.data);
                    OnPrivateChannelStatusEvent(
                        new PrivateChannelStatusEventArgs(
                        ((PrivateChannelStatusPacket)packet).ChannelID,
                        this.GetUserName(((PrivateChannelStatusPacket)packet).ChannelID),
                        ((PrivateChannelStatusPacket)packet).CharacterID,
                        this.GetUserName(((PrivateChannelStatusPacket)packet).CharacterID),
                        ((PrivateChannelStatusPacket)packet).Joined
                        ));
                    break;
                case Packet.Type.PRIVGRP_MESSAGE:
                    packet = new PrivateChannelMessagePacket(packetData.type, packetData.data);
                    OnPrivateChannelMessageEvent(
                        new PrivateChannelMessageEventArgs(
                        ((PrivateChannelMessagePacket)packet).ChannelID,
                        this.GetUserName(((PrivateChannelMessagePacket)packet).ChannelID),
                        ((PrivateChannelMessagePacket)packet).CharacterID,
                        this.GetUserName(((PrivateChannelMessagePacket)packet).CharacterID),
                        ((PrivateChannelMessagePacket)packet).Message,
                        ((PrivateChannelMessagePacket)packet).ChannelID == this._id
                        ));
                    break;
                case Packet.Type.CHANNEL_MESSAGE:
                    packet = new ChannelMessagePacket(packetData.type, packetData.data);
                    OnChannelMessageEvent(
                        new ChannelMessageEventArgs(
                        ((ChannelMessagePacket)packet).ChannelID,
                        this.GetChannelName(((ChannelMessagePacket)packet).ChannelID),
                        ((ChannelMessagePacket)packet).CharacterID,
                        this.GetUserName(((ChannelMessagePacket)packet).CharacterID),
                        ((ChannelMessagePacket)packet).Message,
                        this.GetChannelType(((ChannelMessagePacket)packet).ChannelID)
                        ));
                    break;
                case Packet.Type.FORWARD:
                    packet = new ForwardPacket(packetData.type, packetData.data);
                    OnForwardEvent(
                        new ForwardEventArgs(
                        ((ForwardPacket)packet).ID1,
                        ((ForwardPacket)packet).ID2
                        ));
                    break;
                case Packet.Type.AMD_MUX_INFO:
                    packet = new AmdMuxInfoPacket(packetData.type, packetData.data);
                    OnAmdMuxInfoEvent(
                        new AmdMuxInfoEventArgs(
                        ((AmdMuxInfoPacket)packet).Message
                        ));
                    break;
                case Packet.Type.MESSAGE_SYSTEM:
                    packet = new SystemMessagePacket(packetData.type, packetData.data);
                    OnSystemMessageEvent(
                        new SystemMessagePacketEventArgs(
                        ((SystemMessagePacket)packet).ClientID,
                        ((SystemMessagePacket)packet).WindowID,
                        ((SystemMessagePacket)packet).MessageID
                        ));
                    break;
                default:
                    if (packetData.type == Packet.Type.NULL && packetData.data != null)
                    {
                        if (BitConverter.ToInt32(packetData.data, 0) == 0 && packetData.data.Length == 4)
                        {
                            Trace.WriteLine("Disconnect packet received.", "[Debug]");
                            this.Disconnect();
                            return;
                        }
                    }
                    packet = new UnknownPacket(packetData.type, packetData.data);
                    OnUnknownPacketEvent(
                        new UnknownPacketEventArgs(
                        ((UnknownPacket)packet).PacketType,
                        ((UnknownPacket)packet).UnknownData
                        ));
                    break;
            } // End switch (packet type)
        }

        #region Events
        protected virtual void OnUnknownPacketEvent(UnknownPacketEventArgs e)
        {
            if (this.UnknownPacketEvent != null)
                this.UnknownPacketEvent(this, e);
        }

        protected virtual void OnSystemMessageEvent(SystemMessagePacketEventArgs e)
        {
            lock (this._offlineTells)
            {
                _offlineTells.Add(e.ClientID);
            }
            if (this.SystemMessageEvent != null)
                this.SystemMessageEvent(this, e);
        }

        protected virtual void OnAmdMuxInfoEvent(AmdMuxInfoEventArgs e)
        {
            if (this.AmdMuxInfoEvent != null)
                this.AmdMuxInfoEvent(this, e);
        }

        protected virtual void OnForwardEvent(ForwardEventArgs e)
        {
            if (this.ForwardEvent != null)
                this.ForwardEvent(this, e);
        }

        protected virtual void OnChannelMessageEvent(ChannelMessageEventArgs e)
        {
            this.Debug(this.GetUserName(e.CharacterID) + ": " + e.Message, "[" + e.Channel + "]");

            if (this.ChannelMessageEvent != null)
                this.ChannelMessageEvent(this, e);
        }

        protected virtual void OnPrivateChannelMessageEvent(PrivateChannelMessageEventArgs e)
        {
            this.Debug(this.GetUserName(e.CharacterID) + ": " + e.Message, "[" + e.Channel + "]");

            if (this.PrivateChannelMessageEvent != null)
                this.PrivateChannelMessageEvent(this, e);
        }

        protected virtual void OnTellEvent(TellEventArgs e)
        {
            if (this.IgnoreAfkTells)
            {
                string afk = this.GetUserName(e.CharacterID) + " is AFK";
                if (e.Message.Length > afk.Length)
                {
                    if (e.Message.Substring(0, afk.Length) == afk)
                    {
                        return;
                    }
                }
            }
            if (this.IgnoreOfflineTells)
            {
                lock (this._offlineTells)
                {
                    if (this._offlineTells.Contains(e.CharacterID))
                    {
                        this.Debug(e.Message, "[Offline][" + e.Character + "]:");
                        this._offlineTells.Remove(e.CharacterID);
                        return;
                    }
                }
            }
            if (e.Outgoing)
                this.Debug(e.Message, "To [" + e.Character + "]:");
            else
                this.Debug(e.Message, "[" + e.Character + "]:");

            if (this.TellEvent != null)
                this.TellEvent(this, e);
        }

        protected virtual void OnPrivateChannelStatusEvent(PrivateChannelStatusEventArgs e)
        {
            if (this.PrivateChannelStatusEvent != null)
                this.PrivateChannelStatusEvent(this, e);
        }

        protected virtual void OnChannelJoinEvent(ChannelJoinEventArgs e)
        {
            if (this.State != ChatState.Connected)
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Connected));

            lock (_channels)
            {
                this._channels[e.ID] = e.GetChannel();
            }
            if (e.Type == ChannelType.Unknown)
                this.Debug("Unknown channel type: " + e.TypeID, "[Error]");
            this.Debug("Joined channel: " + e.Name + " (ID:" + e.ID + " Type:" + e.Type.ToString() + " Muted:" + e.Mute.ToString() + ")", "[Bot]");
            if (e.Type == ChannelType.Guild)
            {
                this._guild = e.Name;
                this._guildid = e.ID;
                this.Debug("Registered guild: " + e.Name + " (ID:" + e.ID + ")", "[Bot]");
            }

            if (this.ChannelJoinEvent != null)
                this.ChannelJoinEvent(this, e);
        }

        protected virtual void OnFriendStatusEvent(FriendStatusEventArgs e)
        {
            this.Debug("Friend status received: " + e.Character + " (ID:" + e.CharacterID + " Online:" + e.Online.ToString() + " ID2:" + e.ID2 + " Level:" + e.Level + " Class:" + e.Class.ToString() + ")", "[Database]");

            e.First = true; //FIXME
            if (this.FriendStatusEvent != null)
                this.FriendStatusEvent(this, e);
        }

        protected virtual void OnFriendRemovedEvent(CharacterIDEventArgs e)
        {
            this.Debug("Friend removed: " + e.Character, "[Database]");
            if (this.FriendRemovedEvent != null)
                this.FriendRemovedEvent(this, e);
        }

        protected virtual void OnAnonVicinityEvent(AnonVicinityEventArgs e)
        {
            if (this.AnonVicinityEvent != null)
                this.AnonVicinityEvent(this, e);
        }

        protected virtual void OnNameLookupEvent(NameLookupEventArgs e)
        {
            lock (this._users)
            {
                this._users[e.CharacterID] = Format.UppercaseFirst(e.Name);
            }
            if (e.CharacterID > 0)
            {
                this.Debug("Name lookup received: " + e.Name + " (ID:" + e.CharacterID + ")", "[Database]");
            }
            else
            {
                this.Debug("User doesn't exist: " + e.Name, "[Database]");
            }
            this._lookupReset.Set();
            this._lookupReset.Reset();

            if (this.NameLookupEvent != null)
                this.NameLookupEvent(this, e);
        }

        protected virtual void OnLoginOKEvent()
        {
            this.Debug("Logged in succesfully", "[Auth]");
            if (this.LoginOKEvent != null)
                this.LoginOKEvent(this, new EventArgs());
        }

        protected virtual void OnPrivateChannelRequestEvent(PrivateChannelRequestEventArgs e)
        {
            if (this.PrivateChannelRequestEvent != null)
                this.PrivateChannelRequestEvent(this, e);
        }

        protected virtual void OnCharacterIDEvent(CharacterIDEventArgs e)
        {
            if (this.CharacterIDEvent != null)
                this.CharacterIDEvent(this, e);
        }

        protected virtual void OnLoginCharacterListEvent(LoginChararacterListEventArgs e)
        {
            if (this.LoginCharlistEvent != null)
                this.LoginCharlistEvent(this, e);

            if (e.Override)
                this._character = Format.UppercaseFirst(e.Character);

            if (string.IsNullOrEmpty(this._character) || !e.CharacterList.ContainsKey(this._character))
            {
                String clist = String.Empty;
                foreach (String chars in e.CharacterList.Keys)
                {
                    clist += chars + " ";
                }
                this.Debug(String.Format("The character name, {0}, was not found in {1}.", this._character, clist.Trim()), "[Auth]");
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Error));
            }
            else
            {
                LoginChar character = (LoginChar)e.CharacterList[this._character];
                if (character.IsOnline && !this.IgnoreCharacterLoggedIn)
                {
                    this.Debug("Character " + this._character + " is already online!", "[Auth]");
                    this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
                    return;
                }
                lock (this._users)
                    this._users.Add(character.ID, Format.UppercaseFirst(character.Name));
                this._id = character.ID;
                SimpleIdPacket packet = new SimpleIdPacket(Packet.Type.LOGIN_SELCHAR, character.ID);
                packet.Priority = PacketQueue.Priority.Urgent;
                this.SendPacket(packet);
                this.Debug("Selecting character: " + this._character, "[Auth]");
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.CharacterSelect));
            }
        }

        protected virtual void OnSimpleMessageEvent(SimpleStringPacketEventArgs e)
        {
            this.Debug(e.Message, "[System]");

            if (this.SimpleMessageEvent != null)
                this.SimpleMessageEvent(this, e);
        }

        protected virtual void OnLoginSeedEvent(LoginMessageEventArgs e)
        {
            this.Debug("Logging in with account: " + this._account, "[Auth]");
            this.SendPacket(new LoginMessagePacket(this._account, this._password, e.Seed));
            this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Login));
            if (this.LoginSeedEvent != null)
                this.LoginSeedEvent(this, e);
        }

        protected virtual void OnStatusChangeEvent(StatusChangeEventArgs e)
        {
            if (this._state == ChatState.Reconnecting && e.State == ChatState.Disconnected)
            {
                return;
            }
            if (this._state != e.State)
            {
                if (e.State == ChatState.Disconnected)
                {
                    if (this._pingTimer != null)
                    {
                        this._pingTimer.Stop();
                    }
                }
                if (e.State == ChatState.Disconnected && this._closing == false && this.AutoReconnect == true)
                {
                    this._state = ChatState.Reconnecting;
                    e = new StatusChangeEventArgs(this._state);
                    if (this._socket != null)
                    {
                        if (this._socket.Connected) { this._socket.Close(); }
                    }
                    this._reconnectTimer.Interval = this.ReconnectDelay;
                    this._reconnectTimer.Start();
                }
                this._state = e.State;
                this.Debug("State changed to: " + e.State.ToString(), "[Bot]");
                if (this.StatusChangeEvent != null)
                    this.StatusChangeEvent(this, e);
            }
        }

        protected virtual void OnReconnectEvent(object sender, ElapsedEventArgs e)
        {
            this._reconnectTimer.Stop();
            this.Connect();
        }

        protected virtual void OnPongEvent()
        {
            this.Debug("Pong!", "[Bot]");
            this._lastPong = DateTime.Now;
        }

        protected virtual void OnPingTimerEvent(object sender, ElapsedEventArgs e)
        {
            if (this._socket == null || this._socket.Connected == false)
            {
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
            }
            TimeSpan ts = DateTime.Now.Subtract(this._lastPong);
            if (ts.TotalMilliseconds > (this.PingInterval * 1.5))
            {
                this.Debug("Connection timed out", "[Bot]");
                this.OnStatusChangeEvent(new StatusChangeEventArgs(ChatState.Disconnected));
                return;
            }
            this.Debug("Ping?", "[Bot]");
            this.SendPing();
        }
        #endregion

        #region Get Commands
        public virtual UInt32 GetUserID(string user)
        {
            bool Lookup = false;
            user = Format.UppercaseFirst(user);
            for (int i = 0; i < 300; i++)
            {
                lock (this._users)
                {
                    if (this._users.ContainsValue(user))
                    {
                        foreach (KeyValuePair<UInt32, String> kvp in this._users)
                        {
                            if (kvp.Value == user)
                            {
                                if (kvp.Key > 0)
                                {
                                    return kvp.Key;
                                }
                                else
                                {
                                    this._users.Remove(kvp.Key);
                                    return 0;
                                }
                            }
                        }
                    }
                    else if (Lookup == false)
                    {
                        this.SendNameLookup(user);
                        Lookup = true;
                    }
                }
                Thread.Sleep(50);
            }
            return 0;
        }

        public virtual string GetUserName(UInt32 userID)
        {
            if (this._users == null)
                return "";
            lock (this._users)
            {
                if (this._users.ContainsKey(userID))
                {
                    return this._users[userID];
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual BigInteger GetChannelID(String channelName)
        {
            lock (this._channels)
            {
                foreach (KeyValuePair<BigInteger, Channel> kvp in this._channels)
                {
                    if (kvp.Value.Name == channelName)
                        return kvp.Key;
                }
            }
            return new BigInteger(0);
        }

        public virtual string GetChannelName(Int32 channelID) { return this.GetChannelName(new BigInteger(channelID)); }
        public virtual string GetChannelName(BigInteger channelID)
        {
            if (this._channels == null)
                return "";

            lock (this._channels)
            {
                if (this._channels.ContainsKey(channelID))
                {
                    return this._channels[channelID].Name;
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual ChannelType GetChannelType(BigInteger channelID)
        {
            lock (this._channels)
            {
                if (this._channels.ContainsKey(channelID))
                {
                    return this._channels[channelID].Type;
                }
                else
                {
                    return ChannelType.Unknown;
                }
            }
        }
        #endregion

        #region Send Commands
        public virtual void SendPacket(Packet packet)
        {
            if (this._socket == null || !this._socket.Connected)
            {
                this.Debug("Not Connected", "[Error]");
                return;
            }
            switch (packet.PacketType)
            {
                case Packet.Type.PRIVATE_MESSAGE:
                case Packet.Type.CHANNEL_MESSAGE:
                    _slowQueue.Enqueue(packet.Priority, packet);
                    break;
                default:
                    _fastQueue.Enqueue(packet.Priority, packet);
                    break;
            }
        }

        public virtual void SendChannelMessage(string channel, string text) { this.SendChannelMessage(this.GetChannelID(channel), text, PacketQueue.Priority.Standard); }
        public virtual void SendChannelMessage(BigInteger channelID, string text) { this.SendChannelMessage(channelID, text, PacketQueue.Priority.Standard); }
        public virtual void SendChannelMessage(BigInteger channelID, string text, PacketQueue.Priority priority)
        {
            ChannelMessagePacket p = new ChannelMessagePacket(channelID, text);
            p.Priority = priority;
            this.SendPacket(p);
        }

        public virtual void SendFriendAdd(string user)
        {
            if (string.IsNullOrEmpty(user)) return;
            this.Debug("Adding user to friendslist: " + user, "[Bot]");

            ChatCommandPacket p = new ChatCommandPacket("addbuddy", user);
            p.Priority = PacketQueue.Priority.Standard;

            this.SendPacket(p);
        }

        public virtual void SendFriendRemove(string user)
        {
            if (string.IsNullOrEmpty(user)) return;
            this.Debug("Removing user from friendslist: " + user, "[Bot]");

            ChatCommandPacket p = new ChatCommandPacket("rembuddy", user);
            p.Priority = PacketQueue.Priority.Standard;

            this.SendPacket(p);
        }

        public virtual void SendPrivateChannelInvite(string user) { this.SendPrivateChannelInvite(this.GetUserID(user)); }
        public virtual void SendPrivateChannelInvite(UInt32 userID)
        {
            if (userID == this._id)
                return;
            SimpleIdPacket p = new SimpleIdPacket(Packet.Type.PRIVATE_CHANNEL_INVITE, userID);
            p.Priority = PacketQueue.Priority.Urgent;
            this.SendPacket(p);
        }

        public virtual void SendPrivateChannelKick(string user) { this.SendPrivateChannelKick(this.GetUserID(user)); }
        public virtual void SendPrivateChannelKick(UInt32 userID)
        {
            if (userID == this._id)
                return;
            SimpleIdPacket p = new SimpleIdPacket(Packet.Type.PRIVATE_CHANNEL_KICK, userID);
            p.Priority = PacketQueue.Priority.Urgent;
            this.SendPacket(p);
        }

        public virtual void SendPrivateChannelKickAll()
        {
            EmptyPacket p = new EmptyPacket(Packet.Type.PRIVATE_CHANNEL_KICKALL);
            p.Priority = PacketQueue.Priority.Urgent;
            this.SendPacket(p);
        }

        public virtual void SendPrivateChannelMessage(string text) { this.SendPrivateChannelMessage(this._id, text); }
        public virtual void SendPrivateChannelMessage(string channel, string text) { this.SendPrivateChannelMessage(this.GetUserID(channel), text); }
        public virtual void SendPrivateChannelMessage(UInt32 channelID, string text)
        {
            PrivateChannelMessagePacket p = new PrivateChannelMessagePacket(channelID, text);
            p.Priority = PacketQueue.Priority.Urgent;
            this.SendPacket(p);
        }

        public virtual void SendPrivateMessage(string user, string text) { this.SendPrivateMessage(this.GetUserID(user), text, PacketQueue.Priority.Standard); }
        public virtual void SendPrivateMessage(UInt32 userID, string text) { this.SendPrivateMessage(userID, text, PacketQueue.Priority.Standard); }
        public virtual void SendPrivateMessage(UInt32 userID, string text, PacketQueue.Priority priority)
        {
            if (userID == this._id)
                return;
            TellPacket p = new TellPacket(userID, text);
            p.Priority = priority;
            this.SendPacket(p);
        }

        public virtual void SendNameLookup(string name)
        {
            lock (this._users)
                if (this._users.ContainsValue(Format.UppercaseFirst(name)))
                    return;

            NameLookupPacket p = new NameLookupPacket(name);
            p.Priority = PacketQueue.Priority.Urgent;
            this.Debug("Requesting ID: " + name, "[Database]");
            this.SendPacket(p);
        }

        public virtual void SendPing()
        {
            EmptyPacket p = new EmptyPacket(Packet.Type.PING);
            p.Priority = PacketQueue.Priority.Urgent;
            this.SendPacket(p);
        }
        #endregion

        public override string ToString()
        {
            return this.Character + "@" + this._serverAddress + ":" + this._port;
        }

        protected void Debug(string msg, string cat)
        {
            Trace.WriteLine("[" + this.ToString() + "] " + cat + " " + msg);
        }
    } // end of Chat

    public class StatusChangeEventArgs : EventArgs
    {
        private ChatState _state;
        public StatusChangeEventArgs(ChatState state)
        {
            this._state = state;
        }
        public ChatState State
        {
            get { return this._state; }
        }
    }

    public class ParsePacketData
    {
        public Packet.Type type;
        public short lenght = 0;
        public byte[] data;

        public ParsePacketData(Packet.Type t, short l, byte[] d)
        {
            this.type = t;
            this.lenght = l;
            if (d != null)
            {
                this.data = new byte[d.Length];
                d.CopyTo(this.data, 0);
            }
        }
    }
}