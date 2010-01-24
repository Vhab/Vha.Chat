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

namespace Vha.MDB
{
    public class Message
    {
        private Int32 _categoryID = 0;
        private Int32 _entryID = 0;
        private List<Argument> _arguments = new List<Argument>();
        private String _value = null;
        private String _raw = null;

        public Int32 CategoryID { get { return this._categoryID; } }
        public Int32 EntryID { get { return this._entryID; } }
        public Argument[] Arguments { get { return this._arguments.ToArray(); } }
        public String Value
        {
            get { return this._value; }
            set
            {
                if (this._value == null)
                    this._value = value;
                else
                    throw new Exception("AoDescrambledMessage.Message can only be set once!");
            }
        }
        public String Raw { get { return this._raw; } }

        public Message(Int32 categoryID, Int32 entryID, string raw)
        {
            this._categoryID = categoryID;
            this._entryID = entryID;
            this._raw = raw;
        }

        public void Append(Argument argument)
        {
            if (this._categoryID == 0 && this._entryID == 0)
                throw new Exception("Trying to append to an invalid AoDescrambledMessage object! CategoryID and EntryID are both 0!");

            this._arguments.Add(argument);
        }
    }
}
