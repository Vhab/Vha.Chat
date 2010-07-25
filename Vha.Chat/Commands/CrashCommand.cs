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
using System.Text;
using System.Windows.Forms;

namespace Vha.Chat.Commands
{
    public class CrashCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            DialogResult result = MessageBox.Show(
                "This command will intentionally crash the client.\nAre you sure you wish to continue?",
                "Crash command", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (result != DialogResult.OK) return false;
            throw new Exception("/crash command executed");
        }

        public CrashCommand()
            : base(
                "Crash the client", // Name
                new string[] { "crash" }, // Triggers
                new string[] { "crash" }, // Usage
                new string[] { "crash" }, // Examples
                // Description
                "This command will intentionally crash the client for debugging purposes."
            )
        { }
    }
}
