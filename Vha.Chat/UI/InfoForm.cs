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
using Vha.Chat.UI.Controls;
using Vha.AOML.DOM;

namespace Vha.Chat.UI
{
    public partial class InfoForm : BaseForm
    {
        protected Form _parent = null;
        protected Context _context = null;

        public InfoForm(Context context, Form parent, Element element)
            : base (context, "Info")
        {
            InitializeComponent();
            base.Initialize();

            this._context = context;
            this._parent = parent;
            this._info.Context = context;
            this._info.BackgroundColor = this.BackColor;
            this._info.ForegroundColor = this.ForeColor;
            this._info.MaximumLines = 0;
            this._info.Write("{0}", element, TextStyle.Default, false);
            this._info.Padding = new Padding(7);
        }
    }
}