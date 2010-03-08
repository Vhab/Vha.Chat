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
using System.Collections.Generic;
using System.Text;

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A link to an Anarchy Online item.
    /// Commonly known as itemref://
    /// </summary>
    public class ItemLink : Link
    {
        /// <summary>
        /// Returns the low id of the item which should be shown
        /// </summary>
        public readonly UInt32 LowID;

        /// <summary>
        /// Returns the high id of the item which should be shown
        /// </summary>
        public readonly UInt32 HighID;

        /// <summary>
        /// Returns the quality of the item which should be shown
        /// </summary>
        public readonly UInt32 Quality;

        /// <summary>
        /// Initializes a new instance of ItemLink
        /// </summary>
        /// <param name="command">The low id of the item</param>
        /// <param name="command">The high id of the item</param>
        /// <param name="command">The quality of the item</param>
        public ItemLink(UInt32 lowID, UInt32 highID, UInt32 quality)
            : base(LinkType.Item)
        {
            this.LowID = lowID;
            this.HighID = highID;
            this.Quality = quality;
        }

        /// <summary>
        /// Creates a clone of this ItemLink
        /// </summary>
        /// <returns>A new ItemLink</returns>
        public override Link Clone()
        {
            return new ItemLink(this.LowID, this.HighID, this.Quality);
        }
    }
}
