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
using System.Collections.Generic;
using System.Text;
using Vha.Chat.Commands;
using Vha.Common;

namespace Vha.Chat
{
    public class Input
    {
        public string Prefix { get { return this._prefix; } }

        #region Input sanity commands
        public bool CheckArguments(string command, int count, int minimum, bool output)
        {
            if (count >= minimum) return true;
            if (output)
            {
                string message = "";
                Command c = this.GetCommand(command);
                if (c == null)
                {
                    // Generic response
                    this._context.Write(MessageClass.Error, "Expecting at least " + count + " arguments");
                }
                else
                {
                    // Command specific response
                    message = "Expected usage: ";
                    foreach (string usage in c.Usage)
                        message += "\n" + this.Prefix + usage;
                    this._context.Write(MessageClass.Error, message);
                }
                message = string.Format("Use '{0}help {1}' for more information", this.Prefix, command);
                this._context.Write(MessageClass.Error, message);
            }
            return false;
        }

        public bool CheckConnection(bool output)
        {
            if (this._context.State == ContextState.Connected) return true;
            if (output)
                this._context.Write(MessageClass.Error, "Not connected");
            return false;
        }

        public bool CheckOrganization(bool output)
        {
            if (this._context.HasOrganization) return true;
            if (output)
                this._context.Write(MessageClass.Error, "Not a member of an organization");
            return false;
        }

        public bool CheckUser(string user, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.Chat.GetUserID(user) != 0) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown user: " + Format.UppercaseFirst(user));
            }
            return false;
        }

        public bool CheckChannel(string channel, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.HasChannel(channel)) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown channel: " + channel);
            }
            return false;
        }

        public bool CheckPrivateChannel(string channel, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.HasPrivateChannel(channel)) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown private channel: " + channel);
            }
            return false;
        }
        #endregion

        #region Commands for sending message or command input
        public void Send(MessageTarget target, string message, bool allowCommands);

        public void Command(string command);

        public bool RegisterCommand(Command command);

        public void UnregisterCommand(string name);

        public bool HasCommand(string command);

        public Command GetCommand(string command);
        #endregion

        #region Internal
        internal Input(Context context, string prefix)
        {
            this._context = context;
            this._prefix = prefix;
        }

        private Context _context;
        private string _prefix;
        #endregion
    }
}
