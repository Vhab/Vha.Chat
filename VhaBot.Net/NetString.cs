/*
* VhaBot - Barbaric Edition
* Copyright (C) 2005-2008 Remco van Oosterhout
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

namespace VhaBot.Net
{
	public class NetString
	{
		protected String _str;
		protected short _len;
        protected Encoding enc = Encoding.GetEncoding("iso-8859-1");
		public NetString(byte[] data): this(data, 0) {}
		public NetString(byte[] data, int StartIndex)
		{
			if (data == null || data.Length - StartIndex < 3) { return; }

			this._len = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, StartIndex));
			this._str = this.enc.GetString(data, 2 + StartIndex, this._len);
		}
		public NetString(String str)
		{
			
			this._str = str;
			if (str == null)
			{
				this._len = 0;
			}
			else
			{
				this._len = (short)str.Length;
			}
		}
		public String Value
		{
			get { return (this._str == null ? String.Empty : this._str); }
		}
		public short Length
		{
			get { return this._len; }
		}
		public int TotalLength
		{
			get { return this.Length + BitConverter.GetBytes(this._len).Length; }
		}
		override public String ToString()
		{
			StringBuilder ret = new StringBuilder(this.Value);
			return ret.ToString();
		}
		public byte[] GetBytes()
		{
			if (this.Value == null)
				return null;
			else
			{
				StringBuilder ret = new StringBuilder(this.Value);

				this._str = ret.ToString();
				this._len = (short)ret.Length;
				
				byte[] b = new byte[this.TotalLength];
				BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Length)).CopyTo(b, 0);
				enc.GetBytes(this.Value).CopyTo(b, 2);
				return b;
			}
		}
	}
}
