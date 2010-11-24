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
using Vha.Common;

namespace Vha.Chat.Commands
{
    public class IgnoreCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            // Add ignore
            if (!context.Input.CheckCharacter(args[0], true)) return false;
            string character = Format.UppercaseFirst(args[0]);
            if (context.Ignores.Contains(character))
            {
                context.Write(MessageClass.Error, character + " already is on your ignore list");
                return false;
            }
            context.Ignores.Add(args[0]);
            context.Write(MessageClass.Internal, character + " has been added to your ignore list");
            return true;
        }

        public IgnoreCommand()
            : base(
                "Ignore character", // Name
                new string[] { "ignore" }, // Triggers
                new string[] { "ignore [character]" }, // Usage
                new string[] { "ignore Vhab" }, // Examples
                // Description
                "The ignore command allows you to prevent characters from sending messages to you.\n" +
                "Once a character has been put on ignore, (s)he is no longer able to contact you in any way through the chat server."
            )
        { }
    }
}
