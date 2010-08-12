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

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vha.AOML;
using Vha.AOML.DOM;

namespace Vha.Chat.UI.Controls
{
    public delegate void OutputControlHandler(OutputControl sender);
    public delegate void OutputControlHandler<T>(OutputControl sender, T e);

    public class OutputControl : WebBrowser
    {
        #region Properties
        /// <summary>
        /// Gets the default template which fills the control when first loaded
        /// </summary>
        public string Template { get { return Properties.Resources.Chat; } }
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public System.Drawing.Color BackgroundColor
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
        public System.Drawing.Color ForegroundColor
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
            get { return this._cache.ElementCacheSize; }
            set { this._cache.ElementCacheSize = value; }
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

        /// <summary>
        /// Gets or sets the current Context used to handle links
        /// </summary>
        public Context Context
        {
            get { return this._context; }
            set { this._context = value; }
        }

        /// <summary>
        /// Gets or sets the context menu used to handle right clicks on links
        /// </summary>
        public new ChatContextMenu ContextMenu
        {
            get { return this._contextMenu; }
            set { this._contextMenu = value; }
        }
        #endregion

        #region Events
        public event OutputControlHandler ReadyEvent;

        public event OutputControlHandler<OutputControlClickedEventArgs> ClickedEvent;
        #endregion

        #region Methods
        /// <summary>
        /// Adds AOML code to this control
        /// </summary>
        /// <param name="aoml"></param>
        /// <param name="style"></param>
        /// <param name="scroll"></param>
        public void Write(string template, string aoml, TextStyle style, bool scroll)
        {
            Element element = this._dominizer.Parse(aoml);
            this.Write(template, element, style, scroll);
        }

        public void Write(string template, Element element, TextStyle style, bool scroll)
        {
            // Queue messages if the control isn't ready yet
            if (this.Document == null || this.Document.Body == null)
            {
                this._buffer.Enqueue(new WriteBuffer(template, element, style, scroll));
                return;
            }
            // Format message
            string html = template;
            if (element != null)
            {
                OutputControlFormatter formatter = new OutputControlFormatter(this._cache, style);
                html = string.Format(template, formatter.Format(element));
            }
            // To be able to count the lines, we need to make this a single root element
            if (this._maximumLines > 0)
                html = "<span>" + html + "</span>";
            // Write content
            this.Document.Body.InnerHtml += html;
            // Apply the maximum lines rule
            while (this._maximumLines > 0 && this.Document.Body.Children.Count > this._maximumLines)
                this.Document.Body.FirstChild.OuterHtml = "";
            // Scroll
            if (scroll)
                this.Document.InvokeScript("scrollToBottom");
        }
        #endregion

        public OutputControl() : base() { }

        #region Internal
        private System.Drawing.Color _backgroundColor = System.Drawing.Color.White;
        private System.Drawing.Color _foregroundColor = System.Drawing.Color.Black;
        private int _maximumLines = 0;
        private Context _context = null;
        private ChatContextMenu _contextMenu = null;
        private OutputControlCache _cache = new OutputControlCache();
        private Dominizer _dominizer = new Dominizer();
        private Queue<WriteBuffer> _buffer = new Queue<WriteBuffer>();
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
            this.Document.MouseUp -= new HtmlElementEventHandler(OnMouseUp);
            this.Document.MouseUp += new HtmlElementEventHandler(OnMouseUp);
            // Trigger event
            base.OnDocumentCompleted(e);
            // Clear buffer
            if (this.Document != null && this.Document.Body != null)
            {
                while (this._buffer.Count > 0)
                {
                    WriteBuffer buffer = this._buffer.Dequeue();
                    this.Write(buffer.Template, buffer.Element, buffer.Style, buffer.Scroll);
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
        }

        void OnMouseUp(object sender, HtmlElementEventArgs e)
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

            // Raise event
            OutputControlClickedEventArgs args = new OutputControlClickedEventArgs(
                type, argument, e.MouseButtonsPressed, e.ShiftKeyPressed,
                e.CtrlKeyPressed, e.MousePosition);
            if (this.ClickedEvent != null)
                this.ClickedEvent(this, args);
            if (args.Handled || this._context == null) return;

            // Default behavior
            MessageTarget target = null;
            switch (type)
            {
                case "text":
                    // Left and middle clicks on text:// shows info window
                    if (e.MouseButtonsPressed != MouseButtons.Left && e.MouseButtonsPressed != MouseButtons.Middle) return;
                    int i = -1;
                    if (!int.TryParse(argument, out i)) return;
                    Element el = this._cache.GetElement(i);
                    if (el == null) return;
                    Utils.InvokeShow(Program.ApplicationContext.MainForm, new InfoForm(this._context, Program.ApplicationContext.MainForm, el));
                    return;
                case "chatcmd":
                    // Left and middle clicks on chatcmd:// executes command
                    if (e.MouseButtonsPressed != MouseButtons.Left && e.MouseButtonsPressed != MouseButtons.Middle) return;
                    this._context.Input.Command(argument);
                    return;
                case "itemref":
                    // Left and middle clicks on itemref:// shows item window
                    if (e.MouseButtonsPressed != MouseButtons.Left && e.MouseButtonsPressed != MouseButtons.Middle) return;
                    Utils.InvokeShow(Program.ApplicationContext.MainForm, new BrowserForm(this._context, argument, BrowserFormType.Item));
                    return;
                case "character":
                    target = new MessageTarget(MessageType.Character, argument);
                    break;
                case "channel":
                    target = new MessageTarget(MessageType.Channel, argument);
                    break;
                case "privchan":
                    target = new MessageTarget(MessageType.PrivateChannel, argument);
                    break;
                default:
                    this._context.Write(MessageClass.Error, "Unknown link type '" + type + "'");
                    return;
            }
            // A 'message target' was clicked
            if (target == null || target.Type == MessageType.None) return;
            if (target.Type == MessageType.Character && target.Target.ToLower() == this._context.Character.ToLower()) return;
            if (e.MouseButtonsPressed == MouseButtons.Left || e.MouseButtonsPressed == MouseButtons.Middle)
            {
                // Show popup window
                this._context.Input.Command("open " + target.Type.ToString() + " " + target.Target);
            }
            else if (e.MouseButtonsPressed == MouseButtons.Right)
            {
                if (this._contextMenu == null) return;
                // Show 'right-click' context menu
                this._contextMenu.Show(target);
            }
        }
        #endregion

        private class WriteBuffer
        {
            public readonly string Template;
            public readonly Element Element;
            public readonly TextStyle Style;
            public readonly bool Scroll;
            public WriteBuffer(string template, Element element, TextStyle style, bool scroll)
            {
                this.Template = template;
                this.Element = element;
                this.Style = style;
                this.Scroll = scroll;
            }
        }
    }

    public class OutputControlClickedEventArgs
    {
        public readonly string Type;
        public readonly string Argument;
        public readonly MouseButtons ButtonsPressed;
        public readonly bool ShiftPressed;
        public readonly bool CtrlPressed;
        public readonly Point Position;
        public bool Handled;

        public OutputControlClickedEventArgs(string type, string argument, MouseButtons buttonsPressed, bool shiftPressed, bool ctrlPressed, Point position)
        {
            this.Type = type;
            this.Argument = argument;
            this.ButtonsPressed = buttonsPressed;
            this.ShiftPressed = shiftPressed;
            this.CtrlPressed = ctrlPressed;
            this.Position = position;
            this.Handled = false;
        }
    }
}
