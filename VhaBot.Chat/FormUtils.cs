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
using System.Text;
using System.Windows.Forms;

namespace VhaBot.Chat
{
    public static class FormUtils
    {
        public delegate void FormDelegate(Form form);
        public static void InvokeHide(Form form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new FormDelegate(InvokeHide), form);
                return;
            }
            form.Visible = false;
        }

        public static void InvokeShow(Form form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new FormDelegate(InvokeShow), form);
                return;
            }
            form.Visible = true;
        }

        public static void InvokeClose(Form form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new FormDelegate(InvokeClose), form);
                return;
            }
            form.Close();
        }
    }
}
