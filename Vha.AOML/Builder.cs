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
using System.Collections.Generic;
using System.Text;
using Vha.AOML.DOM;
using Vha.AOML.Formatting;

namespace Vha.AOML
{
    public class Builder
    {
        public Builder BeginAlign(Alignment alignment)
        {
            this._push(new AlignElement(alignment));
            return this;
        }

        public Builder Align(Alignment alignment, string text)
        {
            this.BeginAlign(alignment);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder Break()
        {
            this._add(new BreakElement());
            return this;
        }

        public Builder BeginColor(Color color)
        {
            if (color == null) throw new ArgumentNullException();
            this._push(new ColorElement(color));
            return this;
        }

        public Builder BeginColor(string color)
        {
            DOM.Color c = DOM.Color.FromString(color);
            if (c == null) throw new ArgumentException("Invalid color value: " + color);
            this.BeginColor(c);
            return this;
        }

        public Builder Color(Color color, string text)
        {
            this.BeginColor(color);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder Color(string color, string text)
        {
            this.BeginColor(color);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder Begin()
        {
            this._push(new ContainerElement());
            return this;
        }

        public Builder Image(ImageType type, string image)
        {
            this._add(new ImageElement(type, image));
            return this;
        }

        public Builder BeginLink(Link link, bool stylized = true)
        {
            if (link == null) throw new ArgumentNullException();
            this._push(new LinkElement(link, stylized));
            return this;
        }

        public Builder Link(Link link, string text, bool stylized = true)
        {
            this.BeginLink(link, stylized);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder BeginCommandLink(string command, bool stylized = true)
        {
            this.BeginLink(new CommandLink(command), stylized);
            return this;
        }

        public Builder CommandLink(string command, string text, bool stylized = true)
        {
            this.BeginCommandLink(command, stylized);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder BeginWindowLink(Element popupElement, bool stylized = true)
        {
            if (popupElement == null) throw new ArgumentNullException();
            this.BeginLink(new WindowLink(popupElement), stylized);
            return this;
        }

        public Builder WindowLink(Element popupElement, string text, bool stylized = true)
        {
            this.BeginWindowLink(popupElement, stylized);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder BeginOtherLink(string uri, bool stylized = true)
        {
            this.BeginLink(new OtherLink(uri), stylized);
            return this;
        }

        public Builder OtherLink(string uri, string text, bool stylized = true)
        {
            this.BeginOtherLink(uri, stylized);
            this.Text(text);
            this.End();
            return this;
        }

        public Builder BeginItalic()
        {
            this._push(new ItalicElement());
            return this;
        }

        public Builder Italic(string text)
        {
            this.BeginItalic();
            this.Text(text);
            this.End();
            return this;
        }

        public Builder BeginUnderline()
        {
            this._push(new UnderlineElement());
            return this;
        }

        public Builder Underline(string text)
        {
            this.BeginUnderline();
            this.Text(text);
            this.End();
            return this;
        }

        public Builder Text(string text)
        {
            this._add(new TextElement(text));
            return this;
        }

        public Builder End()
        {
            if (this._depth.Count == 0)
            {
                throw new InvalidOperationException("There is no open element left to terminate");
            }
            this._depth.Pop();
            return this;
        }

        public string Format(Formatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException();
            return formatter.Format(this._root);
        }

        /// <summary>
        /// Returns an AOML DOM tree reflecting the earlier calls made to this object
        /// </summary>
        /// <returns>A copy of the output in AOML DOM format</returns>
        public Element Dominize()
        {
            return this._root.Clone();
        }

        public void Reset()
        {
            this._depth.Clear();
            this._root = new ContainerElement();
        }

        #region Internal
        private Stack<Element> _depth = new Stack<Element>();
        private ContainerElement _root = new ContainerElement();
        private Element _top()
        {
            if (this._depth.Count > 0) return this._depth.Peek();
            return this._root;
        }
        private void _push(Element element)
        {
            if (element == null)
                throw new ArgumentNullException();
            if (!element.SupportsChildren)
                throw new InvalidOperationException("Unable to push an element which doesn't support children");
            this._top().Children.Add(element);
            this._depth.Push(element);
        }
        private void _add(Element element)
        {
            if (element == null)
                throw new ArgumentNullException();
            this._top().Children.Add(element);
        }
        #endregion
    }
}
