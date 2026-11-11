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
    public partial class AboutForm : Form
    {
        private readonly Context _context;

        public AboutForm(Context context)
        {
            this._context = context;

            InitializeComponent();

            // Set the version
            this._version.Text = "Version " + Application.ProductVersion;

            // Apply dark colors without changing the original Light theme.
            bool dark = string.Equals(
                this._context.Options.Theme,
                "Dark",
                StringComparison.OrdinalIgnoreCase);

            if (dark)
                ThemeManager.ApplyControls(this, true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            bool dark = string.Equals(
                this._context.Options.Theme,
                "Dark",
                StringComparison.OrdinalIgnoreCase);

            ThemeManager.ApplyTitleBar(this, dark);
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this._license.Select(0, 0);
        }
    }
}