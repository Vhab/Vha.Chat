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
    public class ColorsCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            context.Write(MessageClass.None, "This is an unclassified message");
            context.Write(MessageClass.Error, "This is an error message");
            context.Write(MessageClass.Internal, "This is an internal message");
            context.Write(MessageClass.Text, "This is an echo/text message");
            context.Write(MessageClass.PrivateMessage, "This is a private message");
            context.Write(MessageClass.PrivateChannel, "This is a private channel message");
            context.Write(MessageClass.AnnouncementsChannel, "This is an announcements channel message");
            context.Write(MessageClass.GeneralChannel, "This is a general channel message");
            context.Write(MessageClass.LeadersChannel, "This is a leaders channel message");
            context.Write(MessageClass.OrganizationChannel, "This is an organization channel message");
            context.Write(MessageClass.ShoppingChannel, "This is a shopping channel message");
            context.Write(MessageClass.BroadcastMessage, "This is a broadcast message");
            return true;
        }

        public ColorsCommand()
            : base(
                "Colors test", // Name
                new string[] { "colors" }, // Triggers
                new string[] { "colors" }, // Usage
                new string[] { "colors" }, // Examples
                // Description
                "The colors command allows you to quickly test all used colors for text output."
            )
        { }
    }
}
