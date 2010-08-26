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
    public class Friend : IComparable<Friend>
    {
        private readonly UInt32 _id;
        private readonly String _name;
        private readonly bool _online;
        private readonly string _tag;
        private readonly bool _temporary;

        public Friend(UInt32 id, String name, bool online, string tag, bool temporary)
        {
            this._id = id;
            this._name = name;
            this._online = online;
            this._tag = tag;
            this._temporary = temporary;
        }

        public UInt32 ID { get { return this._id; } }
        public String Name { get { return this._name; } }
        public bool Online { get { return this._online; } }
        public string Tag { get { return this._tag; } }
        public bool Temporary { get { return this._temporary; } }
        public override string ToString() { return this._name; }

        public int CompareTo(Friend friend)
        {
            if (friend == null)
                return 1;
            if (this.ID != friend.ID)
                return this.ID.CompareTo(friend.ID);
            if (this.Name != friend.Name)
                return this.Name.CompareTo(friend.Name);
            if (this.Online != friend.Online)
                return this.Online.CompareTo(friend.Online);
            if (string.Compare(this.Tag, friend.Tag, StringComparison.Ordinal) != 0)
                return string.Compare(this.Tag, friend.Tag, StringComparison.Ordinal);
            if (this.Temporary != friend.Temporary)
                return this.Tag.CompareTo(friend.Temporary);
            return 0;
        }

        public bool Equals(Friend right) { return this.CompareTo(right) == 0; }
    }
}
