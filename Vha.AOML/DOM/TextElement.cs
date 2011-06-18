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
    /// An element that represents a piece of text within the DOM tree
    /// </summary>
    public class TextElement : Element
    {
        /// <summary>
        /// Returns the text contained within this element
        /// </summary>
        public readonly string Text;

        /// <summary>
        /// Initializes a new instance of TextElement
        /// </summary>
        /// <param name="text">The text to be contained within this element</param>
        public TextElement(string text)
            : base(ElementType.Text, false)
        {
            this.Text = text;
        }

        /// <summary>
        /// Creates a clone of this TextElement
        /// </summary>
        /// <returns>A new TextElement</returns>
        public override Element Clone()
        {
            return new TextElement(this.Text);
        }
    }
}
