/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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

namespace Vha.Chat.Events
{
    public class IgnoreEventArgs
    {
        /// <summary>
        /// The name of the character.
        /// Note this name may not match the original entry if the name was changed on the server.
        /// </summary>
        public readonly string Character;
        /// <summary>
        /// The id of the character
        /// </summary>
        public readonly UInt32 CharacterID;
        /// <summary>
        /// Whether this character was added to the ignore list
        /// </summary>
        public readonly bool Added;

        public IgnoreEventArgs(string character, UInt32 characterID, bool added)
        {
            this.Character = character;
            this.CharacterID = characterID;
            this.Added = added;
        }
    }
}
