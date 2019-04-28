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

namespace Vha.Net
{
    public class Channel : IComparable<Channel>
    {
        private readonly BigInteger _id = 0;
        private readonly String _name = null;
        private readonly ChannelType _type = ChannelType.Unknown;
        private readonly ChannelFlags _flags;
        private readonly string _tag;

        public Channel(BigInteger id, String name, ChannelType type, ChannelFlags flags, string tag)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._flags = flags;
            this._tag = tag;
        }

        public BigInteger ID { get { return this._id; } }
        public String Name { get { return this._name; } }
        public ChannelType Type { get { return this._type; } }
        public ChannelFlags Flags { get { return this._flags; } }
        public string Tag { get { return this._tag; } }
        public override string ToString() { return this._name; }

        public int CompareTo(Channel channel)
        {
            if (channel == null)
                return 1;
            if (this.ID != channel.ID)
                return this.ID < channel.ID ? -1 : 1;
            if (this.Name != channel.Name)
                return this.Name.CompareTo(channel.Name);
            if (this.Type != channel.Type)
                return this.Type.CompareTo(channel.Type);
            if (this.Flags != channel.Flags)
                return this.Flags.CompareTo(channel.Flags);
            if (string.Compare(this.Tag, channel.Tag, StringComparison.Ordinal) != 0)
                return string.Compare(this.Tag, channel.Tag, StringComparison.Ordinal);
            return 0;
        }

        public bool Equals(Channel right) { return this.CompareTo(right) == 0; }
    }
}
