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
    /// Holds event args for channel status messages.
    /// </summary>
    public class ChannelStatusEventArgs : EventArgs
    {
        private readonly BigInteger _id = 0;
        private readonly String _name = null;
        private readonly ChannelFlags _flags = 0;
        private readonly byte _typeID = 0;
        private readonly ChannelType _type = ChannelType.Unknown;
        private readonly string _tag = "";

        /// <summary>
        /// constructor for channel mute events
        /// </summary>
        /// <param name="id">5-byte channel id</param>
        /// <param name="name">channel name</param>
        /// <param name="mute">whether the channel is muted or not</param>
        /// <param name="channelType">channel type</param>
        public ChannelStatusEventArgs(BigInteger id, String name, ChannelFlags flags, byte channelType, string tag)
        {
            this._id = id;
            this._name = name;
            this._flags = flags;
            this._typeID = channelType;
            if (Enum.IsDefined(typeof(ChannelType), (int)channelType))
                this._type = (ChannelType)channelType;
            this._tag = tag;
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
        public ChannelFlags Flags { get { return this._flags; } }

        /// <summary>
        /// Channel type ID
        /// </summary>
        public byte TypeID { get { return this._typeID; } }

        /// <summary>
        /// Channel type
        /// </summary>
        public ChannelType Type { get { return this._type; } }

        /// <summary>
        /// A string value associated with this channel
        /// </summary>
        public string Tag { get { return this._tag; } }

        /// <summary>
        /// Returns combined channel data
        /// </summary>
        public Channel GetChannel() { return new Channel(this._id, this._name, this._type, this._flags, this._tag); }
    }
}
