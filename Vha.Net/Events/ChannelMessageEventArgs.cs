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
    /// Class for holding event args for channel message events.
    /// </summary>
    public class ChannelMessageEventArgs : EventArgs
    {
        private readonly BigInteger _channelID = 0;
        private readonly string _channel;
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly string _message = null;
        private ChannelType _type = ChannelType.Unknown;

        /// <summary>
        /// Constructor for channel message events
        /// </summary>
        /// <param name="channelID">5-byte channel id</param>
        /// <param name="channel">channel name</param>
        /// <param name="characterID">character id</param>
        /// <param name="character">character name</param>
        /// <param name="message">message</param>
        public ChannelMessageEventArgs(BigInteger channelID, string channel, UInt32 characterID, string character, string message, ChannelType type)
        {
            this._channelID = channelID;
            this._channel = channel;
            this._characterID = characterID;
            this._character = character;
            this._message = message;
            this._type = type;
        }

        public BigInteger ChannelID { get { return this._channelID; } }
        public string Channel { get { return this._channel; } }
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        public string Message { get { return this._message; } }
        public ChannelType Type { get { return this._type; } }
    }
}
