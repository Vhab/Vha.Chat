/*
* VhaBot.Chat
* Copyright (C) 2009 Remco van Oosterhout
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
using System.Threading;
using VhaBot.Net;
using VhaBot.Net.Events;
using VhaBot.Common;

namespace VhaBot.Chat
{
    public partial class AuthenticationForm : Form
    {
        protected StatusForm _status = null;
        protected Net.Chat _chat = null;

        public AuthenticationForm()
        {
            InitializeComponent();
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            // FIXME: Add a dimension file loader here
            this._server.Items.Add(new Server("Atlantean", "chat.d1.funcom.com", 7101));
            this._server.Items.Add(new Server("Rimor", "chat.d2.funcom.com", 7102));
            this._server.Items.Add(new Server("Die Neue Welt", "chat.d3.funcom.com", 7103));
            this._server.Items.Add(new Server("Test", "chat.dt.funcom.com", 7109));
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _login_Click(object sender, EventArgs e)
        {
            // Input verification
            if (this._server.SelectedItem == null)
            {
                MessageBox.Show("You're required to select a server before authenticating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this._account.Text.Trim().Length == 0)
            {
                MessageBox.Show("You're required to enter your account name before authenticating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this._password.Text.Trim().Length == 0)
            {
                MessageBox.Show("You're required to enter your password before authenticating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Create status dialog
            this._status = new StatusForm();
            this._status.SetMessage("Initializing...");
            // Connect
            Server server = (Server)this._server.SelectedItem;
            this._chat = new VhaBot.Net.Chat(server.Address, server.Port, this._account.Text, this._password.Text);
            this._chat.UseThreadPool = false;
            Thread thread = new Thread(new ThreadStart(_connect));
            thread.Start();
            // Show status and wait
            DialogResult result = this._status.ShowDialog();
            this._status = null;
            if (result == DialogResult.Abort)
            {
                this._chat.Disconnect();
                this._chat = null;
            }
        }

        private void _connect()
        {
            this._chat.AutoReconnect = false;
            this._chat.IgnoreAfkMessages = false;
            this._chat.IgnoreCharacterLoggedIn = true;
            this._chat.IgnoreOfflineMessages = false;
            this._chat.LoginCharlistEvent += new LoginCharlistEventHandler(_chat_LoginCharlistEvent);
            this._chat.LoginErrorEvent += new LoginErrorEventHandler(_chat_LoginErrorEvent);
            this._chat.StatusChangeEvent += new StatusChangeEventHandler(_chat_StatusChangeEvent);
            this._chat.ExceptionEvent += new ExceptionEventHandler(Program.UnhandledException);
            this._chat.Connect();
        }

        private void AuthenticationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_chat != null)
            {
                this._chat.LoginCharlistEvent -= new LoginCharlistEventHandler(_chat_LoginCharlistEvent);
                this._chat.LoginErrorEvent -= new LoginErrorEventHandler(_chat_LoginErrorEvent);
                this._chat.StatusChangeEvent -= new StatusChangeEventHandler(_chat_StatusChangeEvent);
            }
        }

        #region Chat Event Handlers
        private void _chat_StatusChangeEvent(VhaBot.Net.Chat chat, StatusChangeEventArgs e)
        {
            if (this._status == null)
                return;
            switch (e.State)
            {
                case ChatState.Connecting:
                    this._status.SetMessage("Connecting...");
                    break;
                case ChatState.Login:
                    this._status.SetMessage("Authenticating...");
                    break;
                case ChatState.CharacterSelect:
                    this._status.SetMessage("Selecting character...");
                    break;
            }
        }

        private void _chat_LoginErrorEvent(VhaBot.Net.Chat chat, LoginErrorEventArgs e)
        {
            if (this._status == null)
                return;
            this._status.SetMessage(e.Error);
        }

        private void _chat_LoginCharlistEvent(VhaBot.Net.Chat chat, LoginChararacterListEventArgs e)
        {
            // Run this method locally
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new LoginCharlistEventHandler(_chat_LoginCharlistEvent),
                    new object[] { chat, e });
                return;
            }
            // Early out
            if (this._status == null)
                return;
            if (e.CharacterList.Length == 0)
                return;
            this._status.DialogResult = DialogResult.OK;
            this._status.Hide();
            this.Hide();
            SelectionForm form = new SelectionForm(e.CharacterList);
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                Program.Context.MainForm = new ChatForm(this._chat);
                this.Close();
                Program.Context.MainForm.Show();
                this._chat.SendLoginCharacter(form.Character);
                this._chat.AutoReconnect = true;
            }
            else
            {
                this._chat.Disconnect();
                _chat = null;
                this.Show();
            }
            this._status = null;
        }
        #endregion
    }
}