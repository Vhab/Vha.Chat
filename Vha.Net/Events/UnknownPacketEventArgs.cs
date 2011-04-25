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
using Vha.Net.Packets;

namespace Vha.Net.Events
{
    /// <summary>
    /// Class for holding event args for unknown events.
    /// </summary>
    public class UnknownPacketEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for creating event args
        /// </summary>
        /// <param name="message"></param>
        public UnknownPacketEventArgs(UInt16 type, String message)
        {
            this.PacketType = type;
            this.Message = message;
        }

        /// <summary>
        /// Message from unknown event
        /// </summary>
        /// <value>returns the message</value>
        public String Message { get; private set; }
        public UInt16 PacketType { get; private set; }
    }
}
