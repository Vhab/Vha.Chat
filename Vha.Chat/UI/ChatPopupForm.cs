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
        protected ChatContextMenu _contextMenu;
        protected MessageTarget _target;
        protected List<string> _history = new List<string>();
        protected int _historyIndex = 0;

        public ChatPopupForm(Context context, MessageTarget target)
            : base(context, "ChatPopup")
        {
            InitializeComponent();
            base.Initialize();

            this.Text += target.Target;

            this._target = target;
            this._context = context;
            this._context.MessageEvent += new Handler<MessageEventArgs>(_context_MessageEvent);

            this._contextMenu = new ChatContextMenu(this, context);
            this._outputBox.ContextMenu = this._contextMenu;
            this._outputBox.Context = context;
            this._outputBox.BackgroundColor = this.BackColor;
            this._outputBox.ForegroundColor = this.ForeColor;
            this._outputBox.Initialize(context.Platform != Platform.DotNet);

            // Preload output window with messages
            MessageEventArgs[] messages = context.GetHistory(target);
            foreach (MessageEventArgs message in messages)
                _context_MessageEvent(context, message);
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
            string prefix = "";
            if (args.Source.Type == MessageType.Character)
            {
                if (args.Source.Outgoing) prefix += "To ";
                prefix += string.Format("[<span class=\"Link\">{0}</span>] ", args.Source.Character);
            }
            else
            {
                if (!string.IsNullOrEmpty(args.Source.Character))
                    prefix += string.Format("<a href=\"character://{0}\" class=\"Link\">{0}</a>: ", args.Source.Character);
            }
            string template = string.Format(
                "<div class=\"Line\"><span class=\"Time\">[{0:00}:{1:00}:{2:00}]</span> <span class=\"{3}\">{4}{{0}}</span></div>",
                args.Time.Hour, args.Time.Minute, args.Time.Second,
                args.Class.ToString(), prefix);
            this._outputBox.Write(template, args.Message, context.Options.TextStyle, true);
        }

        private void _inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (this._historyIndex < this._history.Count)
                {
                    this._historyIndex++;
                    this._inputBox.Text = this._history[this._historyIndex - 1];
                    this._inputBox.Select(this._inputBox.Text.Length, 0);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (this._historyIndex > 0)
                {
                    this._historyIndex--;
                    if (this._historyIndex == 0)
                        this._inputBox.Text = "";
                    else
                        this._inputBox.Text = this._history[this._historyIndex - 1];
                    this._inputBox.Select(this._inputBox.Text.Length, 0);
                }
            }
            else
            {
                this._historyIndex = 0;
            }
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
                if (this._inputBox.Text.StartsWith(this._context.Input.Prefix))
                {
                    this._context.Input.Command(Web.EscapeHtml(this._inputBox.Text));
                    this._inputBox.Text = "";
                    return;
                }
                this._context.Input.Send(this._target, Web.EscapeHtml(this._inputBox.Text), false);
                this._inputBox.Text = "";
            }
        }
    }
}
