/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.Drawing;
using System.IO;
using System.Web;
using System.Windows.Forms;
using Vha.AOML;
using Vha.AOML.DOM;
using Vha.Common;

namespace Vha.Chat.UI.Controls
{
    public delegate void OutputControlHandler(OutputControl sender);
    public delegate void OutputControlHandler<T>(OutputControl sender, T e);

    public enum OutputControlInitializationMode
    {
        /// <summary>
        /// Attempt to detect the optimal boot method
        /// </summary>
        Detect,
        /// <summary>
        /// Directly load the template into the control
        /// </summary>
        Direct,
        /// <summary>
        /// Delay the template loading by navigating to 'about:blank' first
        /// </summary>
        Delayed,
        /// <summary>
        /// Delay the template loading by first loading 'Bootstrap.html'
        /// </summary>
        External
    }

    public class OutputControl : WebBrowser
    {
        #region Properties
        /// <summary>
        /// Gets the default template which fills the control when first loaded
        /// </summary>
        public string Template { get { return Properties.Resources.OutputControl; } }
        /// <summary>
        /// Gets the initialization mode.
        /// </summary>
        public OutputControlInitializationMode InitializationMode { get { return this._initializationMode; } }
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public System.Drawing.Color BackgroundColor
        {
            get { return this._backgroundColor; }
            set
            {
                this._backgroundColor = value;
                this._updateProperties();
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
                this._updateProperties();
            }
        }
        /// <summary>
        /// Gets or set the content padding value
        /// </summary>
        public Padding InnerPadding
        {
            get { return this._padding; }
            set
            {
                this._padding = value;
                this._updateProperties();
            }
        }
        public int TextSize
        {
            get { return this._textSize; }
            set
            {
                this._textSize = value;
                this._updateProperties();
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

        /// <summary>
        /// Whether icons and gui images are enabled on this control
        /// </summary>
        public bool EnableImages
        {
            get { return this._enableImages; }
            set { this._enableImages = value; }
        }
        #endregion

        #region Events
        public event OutputControlHandler ReadyEvent;

        public event OutputControlHandler<OutputControlClickedEventArgs> ClickedEvent;
        #endregion

        #region Methods
        public void Initialize(OutputControlInitializationMode initializationMode)
        {
            if (initializationMode == OutputControlInitializationMode.Detect)
            {
                // When running on mono, use the bootstrap.html method
                if (Platform.Runtime == Runtime.Mono)
                    this._initializationMode = OutputControlInitializationMode.External;
                else
                    this._initializationMode = OutputControlInitializationMode.Direct;
            }
            else
            {
                // Use the configured initialization mode
                this._initializationMode = initializationMode;
            }
            // Initialize
            switch (this._initializationMode)
            {
                case OutputControlInitializationMode.Direct:
                    this.DocumentText = this.Template;
                    break;
                case OutputControlInitializationMode.Delayed:
                    this.Navigate("about:blank");
                    break;
                case OutputControlInitializationMode.External:
                    string path = "file://" + System.Environment.CurrentDirectory + Path.DirectorySeparatorChar;
                    this.Navigate(new System.Uri(path + "Bootstrap.html", System.UriKind.Absolute));
                    break;
            }
        }

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
            if (!this._initialized)
            {
                this._buffer.Enqueue(new WriteBuffer(template, element, style, scroll));
                return;
            }
            // Format message
            string html = template;
            if (element != null)
            {
                OutputControlFormatter formatter = new OutputControlFormatter(this._cache, style, this.EnableImages);
                html = string.Format(template, formatter.Format(element));
            }
            // Write output to control
            this._execute("write", html, this._maximumLines);
            // Scroll
            if (scroll)
                this._execute("scrollToBottom");
        }

        /// <summary>
        /// Clear the contents of this control
        /// </summary>
        public void Clear()
        {
            this._execute("clear");
        }
        #endregion

        public OutputControl()
            : base()
        { }

        #region Internal
        private System.Drawing.Color _backgroundColor = System.Drawing.Color.White;
        private System.Drawing.Color _foregroundColor = System.Drawing.Color.Black;
        private int _textSize = 11;
        private Padding _padding = new Padding(0);
        private int _maximumLines = 0;
        private Context _context = null;
        private ChatContextMenu _contextMenu = null;
        private OutputControlCache _cache = new OutputControlCache();
        private Dominizer _dominizer = new Dominizer();
        private Queue<WriteBuffer> _buffer = new Queue<WriteBuffer>();
        private OutputControlInitializationMode _initializationMode;
        private bool _enableImages = true;
        private bool _initialized = false;

        private void _updateProperties()
        {
            if (this.Document == null || this.Document.Body == null) return;
            // Foreground color
            string fgcolor = string.Format("#{0:X2}{1:X2}{2:X2}", this.ForegroundColor.R, this.ForegroundColor.G, this.ForegroundColor.B);
            this._execute("setForegroundColor", fgcolor);
            // Background color
            string bgcolor = string.Format("#{0:X2}{1:X2}{2:X2}", this.BackgroundColor.R, this.BackgroundColor.G, this.BackgroundColor.B);
            this._execute("setBackgroundColor", bgcolor);
            // Padding
            Padding p = this.InnerPadding;
            this._execute("setPadding", p.Top, p.Right, p.Bottom, p.Left);
            // Text size
            this._execute("setTextSize", this.TextSize);
        }

        private void _execute(string command, params object[] arguments)
        {
            this.Document.InvokeScript(command, arguments);
        }
        #endregion

        #region Internal overrides
        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            if (this._initialized) return;
            // Load template
            switch (this._initializationMode)
            {
                case OutputControlInitializationMode.Delayed:
                    this.Document.Write(this.Template);
                    break;
                case OutputControlInitializationMode.External:
                    this._execute("Write", this.Template);
                    break;
            }
            // Setup properties
            this._updateProperties();
            // Setup events
            this.Document.Click -= new HtmlElementEventHandler(OnClick);
            this.Document.Click += new HtmlElementEventHandler(OnClick);
            this.Document.MouseUp -= new HtmlElementEventHandler(OnMouseUp);
            this.Document.MouseUp += new HtmlElementEventHandler(OnMouseUp);
            // Trigger event
            this._initialized = true;
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
            if (e.Url.Scheme != "about" && e.Url.Scheme != "file")
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
            while (element != null && element.TagName.ToLower() != "a") element = element.Parent;
            if (element == null || element.TagName.ToLower() != "a") return;
            // Get href
            string href = element.GetAttribute("href").Trim('/');
            href = HttpUtility.UrlDecode(href);
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
                    // Check if we should reuse an existing form
                    if (this._context.Options.InfoWindowBehavior == InfoWindowBehavior.UseExisting)
                    {
                        InfoForm targetForm = InfoForm.LatestActiveForm;
                        if (targetForm != null)
                        {
                            targetForm.ReplaceContent(el);
                            return; // Return before creating a new form
                        }
                    }
                    // Create new form
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
                case "itemid":
                    // Left and middle clicks on itemid:// shows item window
                    if (e.MouseButtonsPressed != MouseButtons.Left && e.MouseButtonsPressed != MouseButtons.Middle) return;
                    Utils.InvokeShow(Program.ApplicationContext.MainForm, new BrowserForm(this._context, argument, BrowserFormType.Entity));
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
