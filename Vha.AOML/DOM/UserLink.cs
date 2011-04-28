/*
* Vha.AOML
* Copyright (C) 2010 Remco van Oosterhout
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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A link to an Anarchy Online character.
    /// Commonly known as user://
    /// </summary>
    public class UserLink : Link
    {
        /// <summary>
        /// Returns the character the link refers at
        /// </summary>
        public readonly string Character;

        /// <summary>
        /// Initializes a new instance of UserLink
        /// </summary>
        /// <param name="character">The character</param>
        public UserLink(string character)
            : base(LinkType.User)
        {
            this.Character = character;
        }

        /// <summary>
        /// Creates a clone of this UserLink
        /// </summary>
        /// <returns>A new UserLink</returns>
        public override Link Clone()
        {
            return new UserLink(this.Character);
        }
    }
}
