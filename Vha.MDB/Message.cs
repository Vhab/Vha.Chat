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
