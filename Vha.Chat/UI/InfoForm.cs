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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Vha.Chat;
using Vha.Chat.UI.Controls;
using Vha.AOML.DOM;

namespace Vha.Chat.UI
{
    public partial class InfoForm : BaseForm
    {
        private static List<InfoForm> _activeForms = new List<InfoForm>();

        public static bool HasActiveForms
        {
            get { lock (_activeForms) return _activeForms.Count > 0; }
        }

        public static InfoForm LatestActiveForm
        {
            get
            {
                lock (_activeForms)
                {
                    if (_activeForms.Count <= 0) return null;
                    return _activeForms[_activeForms.Count - 1];
                }
            }
        }

        protected Form _parent = null;
        protected Context _context = null;

        public InfoForm(Context context, Form parent, Element element)
            : base (context, "Info")
        {
            InitializeComponent();
            base.Initialize();

            this._context = context;
            this._parent = parent;
            this._info.Context = context;
            this._info.BackgroundColor = this.BackColor;
            this._info.ForegroundColor = this.ForeColor;
            this._info.MaximumLines = 0;
            this._info.Write("{0}", element, TextStyle.Default, false);
            this._info.InnerPadding = new Padding(7, 6, 7, 7);
            this._info.EnableImages = true;
            this._info.Initialize(this._context.Configuration.OutputMode);

            // Force options update manually
            this._context_SavedEvent(this._context, this._context.Options);
            // Listen to future updates
            this._context.Options.SavedEvent += new Handler<Options>(this._context_SavedEvent);

            // Used to keep track of open forms
            lock (_activeForms) _activeForms.Add(this);
            this.FormClosed += new FormClosedEventHandler(_onClosed);
        }

        private delegate void ReplaceContentDelegate(Element element);
        public void ReplaceContent(Element element)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new ReplaceContentDelegate(ReplaceContent),
                    new object[] { element });
                return;
            }
            this._info.Clear();
            this._info.Write("{0}", element, TextStyle.Default, false);
        }

        private void _context_SavedEvent(Context context, Options args)
        {
            this._info.TextSize = context.Options.InfoWindowTextSize;
        }

        private void _onClosed(object sender, FormClosedEventArgs e)
        {
            lock (_activeForms) _activeForms.Remove(this);
            this.FormClosed -= new FormClosedEventHandler(_onClosed);
            this._context.Options.SavedEvent -= new Handler<Options>(this._context_SavedEvent);
        }
    }
}