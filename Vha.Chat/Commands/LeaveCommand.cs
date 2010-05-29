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
    public class LeaveCommand : Command
    {
        public override bool Process(Context context, string command, string[] args)
        {
            if (!context.Input.CheckArguments(command, 1)) return false;
            if (!context.Input.CheckPrivateChannel(args[0])) return false;
            context.Chat.SendPrivateChannelLeave(args[0]);
            return true;
        }

        public LeaveCommand()
            : base(
                "Leave private channel", // Name
                new string[] { "leave" }, // Triggers
                new string[] { "leave [channel]" }, // Usage
                new string[] { "leave Helpbot" }, // Examples
                // Description
                "The leave command allows you to leave a remote private channel."
                ) { }
    }
}
