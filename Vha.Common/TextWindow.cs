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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Vha.Common
{
    public enum TextMode
    {
        SingleQuotes,
        DoubleQuotes
    }

    public class TextWindow
    {
        protected TextMode _mode = TextMode.DoubleQuotes;
        protected string _outerQuotes = "\"";
        protected string _innerQuotes = "'";
        protected StringBuilder _builder = new StringBuilder();
        protected Int32 _characterLimit = 14000;

        public TextMode Mode { 
            get { return this._mode; }
            set
            {
                this._mode = value;
                if (this._mode == TextMode.SingleQuotes)
                {
                    this._outerQuotes = "'";
                    this._innerQuotes = "\"";
                }
                else
                {
                    this._outerQuotes = "\"";
                    this._innerQuotes = "'";
                }
            }
        }
        /// <summary>
        /// Returns the current contents of the window
        /// </summary>
        public string Text { get { return this._builder.ToString(); } }

        public TextWindow() { }
        public TextWindow(TextMode mode)
        {
            this._mode = mode;
        }

        /// <summary>
        /// Appends text and escapes it
        /// </summary>
        /// <param name="text">Text to append</param>
        public void AppendString(string text)
        {
            this._builder.Append(HTML.EscapeString(text));
        }

        /// <summary>
        /// Appends text without escaping it
        /// </summary>
        /// <param name="text">Text to append</param>
        public void AppendRawString(string text)
        {
            this._builder.Append(text);
        }

        /// <summary>
        /// Appends a clickable link to an Anarchy Online item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="lowID">LowID of the item</param>
        /// <param name="highID">HighID of the item</param>
        /// <param name="ql">Quality of the item</param>
        public void AppendItem(string name, int lowID, int highID, int ql) { this.AppendItem(name, lowID, highID, ql, false); }
        public void AppendItem(string name, int lowID, int highID, int ql, bool disableStyle)
        {
            this._builder.Append(HTML.CreateItem(name, lowID, highID, ql, disableStyle, this._innerQuotes));
        }

        public void AppendItemStart(int lowID, int highID, int ql) { this.AppendItemStart(lowID, highID, ql, false); }
        public void AppendItemStart(int lowID, int highID, int ql, bool disableStyle)
        {
            this._builder.Append(HTML.CreateItemStart(lowID, highID, ql, disableStyle, this._innerQuotes));
        }

        /// <summary>
        /// Appends a clickable command
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="command">The command itself</param>
        public void AppendCommand(string name, string command) { this.AppendCommand(name, command, false); }
        public void AppendCommand(string name, string command, bool disableStyle)
        {
            this._builder.Append(HTML.CreateCommand(name, command, disableStyle, this._innerQuotes));
        }

        public void AppendCommandStart(string command) { this.AppendCommandStart(command, false); }
        public void AppendCommandStart(string command, bool disableStyle)
        {
            this._builder.Append(HTML.CreateCommandStart(command, disableStyle, this._innerQuotes));
        }

        /// <summary>
        /// Closes a link started with "AppendItemStart()" or "AppendCommandStart()"
        /// </summary>
        public void AppendLinkEnd()
        {
            this._builder.Append(HTML.CreateLinkEnd());
        }
        /// <summary>
        /// Alias of AppendLinkEnd()
        /// </summary>
        public void AppendCommandEnd() { this.AppendLinkEnd(); }
        /// <summary>
        /// Alias of AppendLinkEnd()
        /// </summary>
        public void AppendItemEnd() { this.AppendLinkEnd(); }

        /// <summary>
        /// Appends a color opening tag
        /// </summary>
        /// <param name="colorHex">Color HEX value</param>
        public void AppendColorStart(string colorHex)
        {
            this._builder.Append(HTML.CreateColorStart(colorHex));
        }

        /// <summary>
        /// Appends a color closing tag
        /// </summary>
        public void AppendColorEnd()
        {
            this._builder.Append(HTML.CreateColorEnd());
        }

        /// <summary>
        /// Appends a colored string and closes the color tag after
        /// </summary>
        /// <param name="colorHex">Color HEX value</param>
        /// <param name="text">Text to append</param>
        public void AppendColorString(string colorHex, string text)
        {
            this._builder.Append(HTML.CreateColorString(colorHex, text));
        }

        /// <summary>
        /// Appends 1 LineBreak in Splitting mode
        /// </summary>
        public void AppendLineBreak() { this.AppendLineBreak(1, false); }
        /// <summary>
        /// Appends 1 LineBreak
        /// </summary>
        /// <param name="ignoreSplit">Defines if it should not split on this break</param>
        public void AppendLineBreak(bool ignoreSplit) { this.AppendLineBreak(1, ignoreSplit); }
        /// <summary>
        /// Appends a LineBreak in Splitting mode
        /// </summary>
        /// <param name="count">Defines how many LineBreaks to add</param>
        public void AppendLineBreak(Int32 count) { this.AppendLineBreak(count, true); }
        /// <summary>
        /// Appends a LineBreak
        /// </summary>
        /// <param name="count">Defines how many LineBreaks to add</param>
        /// <param name="ignoreSplit">Defines if it should not split on this break</param>
        public void AppendLineBreak(Int32 count, bool ignoreSplit)
        {
            string append;
            if (ignoreSplit)
                append = "<br>";
            else
                append = "\n";
            for (int i = 0; i < count; i++)
                this._builder.Append(append);
        }

        /// <summary>
        /// Appends an item icon
        /// </summary>
        /// <param name="icondID">The icon ID of the item</param>
        public void AppendIcon(Int32 icondID)
        {
            this._builder.Append(HTML.CreateIcon(icondID));
        }

        /// <summary>
        /// Appends an image
        /// </summary>
        /// <param name="imageID">The ID of the image</param>
        public void AppendImage(string imageID)
        {
            this._builder.Append(HTML.CreateImage(imageID));
        }

        /// <summary>
        /// Appends the start of an Alignment
        /// </summary>
        /// <param name="alignment">The direction the text should be align. Left, Center or Right</param>
        public void AppendAlignStart(string alignment)
        {
            this._builder.Append(HTML.CreateAlignStart(alignment));
        }

        /// <summary>
        /// Appends the end of an Alignment
        /// </summary>
        public void AppendAlignEnd()
        {
            this._builder.Append(HTML.CreateAlignEnd());
        }

        /// <summary>
        /// Converts the window to HTML ready to be send to Anarchy Online
        /// </summary>
        /// <returns>HTML String</returns>
        public override string ToString()
        {
            return this.ToString("Click to View");
        }

        /// <summary>
        /// Converts the window to HTML, ready to be send to Anarchy Online
        /// </summary>
        /// <param name="title">Name of the clickable link</param>
        /// <returns>HTML String</returns>
        public string ToString(string title) { return this.ToString(title, false); }
        public string ToString(string title, bool disableStyle)
        {
            return HTML.CreateWindow(title, this.Text, disableStyle, this._outerQuotes);
        }

        /// <summary>
        /// Converts a large window to an array of HTML pages, ready to be send to Anarchy Online
        /// </summary>
        /// <returns>An array of HTML Strings</returns>
        public virtual string[] ToStrings() { return this.ToStrings(this._characterLimit); }
        public virtual string[] ToStrings(int characterLimit) { return this.ToStrings("Click to View", characterLimit); }
        public virtual string[] ToStrings(string title) { return this.ToStrings(title, this._characterLimit); }
        public virtual string[] ToStrings(string title, int characterLimit) { return this.ToStrings(title, characterLimit, false); }
        public virtual string[] ToStrings(string title, bool disableStyle) { return this.ToStrings(title, this._characterLimit, disableStyle); }
        public string[] ToStrings(string title, int characterLimit, bool disableStyle)
        {
            if (string.IsNullOrEmpty(this.Text))
                return new string[0];

            List<string> pages = new List<string>();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(this.Text));
            StreamReader reader = new StreamReader(stream);
            string buffer = string.Empty;
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                    break;

                if (buffer.Length + line.Length > characterLimit)
                {
                    pages.Add(HTML.CreateWindow(title, buffer, disableStyle, this._outerQuotes));
                    buffer = string.Empty;
                }
                buffer += line + "\n";
            }
            if (!string.IsNullOrEmpty(buffer))
                pages.Add(HTML.CreateWindow(title, buffer, disableStyle, this._outerQuotes));

            return pages.ToArray();
        }
    }
}
