/*
* VhaBot.Net
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

namespace VhaBot.Net.Events
{
    /// <summary>
    /// Holds event args for private messages.
    /// </summary>
    public class PrivateMessageEventArgs : EventArgs
    {
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly string _msg = null;
        private readonly bool _outgoing = false;

        /// <summary>
        /// Private message message event argument constructor
        /// </summary>
        /// <param name="CharacterID">The character id of the sender.</param>
        /// <param name="Message">Message containing text and click links</param>
        /// <param name="VoiceCommand">voice blob in the message, if any</param>
        public PrivateMessageEventArgs(UInt32 characterID, string character, string message, bool outgoing)
        {
            this._characterID = characterID;
            this._character = character;
            this._msg = message;
            this._outgoing = outgoing;
        }

        /// <summary>
        /// The character id of the sender.
        /// </summary>
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        /// <summary>
        /// The originating message containing any text and click links.
        /// </summary>
        public string Message { get { return this._msg; } }
        /// <summary>
        /// Whether the message is an outgoing or incomming message
        /// </summary>
        public bool Outgoing { get { return this._outgoing; } }
    }
}