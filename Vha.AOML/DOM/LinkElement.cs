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
    /// An element that describes an action related to its child elements.
    /// Commonly known as A.
    /// </summary>
    public class LinkElement : Element
    {
        /// <summary>
        /// Returns the action this element describes
        /// </summary>
        public readonly Link Link;

        /// <summary>
        /// Returns whether this link has to be stylized as a link
        /// </summary>
        public readonly bool Stylized;

        /// <summary>
        /// Initializes a new instance of LinkElement
        /// </summary>
        /// <param name="link">The action (link) related to this element</param>
        public LinkElement(Link link)
            : base(ElementType.Link, true)
        {
            if (link == null)
                throw new ArgumentNullException();
            this.Link = link;
        }

        /// <summary>
        /// Initializes a new instance of LinkElement
        /// </summary>
        /// <param name="link">The action (link) related to this element</param>
        /// <param name="stylized">Whether this link has to be stylized</param>
        public LinkElement(Link link, bool stylized)
            : base(ElementType.Link, true)
        {
            if (link == null)
                throw new ArgumentNullException();
            this.Link = link;
            this.Stylized = stylized;
        }

        /// <summary>
        /// Creates a clone of this LinkElement and its children
        /// </summary>
        /// <returns>A new LinkElement</returns>
        public override Element Clone()
        {
            Element clone = new LinkElement(this.Link, this.Stylized);
            foreach (Element child in this.Children)
                clone.Children.Add(child.Clone());
            return clone;
        }
    }
}
