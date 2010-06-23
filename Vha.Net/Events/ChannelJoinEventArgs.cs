/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
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
using Vha.Common;

namespace Vha.Net.Events
{
    /// <summary>
    /// Holds event args for channel join messages.
    /// </summary>
    public class ChannelJoinEventArgs : EventArgs
    {
        private readonly BigInteger _id = 0;
        private readonly String _name = null;
        private readonly UInt16 _flags = 0;
        private readonly bool _mute = false;
        private readonly byte _typeID = 0;
        private ChannelType _type = ChannelType.Unknown;

        /// <summary>
        /// constructor for channel mute events
        /// </summary>
        /// <param name="id">5-byte channel id</param>
        /// <param name="name">channel name</param>
        /// <param name="mute">whether the channel is muted or not</param>
        /// <param name="channelType">channel type</param>
        public ChannelJoinEventArgs(BigInteger id, String name, UInt16 flags, bool mute, byte channelType)
        {
            this._id = id;
            this._name = name;
            this._flags = flags;
            this._mute = mute;
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
        /// Channel flags
        /// </summary>
        public UInt16 Flags { get { return this._flags; } }

        /// <summary>
        /// Whether the channel is muted or not
        /// </summary>
        public bool Muted { get { return this._mute; } }

        /// <summary>
        /// Channel type ID
        /// </summary>
        public byte TypeID { get { return this._typeID; } }

        /// <summary>
        /// Channel type
        /// </summary>
        public ChannelType Type { get { return this._type; } }

        /// <summary>
        /// Returns combined channel data
        /// </summary>
        public Channel GetChannel() { return new Channel(this._id, this._name, this._type, this.Muted); }
    }
}
