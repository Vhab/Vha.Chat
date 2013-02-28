/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
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
    /// A generic link to an Anarchy Online entity.
    /// Commonly known as itemid://
    /// </summary>
    public class EntityLink : Link
    {
        /// <summary>
        /// Returns the type id of the entity which should be shown
        /// </summary>
        public UInt32 TypeID { get; private set; }

        /// <summary>
        /// Returns the instance id of the entity which should be shown
        /// </summary>
        public UInt32 InstanceID { get; private set; }

        /// <summary>
        /// Initializes a new instance of ItemLink
        /// </summary>
        /// <param name="typeID">The type id of the entity</param>
        /// <param name="instanceID">The template id of the entity</param>
        public EntityLink(UInt32 typeID, UInt32 instanceID)
            : base(LinkType.Entity)
        {
            this.TypeID = typeID;
            this.InstanceID = instanceID;
        }

        /// <summary>
        /// Creates a clone of this ItemLink
        /// </summary>
        /// <returns>A new ItemLink</returns>
        public override Link Clone()
        {
            return new EntityLink(this.TypeID, this.InstanceID);
        }
    }
}
