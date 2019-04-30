/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
    public enum BrowserFormType
    {
        Url,
        Item,
        Entity
    }

    public partial class BrowserForm : BaseForm
    {
        public BrowserForm(Context context, string argument, BrowserFormType type)
            : base(context, "Browser")
        {
            InitializeComponent();
            base.Initialize();
            // Handle argument
            switch (type)
            {
                case BrowserFormType.Entity:
                    string[] entityParts = argument.Split(new char[] { '/' });
                    if (entityParts.Length < 2)
                    {
                        this._browser.DocumentText = "Invalid entity: " + argument;
                        return;
                    }
                    else
                    {
                        string url = "http://aoitems.com/item/{0}";
                        this._browser.Navigate(string.Format(url, entityParts[1]));
                    }
                    break;
                case BrowserFormType.Item:
                    string[] itemParts = argument.Split(new char[] { '/' });
                    if (itemParts.Length < 3)
                    {
                        this._browser.DocumentText = "Invalid item: " + argument;
                        return;
                    }
                    else
                    {
                        string url = "http://aoitems.com/item/{0}/{1}";
                        this._browser.Navigate(string.Format(url, itemParts[0], itemParts[2]));
                    }
                    break;
                case BrowserFormType.Url:
                    this._browser.Navigate(argument);
                    break;
            }
        }
    }
}