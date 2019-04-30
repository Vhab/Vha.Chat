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
using System.Text;

namespace Vha.Chat.Commands
{
    public class TellCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckCharacter(args[0], true)) return false;
            // Shortcut to '/open character [character]'
            if (args.Length == 1 && context.Input.HasTrigger("open"))
            {
                context.Input.Command("open character " + args[0]);
                return true;
            }
            // Default behavior
            if (!context.Input.CheckArguments(trigger, args.Length, 2, true)) return false;
            string m = string.Join(" ", args, 1, args.Length - 1);
            context.Input.Send(new MessageTarget(MessageType.Character, args[0]), m, false);
            return true;
        }

        public TellCommand()
            : base(
                "Private message", // Name
                new string[] { "tell", "t" }, // Triggers
                new string[] { "tell [character] [message]" }, // Usage
                new string[] { "tell Vhab hey there, how are you doing?" }, // Examples
                // Description
                "The tell command allows you to send a private message to another character.\n" +
                "This message can only be seen by you and the recipient."
            )
        { }
    }
}
