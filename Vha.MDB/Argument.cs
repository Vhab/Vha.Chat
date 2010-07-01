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
