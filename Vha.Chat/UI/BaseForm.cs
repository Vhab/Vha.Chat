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
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Vha.Chat.UI
{
    public class BaseForm : Form
    {
        private string _name;
        private Context _context;

        /// <summary>
        /// DO NOT USE. THIS CONSTRUCTOR EXISTS TO SATISFY THE FORM DESIGNER
        /// </summary>
        protected BaseForm()
        {
            this._context = null;
            this._name = null;
        }

        protected BaseForm(Context context, string name)
        {
            this._context = context;
            this._name = name;
        }

        protected void Initialize()
        {
            this.Load += new EventHandler(BaseForm_Load);
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            OptionsWindow window = this._context.Options.GetWindow(this._name, false);
            if (window != null)
            {
                // Apply default values
                this.StartPosition = FormStartPosition.Manual;
                if (window.Width < this.MinimumSize.Width)
                    window.Width = this.MinimumSize.Width;
                if (window.Height < this.MinimumSize.Height)
                    window.Height = this.MinimumSize.Height;
                this.Size = new Size(window.Width, window.Height);
                this.Location = new Point(window.X, window.Y);
                if (window.Maximized && this.MaximizeBox)
                {
                    // Maximize the screen
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    // Offset the next window
                    window.X += 15;
                    window.Y += 15;
                    window.Maximized = false;
                }
            }
            else
            {
                // Store default values
                window = this._context.Options.GetWindow(this._name, true);
                window.Width = this.Width;
                window.Height = this.Height;
                window.X = this.Location.X;
                window.Y = this.Location.Y;
            }
            // Hook events
            this.Move += new EventHandler(BaseForm_Move);
            this.Resize += new EventHandler(BaseForm_Resize);
            this.FormClosed += new FormClosedEventHandler(BaseForm_FormClosed);
        }

        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OptionsWindow window = this._context.Options.GetWindow(this._name, false);
            if (window == null) return;
            if (this.WindowState == FormWindowState.Normal)
            {
                window.Width = this.Width;
                window.Height = this.Height;
                window.X = this.Location.X;
                window.Y = this.Location.Y;
                window.Maximized = false;
            }
            if (this.WindowState == FormWindowState.Maximized)
            {
                window.Maximized = true;
            }
            this._context.Options.Save();
        }

        private void BaseForm_Resize(object sender, EventArgs e)
        {
            OptionsWindow window = this._context.Options.GetWindow(this._name, false);
            if (window == null) return;
            if (this.WindowState == FormWindowState.Maximized)
            {
                // Store maximized state
                window.Maximized = true;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                // Store position
                window.Width = this.Width;
                window.Height = this.Height;
                window.Maximized = false;
            }
        }

        private void BaseForm_Move(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Normal) return;
            OptionsWindow window = this._context.Options.GetWindow(this._name, false);
            if (window == null) return;
            window.X = this.Location.X + 15;
            window.Y = this.Location.Y + 15;
        }
    }
}
