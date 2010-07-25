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

namespace Vha.Chat.Commands
{
    public class MuteCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 1, true)) return false;
            if (!context.Input.CheckChannel(message, true)) return false;
            context.Chat.SendChannelMute(message, true);
            return true;
        }

        public MuteCommand()
            : base(
                "Mute public channel", // Name
                new string[] { "mute" }, // Triggers
                new string[] { "mute [channel]" }, // Usage
                new string[] { "mute Neutral OOC" }, // Examples
                // Description
                "The mute command allows you to mute messages from a certain public channel."
            )
        { }
    }
}
