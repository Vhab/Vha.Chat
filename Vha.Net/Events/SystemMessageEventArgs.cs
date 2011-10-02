/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
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
        public SystemMessageEventArgs(UInt32 clientID, UInt32 windowID, UInt32 messageID, Byte[] arguments, string notice)
        {
            this.ClientID = clientID;
            this.WindowID = windowID;
            this.MessageID = messageID;
            this.Arguments = arguments;
            if (Enum.IsDefined(typeof(SystemMessageType), (int)messageID))
                this.Type = (SystemMessageType)messageID;
            else 
                this.Type = SystemMessageType.Other;
            this.Notice = notice;
        }

        public UInt32 ClientID { get; private set; }
        public UInt32 WindowID { get; private set; }
        public UInt32 MessageID { get; private set; }
        public UInt32 CategoryID { get { return 2000; } } // Hardcoded MDB category ID
        public Byte[] Arguments { get; private set; }
        public SystemMessageType Type { get; private set; }
        public string Notice { get; private set; }
    }
}
