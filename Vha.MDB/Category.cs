/*
* Vha.MDB
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
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
