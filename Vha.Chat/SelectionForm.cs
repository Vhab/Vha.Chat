/*
* Vha.Chat
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
using Vha.Net;
using Vha.Common;

namespace Vha.Chat
{
    public partial class SelectionForm : Form
    {
        public LoginCharacter Character = null;

        public SelectionForm(LoginCharacter[] characters)
        {
            InitializeComponent();
            foreach (LoginCharacter character in characters)
            {
                this._characters.Items.Add(character);
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
        }
    }
}