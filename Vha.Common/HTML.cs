/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;

namespace Vha.Common
{
    public static class HTML
    {
        private enum StripState
        {
            Text,
            InsideTag,
            InsideString
        }

        public static readonly String ItemLink = "<a href={0}itemref://{1}/{2}/{3}{0}{4}>";
        public static readonly String TextLink = "<a{2} href={0}text://{1}{0}>";
        public static readonly String CommandLink = "<a{2} href={0}chatcmd://{1}{0}>";
        public static readonly String LinkEnd = "</a>";
        public static readonly String CleanLink = " style={0}text-decoration:none{0}";
        public static readonly String ColorStart = "<font color=#{0}>";
        public static readonly String ColorEnd = "</font>";
        public static readonly String ImgIcon = "<img src=rdb://{0}>";
        public static readonly String ImgGui = "<img src=tdb://id:{0}>";
        public static readonly String AlignStart = "<div align='{0}'>";
        public static readonly String AlignEnd = "</div>";
        public static readonly String UnderlineStart = "<u>";
        public static readonly String UnderlineEnd = "</u>";

		/// <summary>
		/// Create an item reference, with styling and quotation character: '
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <returns></returns>
        public static string CreateItem(string name, int lowID, int highID, int QL) { return CreateItem(name, lowID, highID, QL, false, "'"); }
		/// <summary>
		/// Create an item reference with quotation character: '
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateItem(string name, int lowID, int highID, int QL, bool disableStyle) { return CreateItem(name, lowID, highID, QL, disableStyle, "'"); }
		/// <summary>
		/// Create an item reference with a custom quotation character.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateItem(string name, int lowID, int highID, int QL, bool disableStyle, string quotes)
        {
            if (lowID > highID)
            {
                int tmpID = lowID;
                lowID = highID;
                highID = tmpID;
            }
            return String.Format("{0}{1}{2}", CreateItemStart(lowID, highID, QL, disableStyle, quotes), EscapeString(name), CreateLinkEnd());
        }

		/// <summary>
		/// Create the first half of an item reference, with style and quotation character: '
		/// </summary>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <returns></returns>
        public static string CreateItemStart(int lowID, int highID, int QL) { return CreateItemStart(lowID, highID, QL, false, "'"); }
		/// <summary>
		/// Create the first half of an item reference with quotation character: '
		/// </summary>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateItemStart(int lowID, int highID, int QL, bool disableStyle) { return CreateItemStart(lowID, highID, QL, disableStyle, "'"); }
		/// <summary>
		/// Create the first half of an item reference with custom quotation character.
		/// </summary>
		/// <param name="lowID"></param>
		/// <param name="highID"></param>
		/// <param name="QL"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateItemStart(int lowID, int highID, int QL, bool disableStyle, string quotes)
        {
            if (lowID > highID)
            {
                int tmpID = lowID;
                lowID = highID;
                highID = tmpID;
            }
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            return String.Format(ItemLink, quotes, lowID, highID, QL, style);
        }

		/// <summary>
		/// Create a chat command link, with styling and quotation character: '
		/// </summary>
		/// <param name="name"></param>
		/// <param name="command"></param>
		/// <returns></returns>
        public static string CreateCommand(string name, string command) { return CreateCommand(name, command, false, "'"); }
		/// <summary>
		/// Create a chat command link with quotation character: '
		/// </summary>
		/// <param name="name"></param>
		/// <param name="command"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateCommand(string name, string command, bool disableStyle) { return CreateCommand(name, command, disableStyle, "'"); }
		/// <summary>
		/// Create a chat command link with custom quotation character.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="command"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateCommand(string name, string command, bool disableStyle, string quotes)
        {
            return String.Format("{0}{1}{2}", CreateCommandStart(command, disableStyle, quotes), EscapeString(name), CreateLinkEnd());
        }

		/// <summary>
		/// Create the first half of a chat command link, with styling and quotation character: '
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
        public static string CreateCommandStart(string command) { return CreateCommandStart(command, false, "'"); }
		/// <summary>
		/// Create the first half of a chat command link with quotation character: '
		/// </summary>
		/// <param name="command"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateCommandStart(string command, bool disableStyle) { return CreateCommandStart(command, disableStyle, "'"); }
		/// <summary>
		/// Create the first half of a chat command link with custom quotation character.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateCommandStart(string command, bool disableStyle, string quotes)
        {
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            return String.Format(CommandLink, quotes, HTML.EscapeString(command), style);
        }
		/// <summary>
		/// Create a clickable link for displaying contents in the InfoView window, with styling and quoatation character: "
		/// </summary>
		/// <param name="link"></param>
		/// <param name="contents"></param>
		/// <returns></returns>
        public static string CreateWindow(string link, string contents) { return CreateWindow(link, contents, false, "\""); }
		/// <summary>
		/// Create a clickable link for displaying contents in the InfoView window with quoatation character: "
		/// </summary>
		/// <param name="link"></param>
		/// <param name="contents"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateWindow(string link, string contents, bool disableStyle) { return CreateWindow(link, contents, disableStyle, "\""); }
		/// <summary>
		/// Create a clickable link for displaying contents in the InfoView window, with styling and custom quoatation character.
		/// </summary>
		/// <param name="link"></param>
		/// <param name="contents"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateWindow(string link, string contents, bool disableStyle, string quotes)
        {
            return String.Format("{0}{1}{2}", CreateWindowStart(contents, disableStyle, quotes), link, CreateLinkEnd());
        }

