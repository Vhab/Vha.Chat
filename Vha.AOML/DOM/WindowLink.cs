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
    /// A link to an element tree to be shown.
    /// Commonly known as text://
    /// </summary>
    public class WindowLink : Link
    {
        /// <summary>
        /// Returns the element tree which should be shown
        /// </summary>
        public readonly Element Element;

        /// <summary>
        /// Initializes a new instance of ElementLink
        /// </summary>
        /// <param name="element">The element tree which should be shown</param>
        public WindowLink(Element element)
            : base(LinkType.Window)
        {
            if (element == null)
                throw new ArgumentNullException();
            this.Element = element;
        }

        /// <summary>
        /// Creates a clone of this ElementLink
        /// </summary>
        /// <returns>A new ElementLink</returns>
        public override Link Clone()
        {
            return new WindowLink(this.Element);
        }
    }
}
