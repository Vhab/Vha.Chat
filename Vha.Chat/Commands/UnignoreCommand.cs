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
using Vha.Common;

namespace Vha.Chat.Commands
{
    public class UnignoreCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckCharacter(args[0], true)) return false;
            // Remove ignore
            string character = Format.UppercaseFirst(args[0]);
            if (!context.Ignores.Contains(character))
            {
                context.Write(MessageClass.Error, character + " is not on your ignore list");
                return false;
            }
            context.Ignores.Remove(args[0]);
            context.Write(MessageClass.Internal, character + " has been removed from your ignore list");
            return true;
        }

        public UnignoreCommand()
            : base(
                "Unignore character", // Name
                new string[] { "unignore" }, // Triggers
                new string[] { "unignore [character]" }, // Usage
                new string[] { "unignore Vhab" }, // Examples
                // Description
                "The unignore command allows you to undo the effects of the ignore command for a specific character."
            )
        { }
    }
}
