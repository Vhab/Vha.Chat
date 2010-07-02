/*
* Vha.MDB
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
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
