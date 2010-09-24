/*
* Vha.Common
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
using System.Net;

namespace Vha.Common
{
    public class BinaryReader
    {
        public Encoding Encoding { get { return this._encoding; } }

        public int Length { get { return this._data.Length; } }

        public int Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        public Endianness Endianness { get { return this._endianness; } }

        public BinaryReader(byte[] data)
            : this(Encoding.UTF8, data) {}
        public BinaryReader(Encoding encoding, byte[] data)
            : this(encoding, data, BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian) { }
        public BinaryReader(byte[] data, Endianness endianness)
            : this(Encoding.UTF8, data, endianness) { }
        public BinaryReader(Encoding encoding, byte[] data, Endianness endianness)
        {
            this._encoding = encoding;
            this._data = data;
            this._endianness = endianness;
            this._offset = 0;
        }

        #region Internal
        private Encoding _encoding;
        private byte[] _data;
        private int _offset;
        private Endianness _endianness;
        #endregion

        #region Methods
        public byte ReadByte()
        {
            return Binary.ReadByte(ref this._data, ref this._offset);
        }

        public byte[] ReadBytes(int length)
        {
            if (length < 0) throw new ArgumentException("length < 0");
            if (this.Offset + length > this.Length) throw new IndexOutOfRangeException();
            byte[] bytes = new byte[length];
            Array.Copy(this._data, this._offset, bytes, 0, length);
            return bytes;
        }

        public byte[] ReadData()
        {
            UInt16 length = this.ReadUInt16();
            return this.ReadBytes(length);
        }

        public char ReadChar()
        {
            return (char)Binary.ReadByte(ref this._data, ref this._offset);
        }

        public bool ReadBool()
        {
            return Binary.ReadByte(ref this._data, ref this._offset) != 0;
        }

        public Int16 ReadInt16()
        {
            return Binary.ReadInt16(ref this._data, ref this._offset, this._endianness);
        }

        public UInt16 ReadUInt16()
        {
            return Binary.ReadUInt16(ref this._data, ref this._offset, this._endianness);
        }

        public Int32 ReadInt32()
        {
            return Binary.ReadInt32(ref this._data, ref this._offset, this._endianness);
        }

        public UInt32 ReadUInt32()
        {
            return Binary.ReadUInt32(ref this._data, ref this._offset, this._endianness);
        }

        public UInt64 ReadUInt40()
        {
            return Binary.ReadUInt40(ref this._data, ref this._offset, this._endianness);
        }

        public Int64 ReadInt64()
        {
            return Binary.ReadInt64(ref this._data, ref this._offset, this._endianness);
        }

        public UInt64 ReadUInt64()
        {
            return Binary.ReadUInt64(ref this._data, ref this._offset, this._endianness);
        }
        #endregion
    }
}
