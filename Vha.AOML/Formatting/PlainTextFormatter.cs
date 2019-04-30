/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
    /// A Formatter which transforms an Element tree into only plain text without any formatting
    /// </summary>
    public class PlainTextFormatter : Formatter
    {
        public override string OnAlignOpen(AlignElement element)
        {
            string result = "";
            if (this.state == State.Text) { result = "\n"; }
            this.state = State.Alignment;
            return result;
        }

        public override string OnAlignClose(AlignElement element)
        {
            string result = "";
            if (this.state == State.Text) result = "\n";
            this.state = State.Alignment;
            return result;
        }

        public override string OnText(TextElement element)
        {
            this.state = State.Text;
            return element.Text;
        }

        public override string OnOpen() { return ""; }
        public override string OnClose() { return ""; }
        public override string OnBeforeElement(Element element) { return ""; }
        public override string OnAfterElement(Element element) { return ""; }
        public override string OnBreak(BreakElement element) { return "\n"; }
        public override string OnContainerOpen(ContainerElement element) { return ""; }
        public override string OnContainerClose(ContainerElement element) { return ""; }
        public override string OnColorOpen(ColorElement element) { return ""; }
        public override string OnColorClose(ColorElement element) { return ""; }
        public override string OnImage(ImageElement element) { return ""; }
        public override string OnLinkOpen(LinkElement element) { return ""; }
        public override string OnLinkClose(LinkElement element) { return ""; }
        public override string OnUnderlineOpen(UnderlineElement element) { return ""; }
        public override string OnUnderlineClose(UnderlineElement element) { return ""; }
        public override string OnItalicOpen(ItalicElement element) { return ""; }
        public override string OnItalicClose(ItalicElement element) { return ""; }

        #region internal
        private enum State
        {
            Start,
            Alignment,
            Text
        }
        private State state = State.Start;
        #endregion
    }
}
