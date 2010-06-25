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
    public class PrivateChannel : IComparable<PrivateChannel>
    {
        private readonly UInt32 _id = 0;
        private readonly String _name = null;
        private readonly bool _local = false;

        public PrivateChannel(UInt32 id, String name, bool local)
        {
            this._id = id;
            this._name = name;
            this._local = local;
        }

        public UInt32 ID { get { return this._id; } }
        public String Name { get { return this._name; } }
        public bool Local { get { return this._local; } }
        public override string ToString() { return this._name; }

        public int CompareTo(PrivateChannel channel)
        {
            if (channel == null)
                return 1;
            if (this.ID != channel.ID)
                return this.ID.CompareTo(channel.ID);
            if (this.Name != channel.Name)
                return this.Name.CompareTo(channel.Name);
            if (this.Local != channel.Local)
                return this.Local.CompareTo(channel.Local);
            return 0;
        }

        public bool Equals(PrivateChannel right) { return this.CompareTo(right) == 0; }
    }
}
