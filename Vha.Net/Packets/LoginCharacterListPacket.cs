/*
* Vha.Net
* Copyright (C) 2005-2009 Remco van Oosterhout
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
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace Vha.Net.Packets
{
    internal class LoginCharacterListPacket : Packet
    {
        protected short _charactersCount;
        protected LoginCharacter[] _characters;
        internal LoginCharacterListPacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 3) { return; }

            short num = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 0));
            LoginCharacter[] cs = new LoginCharacter[num];

            int i = 0;
            int offset = 2;

            for (; i < num; i++)
            {
                cs[i] = new LoginCharacter();
                cs[i].ID = NetConvert.ToUInt32(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset)));
                offset += 4;
            }
            num = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
            offset += 2;
            for (i = 0; i < num; i++)
            {
                NetString s = new NetString(data, offset);
                cs[i].Name = s.ToString();
                offset += s.TotalLength;
            }
            num = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
            offset += 2;
            for (i = 0; i < num; i++)
            {
                cs[i].Level = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset));
                offset += 4;
            }
            num = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
            offset += 2;
            for (i = 0; i < num; i++)
            {
                cs[i].IsOnline = Convert.ToBoolean(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset)));
                offset += 4;
            }

            this._characters = cs;
            this._charactersCount = num;
        }
        internal LoginCharacter[] Characters
        {
            get { return this._characters; }
        }
    }
}
