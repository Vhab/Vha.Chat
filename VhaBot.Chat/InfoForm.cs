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
    public partial class InfoForm : Form
    {
        protected string _html = "";
        protected ChatHtml _links;

        public InfoForm(ChatHtml links, string html)
        {
            InitializeComponent();
            this._html = html;
            this._links = links;
            this.Info.DocumentText = this._links.Template;
        }
        
        private void Info_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Scheme != "file")
                e.Cancel = true;
        }

        private void Info_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Set background color
            this.Info.Document.BackColor = this.BackColor;
            if (this.Info.Document.Body != null)
            {
                string color = this.ForeColor.R.ToString("X") + this.ForeColor.G.ToString("X") + this.ForeColor.B.ToString("X");
                this.Info.Document.Body.Style =
                    "color: #" + color + ";" +
                    "padding: 6px;";
            }
            // Put in some content
            this._links.AppendHtml(this.Info.Document, this._html);
        }
    }
}