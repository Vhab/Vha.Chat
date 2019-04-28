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
using System.Collections.Generic;
using System.Text;
using Vha.Common;

namespace Vha.Net.Events
{
    /// <summary>
    /// Holds event args for character name messages.
    /// </summary>
    public class NameLookupEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for name lookup events
        /// </summary>
        /// <param name="characterID">the character id</param>
        /// <param name="name">the name of the character</param>
        public NameLookupEventArgs(UInt32 characterID, String name)
        {
            this.CharacterID = characterID;
            this.Name = Format.UppercaseFirst(name);
        }

        /// <summary>
        /// The character id
        /// </summary>
        public UInt32 CharacterID { get; private set; }

        /// <summary>
        /// The character name
        /// </summary>
        public String Name { get; private set; }
    }
}
