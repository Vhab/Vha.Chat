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

namespace Vha.Chat.Commands
{
    public class HelpCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            // Command specific help
            if (args.Length > 0)
            {
                // Check if the trigger exists
                string trig = args[0].ToLower();
                Command command = context.Input.GetCommandByTrigger(trig);
                if (command == null)
                {
                    context.Write(MessageClass.Internal, string.Format(
                        "Unknown command: {0}. Use '{1}help' for more information",
                        trig, context.Input.Prefix));
                    return false;
                }
                // Display help
                string help = string.Format(
                    "{1}\n" +
                    "Usage:\n- {0}{2}\n" +
                    "Examples:\n- {0}{3}",
                    context.Input.Prefix, command.Description,
                    string.Join("\n- " + context.Input.Prefix, command.Usage),
                    string.Join("\n- " + context.Input.Prefix, command.Examples)
                    );
                context.Write(MessageClass.Internal, help);
                return true;
            }
            // General help
            context.Write(
                MessageClass.Internal,
                string.Format(
                    "The following commands are available: {1}\n" +
                    "Use '{0}tell [command]' for more information",
                    context.Input.Prefix,
                    string.Join(", ", context.Input.GetTriggers())
                ));
            return true;
        }

        public HelpCommand()
            : base(
                "Help and documentation", // Name
                new string[] { "help" }, // Triggers
                new string[] { "help", "help [command]" }, // Usage
                new string[] { "help", "help tell" }, // Examples
                // Description
                "The help command provides you with a list of all available commands or information about a specific command."
            )
        { }
    }
}
