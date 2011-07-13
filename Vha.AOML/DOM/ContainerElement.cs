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
    /// An element with as only purpose to contain other elements
    /// </summary>
    public class ContainerElement : Element
    {
        /// <summary>
        /// Initializes a new instance of ContainerElement
        /// </summary>
        public ContainerElement()
            : base(ElementType.Container, true)
        {}

        /// <summary>
        /// Creates a clone of this ContainerElement and its children
        /// </summary>
        /// <returns>A new ContainerElement</returns>
        public override Element Clone()
        {
            Element clone = new ContainerElement();
            foreach (Element child in this.Children)
            {
                clone.Children.Add(child.Clone());
            }
            return clone;
        }
    }
}
