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