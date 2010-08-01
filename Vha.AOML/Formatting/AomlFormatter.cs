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
using Vha.AOML.DOM;
using Vha.Common;

namespace Vha.AOML.Formatting
{
    public enum AomlFormatterStyle
    {
        SingleQuote,
        DoubleQuote
    }
    /// <summary>
    /// A Formatter which transforms an Element tree into an AOML string
    /// </summary>
    public class AomlFormatter : Formatter
    {
        public readonly AomlFormatterStyle Style;
        public string Quote
        {
            get
            {
                if (this.Style == AomlFormatterStyle.DoubleQuote)
                    return "\"";
                if (this.Style == AomlFormatterStyle.SingleQuote)
                    return "'";
                return null;
            }
        }

        public AomlFormatter()
        {
            this.Style = AomlFormatterStyle.DoubleQuote;
        }
        public AomlFormatter(AomlFormatterStyle style)
        {
            this.Style = style;
        }

        public override string OnOpen() { return ""; }
        public override string OnClose() { return ""; }
        public override string OnBeforeElement(Element element) { return ""; }
        public override string OnAfterElement(Element element) { return ""; }

        public override string OnAlignOpen(AlignElement element)
        {
            if (element.Alignment == Alignment.Inherit)
                return "<div>";
            return string.Format("<div align={0}{1}{0}",
                this.Quote, element.Alignment.ToString().ToLower());
        }

        public override string OnAlignClose(AlignElement element)
        {
            return "</div>";
        }

        public override string OnBreak(BreakElement element)
        {
            return "<br>";
        }

        public override string OnContainerOpen(ContainerElement element)
        {
            return "";
        }

        public override string OnContainerClose(ContainerElement element)
        {
            return "";
        }

        public override string OnColorOpen(ColorElement element)
        {
            return string.Format("<font color={0}#{1:X2}{2:X2}{3:X2}{0}>",
                this.Quote, element.Color.Red, element.Color.Green, element.Color.Blue);
        }

        public override string OnColorClose(ColorElement element)
        {
            return "</font>";
        }

        public override string OnImage(ImageElement element)
        {
            return string.Format("<img src={0}{1}://{2}{0}>",
                this.Quote, element.ImageType.ToString(), element.Image);
        }

        public override string OnLinkOpen(LinkElement element)
        {
            string href = "";
            string style = "";
            switch (element.Link.Type)
            {
                case LinkType.Command:
                    CommandLink command = (CommandLink)element.Link;
                    href = "chatcmd://" + command.Command;
                    break;
                case LinkType.Element:
                    ElementLink link = (ElementLink)element.Link;
                    Formatter f = null;
                    if (this.Style == AomlFormatterStyle.DoubleQuote)
                        f = new AomlFormatter(AomlFormatterStyle.SingleQuote);
                    else f = new PlainTextFormatter();
                    href = "text://" + f.Format(link.Element);
                    break;
                case LinkType.Item:
                    ItemLink item = (ItemLink)element.Link;
                    href = string.Format("itemref://{0}/{1}/{2}",
                        item.LowID, item.HighID, item.Quality);
                    break;
            }
            if (!element.Stylized)
                style = string.Format("{0}text-decoration:none{0}", this.Quote);
            return string.Format("<a href={0}{1}{0}{2}>",
                this.Quote, href, style);
        }

        public override string OnLinkClose(LinkElement element)
        {
            return "</a>";
        }

        public override string OnText(TextElement element)
        {
            return HTML.EscapeString(element.Text);
        }

        public override string OnUnderlineOpen(UnderlineElement element)
        {
            return "<u>";
        }

        public override string OnUnderlineClose(UnderlineElement element)
        {
            return "</u>";
        }
    }
}
