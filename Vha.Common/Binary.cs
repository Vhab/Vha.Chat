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
    public enum Endianness
    {
        BigEndian,
        LittleEndian
    }

    public static class Binary
    {
        public static byte[] Convert(byte[] data, int offset, int length, Endianness endianness)
        {
            // Check input
            if (data == null) throw new ArgumentNullException("data");
            if (offset < 0) throw new IndexOutOfRangeException("offset");
            if (length < 0) throw new ArgumentException("negative value for length");
            if (offset + length > data.Length) throw new IndexOutOfRangeException("offset + length > data.Length");
            // Convert bytes
            byte[] bytes = new byte[length];
            Array.Copy(data, offset, bytes, 0, length);
            if (endianness == Endianness.BigEndian && BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        public static byte ReadByte(ref byte[] data, ref int offset)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (offset < 0) throw new IndexOutOfRangeException("offset");
            if (offset >= data.Length) throw new IndexOutOfRangeException("offset >= data.Length");
            return data[offset++];
        }

        public static char ReadChar(ref byte[] data, ref int offset)
        {
            return System.Convert.ToChar(ReadByte(ref data, ref offset));
        }

        public static Int16 ReadInt16(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 2, endianness);
            offset += 2;
            return BitConverter.ToInt16(bytes, 0);
        }

        public static UInt16 ReadUInt16(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 2, endianness);
            offset += 2;
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static Int32 ReadInt32(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 4, endianness);
            offset += 4;
            return BitConverter.ToInt32(bytes, 0);
        }

        public static UInt32 ReadUInt32(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 4, endianness);
            offset += 4;
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static UInt64 ReadUInt40(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = new byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                bytes[0] = data[offset++];
                bytes[1] = data[offset++];
                bytes[2] = data[offset++];
                bytes[3] = data[offset++];
                bytes[4] = data[offset++];
                bytes[5] = bytes[6] = bytes[7] = 0;
            }
            else
            {
                bytes[1] = bytes[2] = bytes[3] = 0;
                bytes[4] = data[offset++];
                bytes[5] = data[offset++];
                bytes[6] = data[offset++];
                bytes[7] = data[offset++];
                bytes[8] = data[offset++];
            }
            bytes = Convert(bytes, 0, bytes.Length, endianness);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static Int64 ReadInt64(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 8, endianness);
            offset += 8;
            return BitConverter.ToInt64(bytes, 0);
        }

        public static UInt64 ReadUInt64(ref byte[] data, ref int offset, Endianness endianness)
        {
            byte[] bytes = Convert(data, offset, 8, endianness);
            offset += 8;
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static string ReadString(ref byte[] data, ref int offset, Encoding encoding, Endianness endianness)
        {
            UInt16 length = ReadUInt16(ref data, ref offset, endianness);
            return ReadString(ref data, ref offset, length, encoding);
        }

        public static string ReadString(ref byte[] data, ref int offset, int length, Encoding encoding)
        {
            if (offset + length > data.Length) throw new IndexOutOfRangeException("offset + length > data.Length");
            string value = encoding.GetString(data, offset, length);
            offset += length;
            return value;
        }

        public static byte[] WriteInt16(Int16 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteUInt16(UInt16 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteInt32(Int32 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteUInt32(UInt32 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteUInt40(UInt64 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            int start = BitConverter.IsLittleEndian ? 0 : 3;
            return Convert(bytes, start, 5, endianness);
        }

        public static byte[] WriteInt64(Int64 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteUInt64(UInt64 value, Endianness endianness)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Convert(bytes, 0, bytes.Length, endianness);
        }

        public static byte[] WriteString(string value, Encoding encoding, Endianness endianness)
        {
            int length = encoding.GetByteCount(value);
            if (length > UInt16.MaxValue)
                throw new ArgumentException("string byte length exceeds " + UInt16.MaxValue);
            byte[] bytes = new byte[length + 2];
            WriteUInt16((UInt16)length, endianness).CopyTo(bytes, 0);
            encoding.GetBytes(value, 0, value.Length, bytes, 2);
            return bytes;
        }
    }
}
