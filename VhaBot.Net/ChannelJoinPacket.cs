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
using System.Net;
using VhaBot.Common;

namespace VhaBot.Net
{
    internal class ChannelJoinPacket : Packet
    {
        internal ChannelJoinPacket(Packet.Type type, byte[] data) : base(type, data) { }
        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 10) { return; }

            int offset = 0;
            this.AddData(popChannelID(ref data, ref offset));
            this.AddData(popString(ref data, ref offset).ToString());
            this.AddData(popShort(ref data, ref offset));
            this.AddData(popShort(ref data, ref offset));
        }

        internal BigInteger ID { get { return (BigInteger)this.Data[0]; } }
        internal String Name { get { return (String)this.Data[1]; } }
        internal bool Mute { get { return Convert.ToBoolean((Int16)this.Data[2] & 0x0100); } }
        internal bool Logging { get { return Convert.ToBoolean((Int16)this.Data[2] & 0x0200); } }
        internal byte Type { get { return ((BigInteger)this.Data[0]).getBytes()[0]; } }
    }

    /// <summary>
    /// Holds event args for channel join messages.
    /// </summary>
    public class ChannelJoinEventArgs : EventArgs
    {
        private readonly BigInteger _id = 0;
        private readonly String _name = null;
        private readonly bool _mute = false;
        private readonly bool _logging = false;
        private readonly byte _typeID = 0;
        private ChannelType _type = ChannelType.Unknown;

        /// <summary>
        /// constructor for channel mute events
        /// </summary>
        /// <param name="id">5-byte channel id</param>
        /// <param name="name">channel name</param>
        /// <param name="mute">whether the channel is muted or not</param>
        /// <param name="logging">whether the channel is logging or not</param>
        /// <param name="channelType">channel type</param>
        public ChannelJoinEventArgs(BigInteger id, String name, bool mute, bool logging, byte channelType)
        {
            this._id = id;
            this._name = name;
            this._mute = mute;
            this._logging = logging;
            this._typeID = channelType;
            if (Enum.IsDefined(typeof(ChannelType), (int)channelType))
                this._type = (ChannelType)channelType;
        }

        /// <summary>
        /// The 5-byte channel id
        /// </summary>
        public BigInteger ID { get { return this._id; } }

        /// <summary>
        /// The channel name
        /// </summary>
        public String Name { get { return this._name; } }

        /// <summary>
        /// Whether the channel is muted or not
        /// </summary>
        public bool Mute { get { return this._mute; } }

        /// <summary>
        /// Whether the channel is logging or not
        /// </summary>
        public bool Logging { get { return this._logging; } }

        /// <summary>
        /// Channel type ID
        /// </summary>
        public byte TypeID { get { return this._typeID; } }

        /// <summary>
        /// Channel type
        /// </summary>
        public ChannelType Type { get { return this._type; } }
    }
}