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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// An element that describes a line break between its siblings.
    /// Commonly known as BR.
    /// </summary>
    public class BreakElement : Element
    {
        /// <summary>
        /// Initializes a new instance of BreakElement
        /// </summary>
        public BreakElement()
            : base(ElementType.Break, false)
        {
        }

        /// <summary>
        /// Creates a clone of this BreakElement
        /// </summary>
        /// <returns>A new BreakElement</returns>
        public override Element Clone()
        {
            return new BreakElement();
        }
    }
}
