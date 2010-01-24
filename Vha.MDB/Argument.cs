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

namespace Vha.MDB
{
    public class Argument
    {
        private ArgumentType _type;
        private String _text;
        private Int32 _integer;
        private UInt32 _unsignedInteger;
        private Single _float;
        private Int32 _categoryID;
        private Int32 _entryID;
        private Message _recursive;
        private String _message;

        public ArgumentType Type { get { return this._type; } }
        public String Text { get { return this._text; } }
        public Int32 Integer { get { return this._integer; } }
        public UInt32 UnsignedInteger { get { return this._unsignedInteger; } }
        public Single Float { get { return this._float; } }
        public Int32 CategoryID { get { return this._categoryID; } }
        public Int32 EntryID { get { return this._entryID; } }
        public Message Recursive { get { return this._recursive; } }
        public String Message { get { return this._message; } }

        public Argument(String text)
        {
            this._text = text;
            this._message = text;
            this._type = ArgumentType.Text;
        }

        public Argument(Int32 integer)
        {
            this._integer = integer;
            this._message = integer.ToString();
            this._type = ArgumentType.Integer;
        }

        public Argument(UInt32 unsignedInteger)
        {
            this._unsignedInteger = unsignedInteger;
            this._message = unsignedInteger.ToString();
            this._type = ArgumentType.UnsignedInteger;
        }

        public Argument(Single single)
        {
            this._float = single;
            this._message = single.ToString();
            this._type = ArgumentType.Float;
        }

        public Argument(Int32 categoryID, Int32 entryID, String message)
        {
            this._categoryID = categoryID;
            this._entryID = entryID;
            this._message = message;
            this._type = ArgumentType.Reference;
        }

        public Argument(Message aoDescrambledMessage)
        {
            this._recursive = aoDescrambledMessage;
            this._message = aoDescrambledMessage.Value;
            this._type = ArgumentType.Recursive;
            this._categoryID = aoDescrambledMessage.CategoryID;
            this._entryID = aoDescrambledMessage.EntryID;
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
