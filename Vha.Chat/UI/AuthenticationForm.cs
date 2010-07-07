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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Vha.Net;
using Vha.Net.Events;
using Vha.Common;
using Vha.Chat;
using Vha.Chat.Events;

namespace Vha.Chat.UI
{
    public partial class AuthenticationForm : Form
    {
        protected StatusForm _status = null;
        protected Context _context = null;

        public AuthenticationForm(Context context)
        {
            this._context = context;
            InitializeComponent();
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            // Add dimensions
            foreach (Dimension dimension in this._context.Configuration.Dimensions)
            {
                this._server.Items.Add(dimension);
                if (this._server.Items.Count == 1)
                    this._server.SelectedIndex = 0; // Make sure we always select the first one, so that if we don't find the last used server, at least some server is selected.
                else if (dimension.Name == this._context.Options.LastDimension)
                    this._server.SelectedIndex = this._server.Items.Count - 1; // this works because count starts at 1 and index starts at 0.
            }

            // Add accounts (sorted alphabetically)
            this._account.Text = this._context.Options.LastAccount;
            List<string> accounts = new List<string>();
            foreach (OptionsAccount acc in this._context.Options.Accounts)
                accounts.Add(acc.Name);
            accounts.Sort();
            foreach (string acc in accounts)
                this._account.Items.Add(acc);

            // Hook events
            this._context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            this._context.SelectCharacterEvent += new Handler<SelectCharacterEventArgs>(_context_SelectCharacterEvent);
            this._context.ErrorEvent += new Handler<ErrorEventArgs>(_context_ErrorEvent);
        }

        private void AuthenticationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Unhook events
            this._context.StateEvent -= new Handler<StateEventArgs>(_context_StateEvent);
            this._context.SelectCharacterEvent -= new Handler<SelectCharacterEventArgs>(_context_SelectCharacterEvent);
            this._context.ErrorEvent -= new Handler<ErrorEventArgs>(_context_ErrorEvent);
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
            Dimension dimension = (Dimension)this._server.SelectedItem;
            this._context.Connect(dimension.Name, this._account.Text, this._password.Text);
            // Show dialog
            DialogResult result = this._status.ShowDialog();
            this._status = null;
            if (result == DialogResult.Abort)
                this._context.Disconnect();
        }

        #region Context event handlers
        private void _context_SelectCharacterEvent(Context context, SelectCharacterEventArgs args)
        {
            // Run this method locally
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new Handler<SelectCharacterEventArgs>(_context_SelectCharacterEvent),
                    new object[] { context, args });
                return;
            }
            // Early out
            if (args.Characters.Length == 0)
                return;
            if (this._status == null)
                return;
            // Hide current form
            this._status.DialogResult = DialogResult.OK;
            this._status.Hide();
            this._status = null;
            this.Hide();
            // Show character selection dialog
            SelectionForm form = new SelectionForm(context, args.Characters);
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Select character
                args.Character = form.Character;
            }
            else
            {
                // Return to authentication screen
                this.Show();
            }
        }

        private void _context_StateEvent(Context context, StateEventArgs args)
        {
            // Run this method locally
            if (this.InvokeRequired)
            {
                this.Invoke(
                    new Handler<StateEventArgs>(_context_StateEvent),
                    new object[] { context, args });
                return;
            }
            // Handle state change
            switch (args.State)
            {
                case ContextState.Connecting:
                    if (this._status != null)
                        this._status.SetMessage("Connecting...");
                    break;
                case ContextState.CharacterSelection:
                    // Hide status form
                    if (this._status != null)
                    {
                        this._status.DialogResult = DialogResult.OK;
                        this._status.Hide();
                    }
                    // Display main form
                    this.Hide();
                    Program.ApplicationContext.MainForm = new ChatForm(context);
                    this.Close();
                    Program.ApplicationContext.MainForm.Show();
                    break;
                case ContextState.Disconnected:
                    if (this._status == null) break;
                    if (this._status.Message == "Connecting...")
                        this._status.SetMessage("Failed to connect!");
                    break;
            }
        }

        private void _context_ErrorEvent(Context context, ErrorEventArgs args)
        {
            if (this._status != null)
                this._status.SetMessage(args.Message);
        }
        #endregion
    }
}