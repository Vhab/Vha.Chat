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
using System.IO;
using Vha.Net;
using Vha.Common;
using Vha.Chat;

namespace Vha.Chat.UI
{
    public partial class SelectionForm : Form
    {
        public LoginCharacter Character = null;
        /// <summary>
        /// Store chat library till later.
        /// </summary>
        private Net.Chat _chat=null;

        public SelectionForm(Net.Chat chat, LoginCharacter[] characters)
        {
            InitializeComponent();
            this._chat = chat; //Store chat libary so we can create ignore list *before* actually logging in.
            List<LoginCharacter> list = new List<LoginCharacter>(characters);
            list.Sort();
            foreach (LoginCharacter character in list)
            {
                this._characters.Items.Add(character);
            }

            this._characters.SelectedIndex = 0; // Default to selecting the first character
            if (Program.Configuration.Accounts.Count > 0)
            {
                foreach (ConfigAccount amap in Program.Configuration.Accounts)
                {
                    if (amap.Account == chat.Account)
                    {
                        foreach (LoginCharacter lc in this._characters.Items)
                        {
                            // Automatically select the last used character
                            if (lc.Name == amap.Character)
                            {
                                this._characters.SelectedIndex = this._characters.Items.IndexOf(lc);
                            }
                        }
                    }
                }
            }
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void _select_Click(object sender, EventArgs e)
        {
            if (this._characters.SelectedItem == null)
            {
                MessageBox.Show("You're required to select a character before logging on.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Character = (LoginCharacter)this._characters.SelectedItem;
            this.DialogResult = DialogResult.OK;
           
            // Create ignore list. Doing so here lets us ignore offline tells and system messages reporting them,
            // since we haven't *really* selected a character yet.. but we know which one we are selecting.
            if (true)
            {
                //Create a new scope to not overflow this methods scope with junk members.
                string dir = "ignore";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                Dimension s = Program.Servers.Get(this._chat.Server, this._chat.Port);
                string dim;
                if (s == null) dim = "unknown";
                else dim = s.Name;
                if (Program.Configuration.IgnoreMethod != IgnoreMethod.None)
                    Program.Ignores = new Ignore(string.Format("{0}/{1}.xml", dir, dim), Program.Configuration.IgnoreMethod, _chat.Account, this.Character.ID);
            }
        }
    }
}