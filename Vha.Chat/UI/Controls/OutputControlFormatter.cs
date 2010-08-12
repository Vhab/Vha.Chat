/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using Vha.AOML.Formatting;
using Vha.AOML.DOM;
using Vha.Common;

namespace Vha.Chat.UI.Controls
{
    public class OutputControlFormatter : Formatter
    {
        public override string OnOpen() { return ""; }
        public override string OnClose() { return ""; }
        public override string OnBeforeElement(Element element) { return ""; }
        public override string OnAfterElement(Element element) { return ""; }

        public override string OnAlignOpen(AlignElement element)
        {
            if (element.Alignment == Alignment.Inherit)
                return "<div>";
            return string.Format("<div align=\"{0}\">",
                element.Alignment.ToString().ToLower());
        }

        public override string OnAlignClose(AlignElement element)
        {
            return "</div>";
        }

        public override string OnBreak(BreakElement element)
        {
            return "<br />";
        }

        public override string OnContainerOpen(ContainerElement element)
        {
            return "<span>";
        }

        public override string OnContainerClose(ContainerElement element)
        {
            return "</span>";
        }

        public override string OnColorOpen(ColorElement element)
        {
            if (this._style == TextStyle.Strip)
                return "<span>";
            Color color = element.Color;
            if (this._style == TextStyle.Invert)
                color = _invert(color);
            return string.Format("<span style=\"color: #{0:X2}{1:X2}{2:X2}\">",
                color.Red, color.Green, color.Blue);
        }

        public override string OnColorClose(ColorElement element)
        {
            return "</span>";
        }

        public override string OnImage(ImageElement element)
        {
            return "";
        }

        public override string OnLinkOpen(LinkElement element)
        {
            // Transform link
            string href = "";
            switch (element.Link.Type)
            {
                case LinkType.Command:
                    CommandLink command = (CommandLink)element.Link;
                    href = "chatcmd://" + command.Command;
                    break;
                case LinkType.Element:
                    ElementLink link = (ElementLink)element.Link;
                    href = "text://" + this._cache.CacheElement(link.Element).ToString();
                    break;
                case LinkType.Item:
                    ItemLink item = (ItemLink)element.Link;
                    href = string.Format("itemref://{0}/{1}/{2}",
                        item.LowID, item.HighID, item.Quality);
                    break;
                case LinkType.Other:
                    OtherLink other = (OtherLink)element.Link;
                    href = other.Uri.ToString();
                    break;
            }
            // Handle 'no-style' links
            string style = "";
            if (!element.Stylized)
            {
                // Find parent color
                Color color = null;
                Element parent = element;
                if (this._style != TextStyle.Strip)
                {
                    while ((parent = parent.Parent) != null)
                    {
                        if (parent.Type != ElementType.Color) continue;
                        color = ((ColorElement)parent).Color;
                        break;
                    }
                }
                // Transform color into a html style
                if (color != null)
                {
                    if (this._style == TextStyle.Invert)
                        color = _invert(color);
                    style = string.Format(
                        " class=\"NoStyle\" style=\"color: #{0:X2}{1:X2}{2:X2}\"",
                        color.Red, color.Green, color.Blue);
                }
                else
                {
                    style = " class=\"NoStyle\"";
                }
            }
            return string.Format("<a href=\"{0}\"{1}>", href, style);
        }

        public override string OnLinkClose(LinkElement element)
        {
            return "</a>";
        }

        public override string OnText(TextElement element)
        {
            return HTML.EscapeString(element.Text).Replace("  ", "&nbsp; ");
        }

        public override string OnUnderlineOpen(UnderlineElement element)
        {
            return "<u>";
        }

        public override string OnUnderlineClose(UnderlineElement element)
        {
            return "</u>";
        }

        public OutputControlFormatter(OutputControlCache cache, TextStyle style)
        {
            if (cache == null)
                throw new ArgumentNullException();
            this._cache = cache;
            this._style = style;
        }

        #region Internal
        private OutputControlCache _cache = null;
        private TextStyle _style;

        private Color _invert(Color c)
        {
            if (c == null) throw new ArgumentNullException();
            return new Color((byte)(255 - c.Red), (byte)(255 - c.Green), (byte)(255 - c.Blue));
        }
        #endregion
    }
}
