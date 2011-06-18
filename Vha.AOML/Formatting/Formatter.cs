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

namespace Vha.AOML.Formatting
{
    public abstract class Formatter
    {
        public abstract string OnOpen();
        public abstract string OnClose();

        public abstract string OnBeforeElement(Element element);
        public abstract string OnAfterElement(Element element);

        public abstract string OnAlignOpen(AlignElement element);
        public abstract string OnAlignClose(AlignElement element);
        public abstract string OnBreak(BreakElement element);
        public abstract string OnContainerOpen(ContainerElement element);
        public abstract string OnContainerClose(ContainerElement element);
        public abstract string OnColorOpen(ColorElement element);
        public abstract string OnColorClose(ColorElement element);
        public abstract string OnImage(ImageElement element);
        public abstract string OnLinkOpen(LinkElement element);
        public abstract string OnLinkClose(LinkElement element);
        public abstract string OnText(TextElement element);
        public abstract string OnUnderlineOpen(UnderlineElement element);
        public abstract string OnUnderlineClose(UnderlineElement element);
        public abstract string OnItalicOpen(ItalicElement element);
        public abstract string OnItalicClose(ItalicElement element);

        public string Format(Element element)
        {
            if (element == null) return null;
            StringBuilder result = new StringBuilder();
            result.Append(this.OnOpen());
            this.Format(ref result, element);
            result.Append(this.OnClose());
            return result.ToString();
        }

        public void Format(ref StringBuilder result, Element element)
        {
            if (result == null || element == null)
                throw new ArgumentNullException();
            // Open element
            result.Append(this.OnBeforeElement(element));
            switch (element.Type)
            {
                case ElementType.Align:
                    AlignElement align = (AlignElement)element;
                    result.Append(this.OnAlignOpen(align));
                    break;
                case ElementType.Break:
                    BreakElement linebreak = (BreakElement)element;
                    result.Append(this.OnBreak(linebreak));
                    break;
                case ElementType.Color:
                    ColorElement color = (ColorElement)element;
                    result.Append(this.OnColorOpen(color));
                    break;
                case ElementType.Container:
                    ContainerElement container = (ContainerElement)element;
                    result.Append(this.OnContainerOpen(container));
                    break;
                case ElementType.Image:
                    ImageElement image = (ImageElement)element;
                    result.Append(this.OnImage(image));
                    break;
                case ElementType.Link:
                    LinkElement link = (LinkElement)element;
                    result.Append(this.OnLinkOpen(link));
                    break;
                case ElementType.Text:
                    TextElement text = (TextElement)element;
                    result.Append(this.OnText(text));
                    break;
                case ElementType.Underline:
                    UnderlineElement underline = (UnderlineElement)element;
                    result.Append(this.OnUnderlineOpen(underline));
                    break;
                case ElementType.Italic:
                    ItalicElement italic = (ItalicElement)element;
                    result.Append(this.OnItalicOpen(italic));
                    break;
            }
            // Process children
            if (element.Children != null)
            {
                foreach (Element child in element.Children)
                {
                    this.Format(ref result, child);
                }
            }
            // Close element
            switch (element.Type)
            {
                case ElementType.Align:
                    AlignElement align = (AlignElement)element;
                    result.Append(this.OnAlignClose(align));
                    break;
                case ElementType.Color:
                    ColorElement color = (ColorElement)element;
                    result.Append(this.OnColorClose(color));
                    break;
                case ElementType.Container:
                    ContainerElement container = (ContainerElement)element;
                    result.Append(this.OnContainerClose(container));
                    break;
                case ElementType.Link:
                    LinkElement link = (LinkElement)element;
                    result.Append(this.OnLinkClose(link));
                    break;
                case ElementType.Underline:
                    UnderlineElement underline = (UnderlineElement)element;
                    result.Append(this.OnUnderlineClose(underline));
                    break;
                case ElementType.Italic:
                    ItalicElement italic = (ItalicElement)element;
                    result.Append(this.OnItalicClose(italic));
                    break;
            }
            result.Append(this.OnAfterElement(element));
        }
    }
}
