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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// Identifies the alignment of an AlignElement
    /// </summary>
    public enum Alignment
    {
        Inherit,
        Left,
        Right,
        Center
    }

    /// <summary>
    /// An element that describes the alignment of its child elements.
    /// Additionally, this element describes all elements within are contained on their own line (or lines).
    /// Commonly known as DIV.
    /// </summary>
    public class AlignElement : Element
    {
        /// <summary>
        /// Returns the alignment of this element's children
        /// </summary>
        public readonly Alignment Alignment;

        /// <summary>
        /// Initializes a new instance of AlignElement
        /// </summary>
        /// <param name="alignment">The alignment of this element</param>
        public AlignElement(Alignment alignment)
            : base(ElementType.Align, true)
        {
            this.Alignment = alignment;
        }

        /// <summary>
        /// Creates a clone of this AlignElement and its children
        /// </summary>
        /// <returns>A new AlignElement</returns>
        public override Element Clone()
        {
            Element clone = new AlignElement(this.Alignment);
            foreach (Element child in this.Children)
                clone.Children.Add(child.Clone());
            return clone;
        }
    }
}