		/// <summary>
		/// Create the first half of a clickable link for displaying contents in the InfoView window, with styling and quoatation character: "
		/// </summary>
		/// <param name="contents"></param>
		/// <returns></returns>
        public static string CreateWindowStart(string contents) { return CreateWindowStart(contents, false, "\""); }
		/// <summary>
		/// Create the first half of a clickable link for displaying contents in the InfoView window with quoatation character: "
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="disableStyle"></param>
		/// <returns></returns>
        public static string CreateWindowStart(string contents, bool disableStyle) { return CreateWindowStart(contents, disableStyle, "\""); }
		/// <summary>
		/// Create the first half of a clickable link for displaying contents in the InfoView window with custom quoatation character.
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="disableStyle"></param>
		/// <param name="quotes"></param>
		/// <returns></returns>
        public static string CreateWindowStart(string contents, bool disableStyle, string quotes)
        {
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            return String.Format(TextLink, quotes, contents, style);
        }

		/// <summary>
		/// End a link. Use this when you used a Create*Start method.
		/// </summary>
		/// <returns></returns>
        public static string CreateLinkEnd()
        {
            return LinkEnd;
        }

		/// <summary>
		/// Use this to start formatting text color.
		/// </summary>
		/// <param name="colorHex"></param>
		/// <returns></returns>
        public static string CreateColorStart(string colorHex)
        {
            if (colorHex.Length == 7 && colorHex.ToCharArray()[0] == '#')
                colorHex = colorHex.Substring(1);

            if (colorHex.Length == 6)
                return string.Format(ColorStart, colorHex);
            else
                return "";
        }

		/// <summary>
		/// Use this to stop the latest text color formatting.
		/// </summary>
		/// <returns></returns>
        public static string CreateColorEnd()
        {
            return ColorEnd;
        }

		/// <summary>
		/// This will format a given string with a given color.
		/// </summary>
		/// <param name="colorHex"></param>
		/// <param name="text"></param>
		/// <returns></returns>
        public static string CreateColorString(string colorHex, string text)
        {
            text = EscapeString(text);
            string color = CreateColorStart(colorHex);
            if (color != null && color != string.Empty)
                return color + text + CreateColorEnd();
            else
                return text;
        }

		/// <summary>
		/// Creates an icon reference
		/// </summary>
		/// <param name="iconID"></param>
		/// <returns></returns>
        public static string CreateIcon(Int32 iconID)
        {
            return string.Format(ImgIcon, iconID);
        }

		/// <summary>
		/// Creates a GUI image reference
		/// </summary>
		/// <param name="imageID"></param>
		/// <returns></returns>
        public static string CreateImage(string imageID)
        {
            return string.Format(ImgGui, imageID);
        }

		/// <summary>
		/// Start aligning text
		/// </summary>
		/// <param name="alignment"></param>
		/// <returns></returns>
        public static string CreateAlignStart(string alignment)
        {
            return string.Format(AlignStart, alignment);
        }

		/// <summary>
		/// Stop aligning text
		/// </summary>
		/// <returns></returns>
        public static string CreateAlignEnd()
        {
            return AlignEnd;
        }

        public static string CreateUnderlineStart() { return HTML.UnderlineStart; }
        public static string CreateUnderlineEnd() { return HTML.UnderlineEnd; }
        public static string CreateUnderlineString(string text)
        {
            return HTML.CreateUnderlineStart() + text + HTML.CreateUnderlineEnd();
        }

        public static string StripTags(string text)
        {
            text = text.Replace("\r", "");
            text = text.Replace("\t", " ");
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            char[] charText = text.ToCharArray();
            string strippedText = String.Empty;
            StripState state = StripState.Text;
            int readFrom = 0;

            for (int i = 0; i < charText.Length; i++)
            {
                switch (charText[i])
                {
                    case '>':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.Text;
                            readFrom = i + 1;
                        }
                        break;
                    case '"':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.InsideString;
                            break;
                        }
                        if (state == StripState.InsideString)
                        {
                            state = StripState.InsideTag;
                        }
                        break;
                    case '<':
                        if (state == StripState.Text || state == StripState.InsideTag)
                        {
                            strippedText += text.Substring(readFrom, i - readFrom);
                            readFrom = i;
                            state = StripState.InsideTag;
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            strippedText += text.Substring(readFrom);
            return strippedText;
        }

		/// <summary>
		/// Replace a selection of risky characters with their html-safe equelants.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
        public static string EscapeString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("'", "&#039;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

		/// <summary>
		/// Replace html-safe codes with their actual character values 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
        public static string UnescapeString(string text)
        {
            text = text.Replace("&amp;", "&");
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&#039;", "'");
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            return text;
        }
    }
}
