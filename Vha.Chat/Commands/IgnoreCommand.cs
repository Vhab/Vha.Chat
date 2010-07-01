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
using Vha.Common;

namespace Vha.Chat.Commands
{
    public class IgnoreCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckUser(args[0], true)) return false;
            IgnoreResult result = context.Ignores.Toggle(args[0]);
            string character = Format.UppercaseFirst(args[0]);
            switch (result)
            {
                case IgnoreResult.Added:
                    context.Write(MessageClass.Internal, character + " has been added to your ignore list");
                    break;
                case IgnoreResult.Removed:
                    context.Write(MessageClass.Internal, character + " has been removed from your ignore list");
                    break;
                default:
                    context.Write(MessageClass.Error, "An error has occured while attempting to ignore " + character);
                    break;
            }
            return true;
        }

        public IgnoreCommand()
            : base(
                "Ignore user", // Name
                new string[] { "ignore" }, // Triggers
                new string[] { "ignore [username]" }, // Usage
                new string[] { "ignore Vhab" }, // Examples
                // Description
                "The ignore command allows you to prevent users from sending messages to you.\n" +
                "Once a user has been put on ignore, (s)he no longer is able to contact you in any way through the chat server."
            )
        { }
    }
}
