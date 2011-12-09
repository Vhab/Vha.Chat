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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Vha.Chat;

namespace Vha.Chat.UI
{
    public partial class OptionsForm : Form
    {
        private Context _context;

        public OptionsForm(Context context)
        {
            InitializeComponent();

            this._context = context;
            this._context.Options.SavedEvent += new Handler<Options>(_context_SavedEvent);

            // Fill comboboxes
            this._textStyle.Items.Add(TextStyle.Default);
            this._textStyle.Items.Add(TextStyle.Strip);
            this._textStyle.Items.Add(TextStyle.Invert);
            this._textStyle.SelectedIndex = 0;
            this._panelPosition.Items.Add(HorizontalPosition.Left);
            this._panelPosition.Items.Add(HorizontalPosition.Right);
            this._panelPosition.SelectedIndex = 0;
            this._ignoreMethod.Items.Add(IgnoreMethod.None);
            this._ignoreMethod.Items.Add(IgnoreMethod.Character);
            this._ignoreMethod.Items.Add(IgnoreMethod.Account);
            this._ignoreMethod.Items.Add(IgnoreMethod.Dimension);
            this._ignoreMethod.SelectedIndex = 0;
            this._proxyType.Items.Add(ProxyType.Disabled);
            this._proxyType.Items.Add(ProxyType.HTTP);
            this._proxyType.Items.Add(ProxyType.Socks4);
            this._proxyType.Items.Add(ProxyType.Socks4a);
            this._proxyType.Items.Add(ProxyType.Socks5);
            this._proxyType.SelectedIndex = 0;
            this._infoWindowBehavior.Items.Add(InfoWindowBehavior.OpenNew);
            this._infoWindowBehavior.Items.Add(InfoWindowBehavior.UseExisting);
            this._infoWindowBehavior.SelectedIndex = 0;

            // Manually trigger this to get in sync
            this._readOptions(this._context.Options);
        }

        private void OptionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._context.Options.SavedEvent -= new Handler<Options>(_context_SavedEvent);
        }

