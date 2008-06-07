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
using VhaBot.Common;

namespace VhaBot.Net
{
    internal class FriendStatusPacket : Packet
    {
        internal FriendStatusPacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 10) { return; }

            int offset = 0;
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popBoolean(ref data, ref offset));
            this.AddData(popByte(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popByte(ref data, ref offset));
        }

        internal UInt32 CharacterID { get { return (UInt32)this.Data[0]; } }
        internal bool Status { get { return (Boolean)this.Data[1]; } }
        internal byte Level { get { return (byte)this.Data[2]; } }
        internal UInt32 ID2 { get { return (UInt32)this.Data[3]; } }
        internal byte Class { get { return (byte)this.Data[4]; } }
    }

    /// <summary>
    /// Holds event args for friend status messages.
    /// </summary>
    public class FriendStatusEventArgs : EventArgs
    {
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly bool _status = false;
        private readonly byte _level = 0;
        private readonly UInt32 _id2 = 0;
        private readonly CharacterClass _class = CharacterClass.Unknown;
        private readonly byte _classID;
        private bool _first = false;

        /// <summary>
        /// Event argument constructor
        /// </summary>
        /// <param name="characterID">The character id of the character</param>
        /// <param name="online">Whether the character is online or not</param>
        /// <param name="level">The level of the character</param>
        /// <param name="id2">Either the last seen timestamp or the playfied id</param>
        /// <param name="characterClass">The class id of the character</param>
        public FriendStatusEventArgs(UInt32 characterID, string character, bool online, byte level, UInt32 id2, byte characterClass)
        {
            this._characterID = characterID;
            this._character = character;
            this._status = online;
            this._level = level;
            this._id2 = id2;
            this._classID = characterClass;
            if (Enum.IsDefined(typeof(CharacterClass), (int)characterClass))
                this._class = (CharacterClass)characterClass;
        }

        /// <summary>
        /// Id of the character
        /// </summary>
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        /// <summary>
        /// Whether the character is added or not
        /// </summary>
        public bool Online { get { return this._status; } }
        /// <summary>
        /// Level of the character
        /// </summary>
        public byte Level { get { return this._level; } }
        /// <summary>
        /// Either the last seen timestamp of the playfield id
        /// </summary>
        public UInt32 ID2 { get { return this._id2; } }
        /// <summary>
        /// Class of the character
        /// </summary>
        public CharacterClass Class { get { return this._class; } }
        /// <summary>
        /// Class id of the character
        /// </summary>
        public Int32 ClassID { get { return this._classID; } }
        /// <summary>
        /// Defines if it's the first time this character was seen
        /// </summary>
        public bool First { get { return this._first; } set { _first = value; } }
    }
}
