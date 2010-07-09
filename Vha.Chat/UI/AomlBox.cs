using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Vha.Chat.UI
{
    public delegate void AomlHandler(AomlBox sender);
    public delegate void AomlHandler<T>(AomlBox sender, T e);

    public class AomlLink { }

    public class AomlBox : WebBrowser
    {
        public AomlBox() : base() { }

        #region Properties
        /// <summary>
        /// Gets or sets the default template which fills the control when first loaded
        /// </summary>
        public string Template { get { return this._template; } }
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public Color BackgroundColor;
        /// <summary>
        /// Gets or sets the foreground color
        /// </summary>
        public Color ForegroundColor;
        #endregion

        #region Events
        //public event AomlHandler ReadyEvent;

        //public event AomlHandler<AomlLink> ClickedEvent;
        #endregion

        #region Methods
        /// <summary>
        /// Adds AOML code to this control
        /// </summary>
        /// <param name="aoml"></param>
        /// <param name="style"></param>
        /// <param name="oneElement"></param>
        public void Write(string aoml, TextStyle style, bool oneElement) { }
        /// <summary>
        /// Clears lines of AOML code from this control
        /// </summary>
        /// <param name="maximumElementsRemaining">The maximum amount of root elements remaining after clearing</param>
        /// <param name="top">Whether to start clearing at the top or at the bottom</param>
        public void Clear(int maximumElementsRemaining, bool top) { }
        #endregion

        #region Private methods and members
        private string _template = "";
        private Color _backgroundColor = Color.White;
        private Color _foregroundColor = Color.Black;

        private string _invertColors(string aoml) { return null; }

        private string _stripColors(string aoml) { return null; }

        private void _secureElement(HtmlElement element) { return; }
        #endregion

        #region Internal overrides
        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
        }
        #endregion
    }
}
