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
using System.Collections.Generic;
using System.Text;
using VhaBot.Common;

namespace VhaBot.Net.Events
{
    /// <summary>
    /// Holds event args for friend status messages.
    /// </summary>
    public class FriendStatusEventArgs : EventArgs
    {
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly bool _online = false;
        private readonly CharacterState _state = CharacterState.Unknown;
        private readonly byte _level = 0;
        private readonly UInt32 _id2 = 0;
        private readonly CharacterClass _class = CharacterClass.Unknown;
        private readonly byte _classID;

        /// <summary>
        /// Event argument constructor
        /// </summary>
        /// <param name="characterID">The character id of the character</param>
        /// <param name="status">The status of the character</param>
        /// <param name="level">The level of the character</param>
        /// <param name="id2">Either the last seen timestamp or the playfied id</param>
        /// <param name="characterClass">The class id of the character</param>
        public FriendStatusEventArgs(UInt32 characterID, string character, byte state, byte level, UInt32 id2, byte characterClass)
        {
            this._characterID = characterID;
            this._character = character;
            if (Enum.IsDefined(typeof(CharacterState), (int)state))
                this._state = (CharacterState)state;
            this._online = (this._state != CharacterState.Offline);
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
        /// Whether the character is online or not
        /// </summary>
        public bool Online { get { return this._online; } }
        /// <summary>
        /// The current state of the character
        /// </summary>
        public CharacterState State { get { return this._state; } }
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
    }
}
