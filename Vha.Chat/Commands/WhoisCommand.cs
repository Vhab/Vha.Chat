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
using System.Text;

namespace Vha.Chat.Commands
{
    public class WhoisCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckCharacter(args[0], true)) return false;
            if (!context.Input.CheckCharacter("Helpbot", false))
            {
                context.Write(MessageClass.Error, "Helpbot is not available on this dimension");
                return false;
            }
            context.Input.Send(new MessageTarget(MessageType.Character, "Helpbot"), "whois " + args[0], false);
            return true;
        }

        public WhoisCommand()
            : base(
                "Character information", // Name
                new string[] { "whois" }, // Triggers
                new string[] { "whois [character]" }, // Usage
                new string[] { "whois Vhab" }, // Examples
                // Description
                "The whois command allows you to obtain information like profession, level and organization for a specific character.\n" +
                "This command requires Helpbot to be available and online on your dimension."
            )
        { }
    }
}
