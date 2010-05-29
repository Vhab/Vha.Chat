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
    public class InviteCommand : Command
    {
        public override bool Process(Context context, string command, string[] args)
        {
            if (!context.Input.CheckArguments(command, 1)) return false;
            if (!context.Input.CheckUser(args[0])) return false;
            context.Chat.SendPrivateChannelInvite(args[0]);
            context.Write(MessageClass.PG, "Inviting " + args[0] + " to your private channel");
            return true;
        }

        public InviteCommand()
            : base(
                "Private channel invite", // Name
                new string[] { "invite" }, // Triggers
                new string[] { "invite [username]" }, // Usage
                new string[] { "invite Vhab" }, // Examples
                // Description
                "The invite command allows you to invite a user into your private channel."
                )
        { }
    }
}
