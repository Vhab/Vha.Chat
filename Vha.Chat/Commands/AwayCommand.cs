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
using System.Collections.Generic;
using Vha.Chat.Events;

namespace Vha.Chat.Commands
{
    public class AwayCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            message = message.Trim();
            // Enable afk
            if (this._away == false)
            {
                this._away = true;
                this._message = message;
                this._time = DateTime.Now;
                context.Write(MessageClass.Internal, "You are now afk");
                return true;
            }
            // Update afk
            if (message.Length > 0)
            {
                this._message = message;
                context.Write(MessageClass.Internal, "Your afk message has been updated");
                lock (this._users)
                    this._users.Clear();
                return true;
            }
            // Disable afk
            this._away = false;
            this._message = "";
            lock (this._users)
                this._users.Clear();
            context.Write(MessageClass.Internal, "You are no longer afk");
            return true;
        }

        public AwayCommand(Context context)
            : base(
                "Away from keyboard", // Name
                new string[] { "afk", "away" }, // Triggers
                new string[] { "afk [message]", "afk" }, // Usage
                new string[] { "afk Raiding RK4, back in 30 minutes", "afk" }, // Examples
                // Description
                "The afk command allows you to mark yourself as away and automatically reply this to incoming private messages.\n" +
                "If you use the command again without arguments and you're already marked as away, your away status will be removed.\n" +
                "Using the command with an argument will simply change your away message if you're already marked as away."
            )
        {
            context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            context.MessageEvent += new Handler<MessageEventArgs>(_context_MessageEvent);
        }

        private void _context_StateEvent(Context context, StateEventArgs args)
        {
            // Reset away status
            if (args.State == ContextState.Disconnected)
            {
                this._away = false;
                this._message = "";
                lock (this._users)
                    this._users.Clear();
            }
        }

        private void _context_MessageEvent(Context context, MessageEventArgs args)
        {
            // Only care of PM's
            if (args.Source == null || args.Source.Type != MessageType.Character)
                return;
            if (string.IsNullOrEmpty(args.Source.Character))
                return;
            // If we're not away, don't care!
            if (this._away == false) return;
            // Let's do this!
            string c = args.Source.Character;
            lock (this._users)
            {
                // Check if we already sent a message to this user recently
                if (!this._users.ContainsKey(c))
                    this._users.Add(c, DateTime.Now);
                else if ((DateTime.Now - this._users[c]).TotalMinutes < 5)
                    return;
                // Update time and create afk message
                this._users[c] = DateTime.Now;
                TimeSpan duration = DateTime.Now - this._time;
                string reply = string.Format(
                    "{0} is AFK (Away from keyboard) since {1} hours and {2} minutes ago.",
                    context.Character, (int)duration.TotalHours, duration.Minutes);
                if (!string.IsNullOrEmpty(this._message))
                    reply += "\n" + this._message;
                // Reply
                context.Input.Send(args.Source.GetTarget(), reply, false);
            }
        }

        private bool _away = false;
        private string _message = "";
        private DateTime _time;
        private Dictionary<string, DateTime> _users = new Dictionary<string, DateTime>();
    }
}
