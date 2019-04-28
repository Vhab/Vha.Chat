/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
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
using System.Collections;
using System.Net;

namespace Vha.Net
{
    public class NetString
    {
        public static Encoding Encoding = Encoding.GetEncoding("utf-8");

        private String _value;
        private short _length;

        public NetString(byte[] data) : this(data, 0) { }
        public NetString(byte[] data, int offset)
        {
            if (data == null || data.Length - offset < 3) { return; }
            this._length = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
            this._value = NetString.Encoding.GetString(data, 2 + offset, this._length);
        }
        public NetString(byte[] data, int offset, short length)
        {
            if (data == null || data.Length - offset - length < 0) { return; }
            this._length = length;
            this._value = NetString.Encoding.GetString(data, offset, this._length);
        }
        public NetString(string value)
        {
            this._value = value;
            if (value == null) this._length = 0;
            else this._length = (short)value.Length;
        }

        public byte[] GetBytes()
        {
            if (this._value == null)
                return null;
            else
            {
                byte[] value = NetString.Encoding.GetBytes(this._value);
                byte[] bytes = new byte[value.Length + 2];
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value.Length)).CopyTo(bytes, 0);
                value.CopyTo(bytes, 2);
                return bytes;
            }
        }

        public string Value { get { return (this._value == null ? string.Empty : this._value); } }
        public short Length { get { return this._length; } }
        public int TotalLength { get { return this.Length + 2; } }
        override public string ToString() { return this.Value; }
    }
}
