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
    public partial class IgnoresForm : BaseForm
    {
        public IgnoresForm(Context context)
            : base(context, "Ignores")
        {
            InitializeComponent();
            base.Initialize();

            this._context = context;
            // Hook events
            this._context.Ignores.AddedEvent += new Handler<IgnoreEventArgs>(_ignores_AddedEvent);
            this._context.Ignores.RemovedEvent += new Handler<IgnoreEventArgs>(_ignores_RemovedEvent);
            this._context.Ignores.ReloadedEvent += new Handler(_ignores_ReloadedEvent);
            this._context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            // Populate form
            this._reloadList();
            this._updateState();
        }

        #region Internal
        private Context _context;

        private void _reloadList()
        {
            List<string> characters = new List<string>(this._context.Ignores.GetCharacters());
            // Add new characters
            foreach (string character in characters)
            {
                if (this._characters.Items.Contains(character)) continue;
                // Insert-sort
                int index = 0;
                foreach (string c in this._characters.Items)
                {
                    if (c.CompareTo(character) > 0) break;
                    index++;
                }
                this._characters.Items.Insert(index, character);
            }
            // Remove characters
            Stack<string> toDelete = new Stack<string>();
            foreach (string character in this._characters.Items)
            {
                if (characters.Contains(character)) continue;
                toDelete.Push(character);
            }
            while (toDelete.Count > 0)
            {
                this._characters.Items.Remove(toDelete.Pop());
            }
        }

        private void _updateState()
        {
            bool enabled = this._context.State == ContextState.Connected;
            this._add.Enabled = enabled;
            this._remove.Enabled = enabled;
            this._selectAll.Enabled = enabled;
            this._selectNone.Enabled = enabled;
            this._character.Enabled = enabled;
            this._characters.Enabled = enabled;
        }

        private void _ignores_ReloadedEvent(Context context)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler(_ignores_ReloadedEvent),
                    new object[] { context });
                return;
            }
            this._reloadList();
        }

        private void _ignores_RemovedEvent(Context context, IgnoreEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<IgnoreEventArgs>(_ignores_RemovedEvent),
                    new object[] { context, args });
                return;
            }
            this._reloadList();
        }

        private void _ignores_AddedEvent(Context context, IgnoreEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<IgnoreEventArgs>(_ignores_AddedEvent),
                    new object[] { context, args });
                return;
            }
            this._reloadList();
        }

        private void _context_StateEvent(Context context, StateEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<StateEventArgs>(_context_StateEvent),
                    new object[] { context, args });
                return;
            }
            this._updateState();
        }

        private void _add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this._character.Text)) return;
            if (!this._context.Input.CheckCharacter(this._character.Text, false))
            {
                MessageBox.Show(
                    "Unknown character: " + Format.UppercaseFirst(this._character.Text),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this._context.Ignores.Contains(this._character.Text))
            {
                MessageBox.Show(
                    Format.UppercaseFirst(this._character.Text)+" already is on your ignore list",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this._context.Ignores.Add(this._character.Text);
            this._character.Text = "";
        }

        private void _selectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this._characters.Items.Count; i++)
                this._characters.SetItemChecked(i, true);
        }

        private void _selectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this._characters.Items.Count; i++)
                this._characters.SetItemChecked(i, false);
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            if (this._characters.CheckedItems.Count == 0)
            {
                MessageBox.Show(
                    "You must select 1 or more characters in order to remove them",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            while (this._characters.CheckedItems.Count > 0)
            {
                string character = this._characters.CheckedItems[0].ToString();
                this._characters.Items.Remove(character);
                this._context.Ignores.Remove(character);
            }
        }
        #endregion
    }
}
