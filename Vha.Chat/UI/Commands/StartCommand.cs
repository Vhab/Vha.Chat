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
using System.Windows.Forms;
using System.Diagnostics;
using Vha.Chat.Commands;
using Vha.Common;

namespace Vha.Chat.UI.Commands
{
    public class StartCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!message.StartsWith("http://") && !message.StartsWith("https://"))
            {
                context.Write(MessageClass.Error, "The start command only supports urls starting with http:// or https://");
                return false;
            }
            if (context.Platform == Platform.Mono)
            {
                context.Write(MessageClass.Error, "This command is not yet available on Mono. We apologize for the inconvenience.");
                return false;
            }
            // Open in default browser
            System.Diagnostics.Process.Start(message);
            return true;
        }

        public StartCommand()
            : base(
                "Open web link", // Name
                new string[] { "start" }, // Triggers
                new string[] { "start [url]" }, // Usage
                new string[] { "start http://www.vhabot.net" }, // Examples
                // Description
                "The start command allows you to open web pages in your default browser."
            ) { }
    }
}
