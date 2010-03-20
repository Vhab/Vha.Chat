/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
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
using System.Collections.Generic;
using System.Text;
using Vha.Common;

namespace Vha.Net.Events
{
    /// <summary>
    /// Class for holding event args for system messages.
    /// </summary>
    public class SystemMessageEventArgs : EventArgs
    {
        private readonly UInt32 _clientid = 0;
        private readonly UInt32 _windowid = 0;
        private readonly UInt32 _messageid = 0;
        private readonly string[] _arguments = new string[0];
        private readonly SystemMessageType _type;
        private readonly string _notice;

        public SystemMessageEventArgs(UInt32 clientID, UInt32 windowID, UInt32 messageID, string[] arguments, string notice)
        {
            this._clientid = clientID;
            this._windowid = windowID;
            this._messageid = messageID;
            this._arguments = arguments;
            if (Enum.IsDefined(typeof(SystemMessageType), (int)messageID))
                this._type = (SystemMessageType)messageID;
            else this._type = SystemMessageType.Other;
            this._notice = notice;
        }

        public UInt32 ClientID { get { return this._clientid; } }
        public UInt32 WindowID { get { return this._windowid; } }
        public UInt32 MessageID { get { return this._messageid; } }
        public UInt32 CategoryID { get { return 20000; } } // Hardcoded MDB category ID
        public string[] Arguments { get { return this._arguments; } }
        public SystemMessageType Type { get { return this._type; } }
        public string Notice { get { return this._notice; } }
    }
}
