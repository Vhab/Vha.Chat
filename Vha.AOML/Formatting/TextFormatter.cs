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
    /// <summary>
    /// A Formatter which transforms an Element tree into an ascii representation of the tree
    /// </summary>
    public class TextFormatter : Formatter
    {
        public override string OnOpen() { return ""; }

        public override string OnClose() { return ""; }

        public override string OnBeforeElement(Element element)
        {
            string result =
                this._prefix.Peek() + "+ " +
                element.Type.ToString().ToUpper() + "\n";
            this._prefix.Push(this._prefix.Peek() + "  ");
            return result;
        }

        public override string OnAfterElement(Element element)
        {
            this._prefix.Pop();
            return "";
        }

        public override string OnAlignOpen(AlignElement element)
        {
            return this._prefix.Peek() +
                "align=" + element.Alignment.ToString() + "\n";
        }

        public override string OnAlignClose(AlignElement element) { return ""; }

        public override string OnBreak(BreakElement element) { return ""; }

        public override string OnContainerOpen(ContainerElement element) { return ""; }

        public override string OnContainerClose(ContainerElement element) { return ""; }

        public override string OnColorOpen(ColorElement element)
        {
            return this._prefix.Peek() +
                "color=" + element.Color.ToString() + "\n";
        }

        public override string OnColorClose(ColorElement element) { return ""; }

        public override string OnImage(ImageElement element)
        {
            return this._prefix.Peek() +
                "type=" + element.ImageType.ToString() +
                " value=" + element.Image + "\n";
        }
        
        public override string OnLinkOpen(LinkElement element)
        {
            switch (element.Link.Type)
            {
                case LinkType.Command:
                    CommandLink command = (CommandLink)element.Link;
                    return this._prefix.Peek() +
                        "command=" + command.Command + "\n";
                case LinkType.Window:
                    WindowLink el = (WindowLink)element.Link;
                    Formatter f = new TextFormatter(this._prefix.Peek() + "| ");
                    return f.Format(el.Element);                    
                case LinkType.Item:
                    ItemLink item = (ItemLink)element.Link;
                    return this._prefix.Peek() +
                        "lid=" + item.LowID.ToString() +
                        " hid=" + item.HighID.ToString() +
                        " ql=" + item.Quality.ToString() + "\n";
                case LinkType.User:
                    UserLink user = (UserLink)element.Link;
                    return this._prefix.Peek() +
                        "character=" + user.Character + "\n";
                case LinkType.Other:
                    OtherLink other = (OtherLink)element.Link;
                    return this._prefix.Peek() +
                        "uri=" + other.Uri.ToString() + "\n";
                case LinkType.Invalid:
                    InvalidLink invalid = (InvalidLink)element.Link;
                    return this._prefix.Peek() +
                        "uri=" + invalid.Raw + "\n";
                default:
                    return this._prefix.Peek() +
                        "unknown LinkType: " +
                        element.Link.Type.ToString() + "\n";
            }
        }

        public override string OnLinkClose(LinkElement element) { return ""; }

        public override string OnText(TextElement element)
        {
            return this._prefix.Peek() +
                "text=" + element.Text.Replace("\n", "\\n") + "\n";
        }

        public override string OnUnderlineOpen(UnderlineElement element) { return ""; }

        public override string OnUnderlineClose(UnderlineElement element) { return ""; }

        public override string OnItalicOpen(ItalicElement element) { return ""; }

        public override string OnItalicClose(ItalicElement element) { return ""; }

        public TextFormatter() { this._prefix.Push(""); }
        public TextFormatter(string prefix) { this._prefix.Push(prefix); }

        private Stack<string> _prefix = new Stack<string>();
    }
}
