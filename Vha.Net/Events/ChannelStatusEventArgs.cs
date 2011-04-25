/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
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
        /// <summary>
        /// constructor for channel mute events
        /// </summary>
        /// <param name="id">5-byte channel id</param>
        /// <param name="name">channel name</param>
        /// <param name="mute">whether the channel is muted or not</param>
        /// <param name="channelType">channel type</param>
        public ChannelStatusEventArgs(BigInteger id, String name, ChannelFlags flags, byte channelType, string tag)
        {
            this.ID = id;
            this.Name = name;
            this.Flags = flags;
            this.TypeID = channelType;
            if (Enum.IsDefined(typeof(ChannelType), (int)channelType))
                this.Type = (ChannelType)channelType;
            this.Tag = tag;
        }

        /// <summary>
        /// The 5-byte channel id
        /// </summary>
        public BigInteger ID { get; private set; }

        /// <summary>
        /// The channel name
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Channel flags
        /// </summary>
        public ChannelFlags Flags { get; private set; }

        /// <summary>
        /// Channel type ID
        /// </summary>
        public byte TypeID { get; private set; }

        /// <summary>
        /// Channel type
        /// </summary>
        public ChannelType Type { get; private set; }

        /// <summary>
        /// A string value associated with this channel
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Returns combined channel data
        /// </summary>
        public Channel GetChannel() { return new Channel(this.ID, this.Name, this.Type, this.Flags, this.Tag); }
    }
}
