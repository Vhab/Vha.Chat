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
    public class KickCommand : Command
    {
        public override bool Process(Context context, string command, string[] args)
        {
            if (!context.Input.CheckArguments(command, 1)) return false;
            if (!context.Input.CheckUser(args[0])) return false;
            context.Chat.SendPrivateChannelKick(args[0]);
            context.Write(MessageClass.PG, "Kicking " + args[0] + " from your private channel");
            return true;
        }

        public KickCommand()
            : base(
                "Private channel kick", // Name
                new string[] { "kick" }, // Triggers
                new string[] { "kick [user]" }, // Usage
                new string[] { "kick Vhab" }, // Examples
                // Description
                "The kick command allows you to kick a user from your private channel."
                )
        { }
    }
}
