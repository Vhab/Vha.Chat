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
    public class FriendCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 2, true)) return false;
            if (args[0].ToLower() == "add")
            {
                if (!context.Input.CheckUser(args[1], true)) return false;
                context.Chat.SendFriendAdd(args[1]);
                return true;
            }
            else if (args[0].ToLower() == "remove")
            {
                if (!context.Input.CheckUser(args[1], true)) return false;
                context.Chat.SendFriendRemove(args[1]);
                return true;
            }
            else
            {
                context.Write(MessageClass.Error, "Expecting either 'add' or 'remove' as first argument for this command");
                return false;
            }
        }

        public FriendCommand()
            : base(
                "Adding and removing friends", // Name
                new string[] { "friend" }, // Triggers
                new string[] { "friend add [username]", "friend remove [username]" }, // Usage
                new string[] { "friend add Vhab", "friend remove Helpbot" }, // Examples
                // Description
                "The friend commands allows you to add and remove users from your friendslist.\n" +
                "Adding users to your friendslist allows you to see whether they're currently online or offline."
            )
        { }
    }
}
