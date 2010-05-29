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

namespace Vha.Chat
{
    public class Input
    {
        #region Input sanity commands
        public bool CheckArguments(string command, int count, bool output);

        public bool CheckConnection(bool output);

        public bool CheckOrganization(bool output);

        public bool CheckUser(string user, bool output);

        public bool CheckChannel(string channel, bool output);

        public bool CheckPrivateChannel(string channel, bool output);
        #endregion

        #region Commands for sending message or command input
        public void Send(MessageTarget target, string message, bool allowCommands);

        public void Command(string command);

        public bool Register(Command command);

        public void Unregister(string name);
        #endregion

        #region Internal
        internal Input(Context context)
        {
            this._context = context;
        }

        private Context _context;
        #endregion
    }
}
