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
    public class OrganizationCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckOrganization(true)) return false;
            context.Input.Send(new MessageTarget(MessageType.Channel, context.Organization), message, false);
            return true;
        }

        public OrganizationCommand()
            : base(
                "Organization message", // Name
                new string[] { "organization", "org", "o" }, // Triggers
                new string[] { "organization [message]" }, // Usage
                new string[] { "organization Hey everyone!", "o Hey everyone!" }, // Examples
                // Description
                "The organization command allows you to send a message to your organization's private channel.\n" +
                "This message can only be seen by you and your fellow organization members."
            )
        { }
    }
}
