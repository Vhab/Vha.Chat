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

namespace Vha.Chat
{
    public partial class InfoForm : Form
    {
        protected string _html = "";
        protected ChatHtml _links;

        public InfoForm(ChatHtml links, string html)
        {
            InitializeComponent();
            this._html = html;
            this._links = links;
        }
        
        private void _info_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Scheme != "file")
                e.Cancel = true;
        }

        private void _info_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this._info.Document.Write(this._links.Template);
            // Set background color
            this._info.Document.BackColor = this.BackColor;
            if (this._info.Document.Body != null)
            {
                string color = this.ForeColor.R.ToString("X") + this.ForeColor.G.ToString("X") + this.ForeColor.B.ToString("X");
                this._info.Document.Body.Style =
                    "color: #" + color + ";" +
                    "padding: 6px;";
            }
            // Put in some content
            this._links.AppendHtml(this._info.Document, this._html);
        }
    }
}