        private void _proxyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._updateProxy();
        }

        private void _save_Click(object sender, EventArgs e)
        {
            if (this._saveOptions(this._context.Options))
                this.Close();
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _reset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "This will reset all options back to their default values. This will include window positions and last used accounts.\n" +
                "Are you sure you wish to continue?", "Reset options", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (result == DialogResult.OK)
                this._context.Options.Reset();
        }

        private void _context_SavedEvent(Context context, Options args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<Options>(_context_SavedEvent),
                    new object[] { context, args });
                return;
            }
            this._readOptions(args);
        }

        private void _readOptions(Options o)
        {
            // Sync options with visuals
            this._chatTextSize.Value = o.ChatTextSize;
            this._infoWindowTextSize.Value = o.InfoWindowTextSize;
            this._textStyle.SelectedItem = o.TextStyle;
            this._maximumHistory.Value = o.MaximumHistory;
            this._maximumMessages.Value = o.MaximumMessages;
            this._maximumTexts.Value = o.MaximumTexts;
            this._panelPosition.SelectedItem = o.PanelPosition;
            this._ignoreMethod.SelectedItem = o.IgnoreMethod;
            this._infoWindowBehavior.SelectedItem = o.InfoWindowBehavior;
            // Proxy
            this._proxyType.SelectedItem = o.Proxy.Type;
            this._proxyAddress.Text = o.Proxy.Address;
            this._proxyPort.Value = o.Proxy.Port;
            this._proxyUsername.Text = o.Proxy.Username;
            this._proxyPassword.Text = o.Proxy.Password;
            // Update 'the view'
            _updateProxy();
            // Misc
            this._donateVisible.Checked = o.DonateVisible;
        }

        private bool _saveOptions(Options o)
        {
            // Ensure the proxy data is good
            if (!_checkProxy()) return false;
            // Options
            o.ChatTextSize = (int)this._chatTextSize.Value;
            o.InfoWindowTextSize = (int)this._infoWindowTextSize.Value;
            o.TextStyle = (TextStyle)this._textStyle.SelectedItem;
            o.MaximumHistory = (int)this._maximumHistory.Value;
            o.MaximumMessages = (int)this._maximumMessages.Value;
            o.MaximumTexts = (int)this._maximumTexts.Value;
            o.PanelPosition = (HorizontalPosition)this._panelPosition.SelectedItem;
            o.IgnoreMethod = (IgnoreMethod)this._ignoreMethod.SelectedItem;
            o.InfoWindowBehavior = (InfoWindowBehavior)this._infoWindowBehavior.SelectedItem;
            // Proxy
            o.Proxy.Type = (ProxyType)this._proxyType.SelectedItem;
            o.Proxy.Address = this._proxyAddress.Text;
            o.Proxy.Port = (int)this._proxyPort.Value;
            o.Proxy.Username = this._proxyUsername.Text;
            o.Proxy.Password = this._proxyPassword.Text;
            // Misc
            o.DonateVisible = this._donateVisible.Checked;
            // Save
            o.Save();
            return true;
        }

        private void _updateProxy()
        {
            switch ((ProxyType)this._proxyType.SelectedItem)
            {
                case ProxyType.HTTP:
                    this._proxyAddress.Enabled = true;
                    this._proxyPort.Enabled = true;
                    this._proxyUsername.Enabled = false;
                    this._proxyPassword.Enabled = false;
                    break;
                case ProxyType.Socks4:
                    this._proxyAddress.Enabled = true;
                    this._proxyPort.Enabled = true;
                    this._proxyUsername.Enabled = true;
                    this._proxyPassword.Enabled = false;
                    break;
                case ProxyType.Socks4a:
                    this._proxyAddress.Enabled = true;
                    this._proxyPort.Enabled = true;
                    this._proxyUsername.Enabled = true;
                    this._proxyPassword.Enabled = false;
                    break;
                case ProxyType.Socks5:
                    this._proxyAddress.Enabled = true;
                    this._proxyPort.Enabled = true;
                    this._proxyUsername.Enabled = true;
                    this._proxyPassword.Enabled = true;
                    break;
                default:
                    this._proxyAddress.Enabled = false;
                    this._proxyPort.Enabled = false;
                    this._proxyUsername.Enabled = false;
                    this._proxyPassword.Enabled = false;
                    break;
            }
        }

        private bool _checkProxy()
        {
            if ((ProxyType)this._proxyType.SelectedItem == ProxyType.Disabled)
            {
                this._proxyAddress.Text = "";
                this._proxyPort.Value = 8080;
                this._proxyUsername.Text = "";
                this._proxyPassword.Text = "";
                return true;
            }
            // Check address and port
            this._proxyAddress.Text = this._proxyAddress.Text.Trim();
            if (string.IsNullOrEmpty(this._proxyAddress.Text))
            {
                MessageBox.Show("Invalid proxy server address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (this._proxyPort.Value <= 0)
            {
                MessageBox.Show("Invalid proxy server port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Check username/password
            this._proxyUsername.Text = this._proxyUsername.Text.Trim();
            bool username = !string.IsNullOrEmpty(this._proxyUsername.Text);
            this._proxyPassword.Text = this._proxyPassword.Text.Trim();
            bool password = !string.IsNullOrEmpty(this._proxyPassword.Text);
            switch ((ProxyType)this._proxyType.SelectedItem)
            {
                case ProxyType.HTTP:
                    // Doesn't support username or passwords
                    this._proxyUsername.Text = "";
                    this._proxyPassword.Text = "";
                    return true;
                case ProxyType.Socks4:
                    // Doesn't support passwords
                    this._proxyPassword.Text = "";
                    return true;
                case ProxyType.Socks4a:
                    // Doesn't support passwords
                    this._proxyPassword.Text = "";
                    return true;
                case ProxyType.Socks5:
                    // Doesn't support only a password
                    if (password && !username)
                    {
                        MessageBox.Show("You can't use a proxy password without a proxy username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    return true;
            }
            return true;
        }
    }
}