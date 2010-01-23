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

namespace VhaBot.Chat
{
    public partial class StatusForm : Form
    {
        public delegate void SetMessageDelegate(string message);

        public StatusForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
        }

        public void SetMessage(string message)
        {
            if (this._message.InvokeRequired)
            {
                this._message.Invoke(new SetMessageDelegate(SetMessage), new object[] { message });
                return;
            }
            this._message.Text = message;
        }
    }
}