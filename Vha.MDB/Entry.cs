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
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Vha.MDB
{
    public class Entry
    {
        private Int32 _entryID;
        private Int32 _offset;
        private string _message;

        [XmlAttribute("ID")]
        public Int32 EntryID { get { return this._entryID; } set { this._entryID = value; } }
        [XmlIgnore()]
        public Int32 Offset { get { return this._offset; } set { this._offset = value; } }
        [XmlAttribute("Message")]
        public string Message { get { return this._message; } set { this._message = value; } }

        public Entry() { }
        public Entry(Int32 entryID, Int32 offset, string message)
        {
            this._entryID = entryID;
            this._offset = offset;
            this._message = message;
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", this._entryID, this._message);
        }
    }
}