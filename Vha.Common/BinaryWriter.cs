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
using System.IO;

namespace Vha.Common
{
    public class BinaryWriter
    {
        public Encoding Encoding { get { return this._encoding; } }

        public long Length { get { return this._data.Length; } }

        public Endianness Endianness { get { return this._endianness; } }

        public BinaryWriter()
            : this(Encoding.UTF8) { }
        public BinaryWriter(Encoding encoding)
            : this(encoding, BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian) { }
        public BinaryWriter(Endianness endianness)
            : this(Encoding.UTF8, endianness) { }
        public BinaryWriter(Encoding encoding, Endianness endianness)
        {
            this._encoding = encoding;
            this._data = new MemoryStream();
            this._endianness = endianness;
        }

        #region Internal
        private Encoding _encoding;
        private MemoryStream _data;
        private Endianness _endianness;
        #endregion

        #region Methods
        public byte[] GetBytes()
        {
            return this._data.ToArray();
        }

        public void WriteByte(byte value)
        {
            this._data.WriteByte(value);
        }

        public void WriteBytes(byte[] bytes)
        {
            this._data.Write(bytes, 0, bytes.Length);
        }

        public void WriteBytes(byte[] bytes, int offset, int length)
        {
            this._data.Write(bytes, offset, length);
        }

        public void WriteData(byte[] bytes)
        {
            if (bytes.Length > UInt16.MaxValue)
                throw new ArgumentException("Data length exceeds" + UInt16.MaxValue);
            this.WriteUInt16((UInt16)bytes.Length);
            this.WriteBytes(bytes);
        }

        public void WriteChar(char value)
        {
            this.WriteByte((byte)value);
        }

        public void WriteBool(bool value)
        {
            this.WriteByte(value ? (byte)1 : (byte)0);
        }

        public void WriteInt16(Int16 value)
        {
            this.WriteBytes(Binary.WriteInt16(value, this._endianness));
        }

        public void WriteUInt16(UInt16 value)
        {
            this.WriteBytes(Binary.WriteUInt16(value, this._endianness));
        }

        public void WriteInt32(Int32 value)
        {
            this.WriteBytes(Binary.WriteInt32(value, this._endianness));
        }

        public void WriteUInt32(UInt32 value)
        {
            this.WriteBytes(Binary.WriteUInt32(value, this._endianness));
        }

        public void WriteUInt40(UInt64 value)
        {
            this.WriteBytes(Binary.WriteUInt40(value, this._endianness));
        }

        public void WriteInt64(Int64 value)
        {
            this.WriteBytes(Binary.WriteInt64(value, this._endianness));
        }

        public void WriteUInt64(UInt64 value)
        {
            this.WriteBytes(Binary.WriteUInt64(value, this._endianness));
        }
        #endregion
    }
}
