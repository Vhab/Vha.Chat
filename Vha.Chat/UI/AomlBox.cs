/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Vha.Chat.UI
{
    public delegate void AomlHandler(AomlBox sender);
    public delegate void AomlHandler<T>(AomlBox sender, T e);

    public class AomlClickedEventArgs
    {
        public readonly string Type;
        public readonly string Argument;
        public readonly MouseButtons ButtonsPressed;
        public readonly bool ShiftPressed;
        public readonly bool CtrlPressed;
        public readonly Point Position;

        public AomlClickedEventArgs(string type, string argument, MouseButtons buttonsPressed, bool shiftPressed, bool ctrlPressed, Point position)
        {
            this.Type = type;
            this.Argument = argument;
            this.ButtonsPressed = buttonsPressed;
            this.ShiftPressed = shiftPressed;
            this.CtrlPressed = ctrlPressed;
            this.Position = position;
        }
    }

    public class AomlBox : WebBrowser
    {
        public AomlBox() : base() { }

        #region Properties
        /// <summary>
        /// Gets the default template which fills the control when first loaded
        /// </summary>
        public string Template { get { return Properties.Resources.Chat; } }
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public Color BackgroundColor
        {
            get { return this._backgroundColor; }
            set
            {
                this._backgroundColor = value;
                if (this.Document == null) return;
                this.Document.BackColor = value;
            }
        }
        /// <summary>
        /// Gets or sets the foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get { return this._foregroundColor; }
            set
            {
                this._foregroundColor = value;
                if (this.Document == null || this.Document.Body == null) return;
                string color = string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                this.Document.Body.Style = "color: " + color + ";";
            }
        }
        /// <summary>
        /// The amount of text:// values are cached
        /// </summary>
        public int MaximumTexts
        {
            get { return this._maximumTexts; }
            set { this._maximumTexts = value; }
        }
        /// <summary>
        /// Sets the maximum amount of root elements in this box.
        /// Setting this to a value > 0 will cause all added aoml to be considered a single element.
        /// </summary>
        public int MaximumLines
        {
            get { return this._maximumLines; }
            set { this._maximumLines = value; }
        }
        #endregion

        #region Events
        public event AomlHandler ReadyEvent;

        public event AomlHandler<AomlClickedEventArgs> ClickedEvent;
        #endregion

        #region Methods
        /// <summary>
        /// Adds AOML code to this control
        /// </summary>
        /// <param name="aoml"></param>
        /// <param name="style"></param>
        /// <param name="scroll"></param>
        public void Write(string aoml, TextStyle style, bool scroll)
        {
            if (this.Document == null || this.Document.Body == null)
            {
                this._buffer.Enqueue(new WriteBuffer(aoml, style, scroll));
                return;
            }
            // Create regex parser
            if (this._charrefRegex == null)
                this._charrefRegex = new Regex("charref://[0-9]+/[0-9]+/");
            if (this._textsRegex == null)
                this._textsRegex = new Regex("href=(\"|')text://([^\\1]*?)\\1");
            // Some preprocessing
            aoml = aoml.Replace("\n", "<br>");
            aoml = this._charrefRegex.Replace(aoml, "text://");
            // Find text:// links and strip them out
            MatchCollection matches = this._textsRegex.Matches(aoml);
            foreach (Match match in matches)
            {
                // Store text
                this._textsIndex++;
                if (this._textsIndex > this._maximumTexts) this._textsIndex = 0;
                this._texts[this._textsIndex.ToString()] = match.Groups[2].Value;
                // Replace link
                string seperator = match.Groups[1].Value;
                string replacement = string.Format("href={0}text://{1}{0}", seperator, this._textsIndex);
                aoml = aoml.Replace(match.Groups[0].Value, replacement);
            }
            // Strip images the aggressive way
            aoml = _stripImages(aoml);
            // Invert or strip colors if needed
            if (style == TextStyle.Invert)
                aoml = _invertColors(aoml);
            else if (style == TextStyle.Strip)
                aoml = _stripColors(aoml);
            // Some hardcore cheating
            HtmlElement tag = this.Document.CreateElement("div");
            // - Without the pre tags, double whitespaces will be stripped
            tag.InnerHtml = "<pre>" + aoml + "</pre>";
            // - FirstChild is our <pre> tag, we replace double whitespaces here to make them visible
            tag.FirstChild.InnerHtml = tag.FirstChild.InnerHtml.Replace("  ", "&nbsp; ");
            // - Final round of html fixing (and font stripping if needed)
            _secureElement(tag.FirstChild);
            // - Ensure we don't have any stray data
            aoml = tag.FirstChild.InnerHtml;
            // - To be able to count the lines, we need to make this a single root element
            if (this._maximumLines > 0)
                aoml = "<span>" + aoml + "</span>";
            // Fill content
            this.Document.Body.InnerHtml += aoml;
            // Apply the maximum lines rule
            while (this._maximumLines > 0 && this.Document.Body.Children.Count > this._maximumLines)
                this.Document.Body.FirstChild.OuterHtml = "";
            // Scroll
            if (scroll)
                this.Document.InvokeScript("scrollToBottom");
        }
        #endregion

        #region Private methods and members
        private Color _backgroundColor = Color.White;
        private Color _foregroundColor = Color.Black;
        private int _maximumTexts = 50;
        private int _maximumLines = 0;

        private Queue<WriteBuffer> _buffer = new Queue<WriteBuffer>();
        private Dictionary<string, string> _texts = new Dictionary<string,string>();
        private int _textsIndex = 0;

        private Regex _textsRegex = null;
        private Regex _charrefRegex = null;
        private Regex _colorRegex = null;
        private Regex _fontRegex = null;
        private Regex _imgRegex = null;

        protected string _invertColors(string aoml)
        {
            if (this._colorRegex == null)
                this._colorRegex = new Regex("color=(['\"]?)#([0-9a-fA-F]{6})\\1");
            MatchCollection matches = this._colorRegex.Matches(aoml);
            foreach (Match match in matches)
            {
                string seperator = match.Groups[1].Value;
                string color = match.Groups[2].Value;
                int r = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int g = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int b = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                string inverse = string.Format("{0:X2}{1:X2}{2:X2}", 255 - r, 255 - g, 255 - b);
                string replacement = string.Format("color={0}#{1}{0}", seperator, inverse);
                aoml = aoml.Substring(0, match.Index) + replacement + aoml.Substring(match.Index + replacement.Length);
            }
            return aoml;
        }

        protected string _stripImages(string aoml)
        {
            if (this._imgRegex == null)
                this._imgRegex = new Regex("[<][/]?img[^><]*[>]");
            MatchCollection matches = this._imgRegex.Matches(aoml);
            int offset = 0;
            foreach (Match match in matches)
            {
                aoml = aoml.Substring(0, match.Index - offset) + aoml.Substring(match.Index + match.Length - offset);
                offset += match.Length;
            }
            return aoml;
        }

        protected string _stripColors(string aoml)
        {
            if (this._fontRegex == null)
                this._fontRegex = new Regex("[<][/]?font[^><]*[>]");
            MatchCollection matches = this._fontRegex.Matches(aoml);
            int offset = 0;
            foreach (Match match in matches)
            {
                aoml = aoml.Substring(0, match.Index - offset) + aoml.Substring(match.Index + match.Length - offset);
                offset += match.Length;
            }
            return aoml;
        }

        protected void _secureElement(HtmlElement element)
        {
            // Early out
            if (element.FirstChild == null) return;
            // Constraints
            List<string> validTags = new List<string>();
            validTags.Add("pre");
            validTags.Add("span");
            validTags.Add("div");
            validTags.Add("font");
            validTags.Add("a");
            validTags.Add("br");
            validTags.Add("center");
            validTags.Add("left");
            validTags.Add("right");
            List<string> validAttributes = new List<string>();
            validAttributes.Add("style");
            validAttributes.Add("align");
            validAttributes.Add("className");
            validAttributes.Add("color");
            List<string> validHrefs = new List<string>();
            validHrefs.Add("text://");
            validHrefs.Add("itemref://");
            validHrefs.Add("channel://");
            validHrefs.Add("privchan://");
            validHrefs.Add("character://");
            validHrefs.Add("chatcmd://");
            // Gather all elements
            List<HtmlElement> elements = new List<HtmlElement>();
            Stack<HtmlElement> todo = new Stack<HtmlElement>();
            todo.Push(element.FirstChild);
            while (todo.Count > 0)
            {
                HtmlElement current = todo.Pop();
                while (current != null)
                {
                    HtmlElement next = current.NextSibling;
                    // Check type against white-list
                    string tag = current.TagName.ToLower();
                    if (!validTags.Contains(tag))
                    {
                        if (current.InnerHtml != null)
                            current.InnerHtml = "";
                        current.OuterHtml = "";
                    }
                    else
                    {
                        // Queue children for processing
                        if (current.FirstChild != null)
                        {
                            todo.Push(current.FirstChild);
                        }
                    }
                    elements.Add(current);
                    current = next;
                }
            }
            // Process elements
            while (elements.Count > 0)
            {
                // Get the last one
                HtmlElement current = elements[elements.Count - 1];
                elements.RemoveAt(elements.Count - 1);
                HtmlElement replacement = null;
                // Special case for fonts
                if (current.TagName.ToLower() == "font")
                {
                    // Fonts are annoying, let's not even try to fix em...
                    continue;
                }
                // Default case
                {
                    // Recreate element with white-listed attributes
                    replacement = this.Document.CreateElement(current.TagName);
                    // - Copy over white-listed attributes
                    foreach (string attribute in validAttributes)
                    {
                        string value = current.GetAttribute(attribute);
                        if (string.IsNullOrEmpty(value)) continue;
                        replacement.SetAttribute(attribute, value);
                    }
                    // - Copy over white-listed href
                    string href = current.GetAttribute("href");
                    if (!string.IsNullOrEmpty(href))
                    {
                        foreach (string validHref in validHrefs)
                        {
                            if (href.ToLower().StartsWith(validHref))
                            {
                                replacement.SetAttribute("href", href);
                                break;
                            }
                        }
                    }
                }
                if (current.InnerText != null)
                {
                    replacement.InnerHtml = current.InnerHtml;
                }
                current.OuterHtml = replacement.OuterHtml;
                // Clean up
                replacement.OuterHtml = "";
                replacement = null;
            }
        }
        #endregion

        #region Internal overrides
        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            // Write template
            this.Document.Write(this.Template);
            // Setup colors
            this.BackgroundColor = this._backgroundColor;
            this.ForegroundColor = this._foregroundColor;
            // Setup events
            this.Document.Click -= new HtmlElementEventHandler(OnClick);
            this.Document.Click += new HtmlElementEventHandler(OnClick);
            // Trigger event
            base.OnDocumentCompleted(e);
            // Clear buffer
            if (this.Document != null && this.Document.Body != null)
            {
                while (this._buffer.Count > 0)
                {
                    WriteBuffer buffer = this._buffer.Dequeue();
                    this.Write(buffer.Aoml, buffer.Style, buffer.Scroll);
                }
                // Fire ready event
                if (this.ReadyEvent != null)
                    this.ReadyEvent(this);
            }
        }

        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Scheme != "about")
            {
                e.Cancel = true;
                return;
            }
            base.OnNavigating(e);
        }

        protected void OnClick(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = false;
            // Get element
            HtmlDocument document = (HtmlDocument)sender;
            HtmlElement element = document.GetElementFromPoint(e.ClientMousePosition);
            if (element == null || element.TagName.ToLower() != "a") return;
            // Get href
            string href = element.GetAttribute("href").Trim('/');
            if (string.IsNullOrEmpty(href)) return;
            // Parse href
            int index = href.IndexOf("://");
            if (index <= 0) return;
            string type = href.Substring(0, index);
            string argument = href.Substring(index + 3);
            // Extract argument from text cache
            if (type.ToLower() == "text")
            {
                if (this._texts.ContainsKey(argument))
                    argument = this._texts[argument];
                else argument = "";
            }
            // Raise event
            AomlClickedEventArgs args = new AomlClickedEventArgs(
                type, argument, e.MouseButtonsPressed, e.ShiftKeyPressed,
                e.CtrlKeyPressed, e.MousePosition);
            if (this.ClickedEvent != null)
                this.ClickedEvent(this, args);
        }
        #endregion

        private class WriteBuffer
        {
            public readonly string Aoml;
            public readonly TextStyle Style;
            public readonly bool Scroll;
            public WriteBuffer(string aoml, TextStyle style, bool scroll)
            {
                this.Aoml = aoml;
                this.Style = style;
                this.Scroll = scroll;
            }
        }
    }
}
