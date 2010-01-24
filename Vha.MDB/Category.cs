/*
* Vha.MDB
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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Vha.MDB
{

    public class Category
    {
        private Int32 _categoryID;
        private Int32 _offset;
        private List<Entry> _entries;

        [XmlAttribute("ID")]
        public Int32 CategoryID { get { return this._categoryID; } set { this._categoryID = value; } }
        [XmlIgnore()]
        public Int32 Offset { get { return this._offset; } set { this._offset = value; } }
        [XmlElement("Entry")]
        public Entry[] Entries
        {
            get { return this._entries.ToArray(); }
            set
            {
                this._entries = new List<Entry>();
                foreach (Entry entry in value)
                    this._entries.Add(entry);
            }
        }

        public Category() { }
        public Category(Int32 categoryID, Int32 offset)
        {
            this._categoryID = categoryID;
            this._offset = offset;
            this._entries = new List<Entry>();
        }

        public bool Add(Entry entry)
        {
            if (entry == null)
                return false;

            if (this.GetEntry(entry.EntryID) != null)
                return false;

            if (this._entries != null)
            {
                lock (this._entries)
                {
                    this._entries.Add(entry);
                    return true;
                }
            }

            return false;

        }

        public Entry GetEntry(Int32 entryID)
        {
            if (this._entries != null)
                lock (this._entries)
                    foreach (Entry entry in this._entries)
                        if (entry.EntryID == entryID)
                            return entry;

            return null;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1} Entries)", this._categoryID, this._entries.Count);
        }
    }
}
