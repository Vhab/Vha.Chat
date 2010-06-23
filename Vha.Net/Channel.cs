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

namespace Vha.Net
{
    public class Channel
    {
        private readonly BigInteger _id = 0;
        private readonly String _name = null;
        private readonly ChannelType _type = ChannelType.Unknown;
        private readonly bool _muted = false;

        public Channel(BigInteger id, String name, ChannelType type, bool muted)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._muted = muted;
        }

        public BigInteger ID { get { return this._id; } }
        public String Name { get { return this._name; } }
        public ChannelType Type { get { return this._type; } }
        public bool Muted { get { return this._muted; } }
        public override string ToString() { return this._name; }

        public bool Equals(Channel channel)
        {
            if (this.ID != channel.ID) return false;
            if (this.Name != channel.Name) return false;
            if (this.Type != channel.Type) return false;
            if (this.Muted != channel.Muted) return false;
            return true;
        }
    }
}
