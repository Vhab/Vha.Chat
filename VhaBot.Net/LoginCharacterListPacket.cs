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
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace VhaBot.Net
{
    internal class LoginCharacterListPacket : Packet
    {
        protected short _charactersCount;
        protected Dictionary<string, LoginChar> _characters;
        internal LoginCharacterListPacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 3) { return; }

            short num = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 0));
            LoginChar[] cs = new LoginChar[num];

            int i = 0;
            int offset = 2;

            for (; i < num; i++)
            {
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

            this._characters = new Dictionary<string, LoginChar>();
            this._charactersCount = num;

            foreach (LoginChar c in cs)
            {
                this._characters.Add(c.Name, c);
                this.AddData(c);
            }
        }
        internal Dictionary<string, LoginChar> Characters
        {
            get { return this._characters; }
        }
    }

    /// <summary>
    /// Holds event args for login character list messages.
    /// </summary>
    public class LoginChararacterListEventArgs : EventArgs
    {
        private readonly Dictionary<string, LoginChar> _characters = null;
        public LoginChararacterListEventArgs(Dictionary<string, LoginChar> CharacterList)
        {
            this._characters = CharacterList;
        }
        public Dictionary<string, LoginChar> CharacterList { get { return this._characters; } }
        public bool Override = false;
        public string Character = String.Empty;
    }
}
