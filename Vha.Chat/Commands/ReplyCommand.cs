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
using System.Collections.Generic;
using Vha.Chat.Events;

namespace Vha.Chat.Commands
{
    public class ReplyCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (string.IsNullOrEmpty(this._character))
            {
                context.Write(MessageClass.Error, "You can't send a reply if you haven't received any private messages yet");
                return false;
            }
            if (args.Length > 0)
            {
                // Reply
                context.Input.Send(
                    new MessageTarget(MessageType.Character, this._character),
                    message, false);
                return true;
            }
            else
            {
                // Emulate zero-argument tell
                context.Input.Command("tell " + this._character);
                return true;
            }
        }

        public ReplyCommand(Context context)
            : base(
                "Reply message", // Name
                new string[] { "reply", "r" }, // Triggers
                new string[] { "reply [message]" }, // Usage
                new string[] { "reply I'm fine. What's up?" }, // Examples
                // Description
                "The reply command allows you to send a private message to the last character who sent you a private message."
            )
        {
            context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            context.MessageEvent += new Handler<MessageEventArgs>(_context_MessageEvent);
        }

        private void _context_StateEvent(Context context, StateEventArgs args)
        {
            if (args.State != ContextState.Disconnected) return;
            this._character = "";
        }

        private void _context_MessageEvent(Context context, MessageEventArgs args)
        {
            // Only care of PM's
            if (args.Source == null || args.Source.Type != MessageType.Character)
                return;
            if (string.IsNullOrEmpty(args.Source.Character))
                return;
            this._character = args.Source.Character;
        }

        private string _character = "";
    }
}
