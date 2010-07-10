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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vha.Chat.Events;
using Vha.Common;

namespace Vha.Chat.UI
{
    public partial class ChatPopupForm : BaseForm
    {
        protected Context _context;
        protected Form _parent;
        protected MessageTarget _target;
        protected List<string> _history = new List<string>();
        protected int _historyIndex = 0;

        public ChatPopupForm(Context context, Form parent, MessageTarget target)
            : base(context, "ChatPopup")
        {
            InitializeComponent();

            this.Text += target.Target;

            this._target = target;
            this._parent = parent;
            this._context = context;
            this._context.MessageEvent += new Handler<MessageEventArgs>(_context_MessageEvent);

            this._outputBox.BackgroundColor = this.BackColor;
            this._outputBox.ForegroundColor = this.ForeColor;
            this._outputBox.ClickedEvent += new AomlHandler<AomlClickedEventArgs>(_outputBox_ClickedEvent);
        }

        private void ChatPopupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._context.MessageEvent -= new Handler<MessageEventArgs>(_context_MessageEvent);
        }

        private void _context_MessageEvent(Context context, MessageEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<MessageEventArgs>(_context_MessageEvent),
                    new object[] { context, args });
                return;
            }
            // Filter messages
            if (!args.Source.GetTarget().Equals(this._target)) return;
            // Format message
            string line = "";
            if (args.Source.Type == MessageType.Character)
            {
                if (args.Source.Outgoing) line += "To ";
                line += string.Format("[<span class=\"Link\">{0}</span>] ", args.Source.Character);
            }
            else
            {
                if (!string.IsNullOrEmpty(args.Source.Character))
                    line += string.Format("<span class=\"Link\">{0}</span>: ", args.Source.Character);
            }
            line += args.Message;
            string aoml = string.Format(
                "<div class=\"Line\"><span class=\"Time\">[{0:00}:{1:00}:{2:00}]</span> <span class=\"{3}\">{4}</span></div>",
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                args.Class.ToString(), line);
            this._outputBox.Write(aoml, context.Options.TextStyle, true);
        }

        private void _inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                // Sensible out
                if (this._inputBox.Text.Trim().Length == 0)
                    return;
                // History
                if (this._history.Count == 0 || this._history[0] != this._inputBox.Text)
                    this._history.Insert(0, this._inputBox.Text);
                while (this._history.Count > this._context.Options.MaximumHistory)
                    this._history.RemoveAt(this._history.Count - 1);
                this._historyIndex = 0;
                // Handle the input
                this._context.Input.Send(this._target, HTML.EscapeString(this._inputBox.Text), false);
                this._inputBox.Text = "";
            }
        }

        void _outputBox_ClickedEvent(AomlBox sender, AomlClickedEventArgs e)
        {
            switch (e.Type)
            {
                case "text":
                    Utils.InvokeShow(this._parent, new InfoForm(this._context, this, e.Argument));
                    break;
                case "chatcmd":
                    this._context.Input.Command(e.Argument);
                    break;
                case "itemref":
                    Utils.InvokeShow(this._parent, new BrowserForm(this._context, e.Argument, BrowserFormType.Item));
                    break;
                default:
                    this._context.Write(MessageClass.Error, "Unexpected link type '" + e.Type + "' in ChatPopupForm");
                    break;
            }
        }
    }
}
