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
    /// Class for holding event args for channel message events.
    /// </summary>
    public class ChannelMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor for channel message events
        /// </summary>
        /// <param name="channelID">5-byte channel id</param>
        /// <param name="channel">channel name</param>
        /// <param name="characterID">character id</param>
        /// <param name="character">character name</param>
        /// <param name="message">message</param>
        public ChannelMessageEventArgs(BigInteger channelID, string channel, UInt32 characterID, string character, string message, ChannelType type, bool outgoing)
        {
            this.ChannelID = channelID;
            this.Channel = channel;
            this.CharacterID = characterID;
            this.Character = character;
            this.Message = message;
            this.Type = type;
            this.Outgoing = outgoing;
        }

        public BigInteger ChannelID { get; private set; }
        public string Channel { get; private set; }
        public UInt32 CharacterID { get; private set; }
        public string Character { get; private set; }
        public string Message { get; private set; }
        public ChannelType Type { get; private set; }
        public bool Outgoing { get; private set; }

    }
}
