/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
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

namespace Vha.Net.Events
{
    /// <summary>
    /// Holds event args for private channel requests
    /// </summary>
    public class PrivateChannelRequestEventArgs : EventArgs
    {
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private bool _join = false;

        /// <summary>
        /// Constructor for private channel requests
        /// </summary>
        /// <param name="characterID">the character id</param>
        /// <param name="join">whether asked to join or leave</param>
        public PrivateChannelRequestEventArgs(UInt32 characterID, string character, bool join)
        {
            this._characterID = characterID;
            this._character = character;
            this._join = join;
        }

        /// <summary>
        /// The character id
        /// </summary>
        public UInt32 CharacterID { get { return this._characterID; } }
        /// <summary>
        /// The character inviting us
        /// </summary>
        public String Character { get { return this._character; } }
        /// <summary>
        /// Whether we accept or decline the join
        /// </summary>
        public bool Join
        {
            get { return this._join; }
            set { this._join = value; }
        }
    }
}
