/*
* Vha.Net
* Copyright (C) 2005-2009 Remco van Oosterhout
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

namespace Vha.Net.Events
{
    /// <summary>
    /// Holds event args for private channel messages.
    /// </summary>
    public class PrivateChannelMessageEventArgs : EventArgs
    {
        private readonly UInt32 _channelID = 0;
        private readonly string _channel;
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly string _message = null;
        private bool _local = false;

        /// <summary>
        /// private channel message event message constructor
        /// </summary>
        /// <param name="channelID">The id of the private chat channel</param>
        /// <param name="characterID">The character id of the sender</param>
        /// <param name="message">Message containing text and click links</param>
        public PrivateChannelMessageEventArgs(UInt32 channelID, string channel, UInt32 characterID, string character, string message, bool local)
        {
            this._channelID = channelID;
            this._channel = channel;
            this._characterID = characterID;
            this._character = character;
            this._message = message;
            this._local = local;
        }

        /// <summary>
        /// id of the channel where the message originated
        /// </summary>
        public UInt32 ChannelID { get { return this._channelID; } }
        public string Channel { get { return this._channel; } }

        /// <summary>
        /// id of the sender
        /// </summary>
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }

        /// <summary>
        /// message contents containing text and click links
        /// </summary>
        public string Message { get { return this._message; } }

        /// <summary>
        /// set true when message is from the bot's own private channel
        /// </summary>
        public bool Local { get { return this._local; } }
    }
}
