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
    /// Holds event args for private channel requests
    /// </summary>
    public class PrivateChannelRequestEventArgs : EventArgs
    {

        private Chat _chat;
        private bool _replied = false;
        /// <summary>
        /// Constructor for private channel requests
        /// </summary>
        /// <param name="characterID">the character id</param>
        /// <param name="join">whether asked to join or leave</param>
        public PrivateChannelRequestEventArgs(Chat chat, UInt32 characterID, string character)
        {
            this._chat = chat;
            this.CharacterID = characterID;
            this.Character = character;
        }

        /// <summary>
        /// The character id
        /// </summary>
        public UInt32 CharacterID { get; private set; }
        /// <summary>
        /// The character inviting us
        /// </summary>
        public String Character { get; private set; }
        /// <summary>
        /// Returns the combined private channel data
        /// </summary>
        /// <returns></returns>
        public PrivateChannel GetPrivateChannel()
        {
            return new PrivateChannel(this.CharacterID, this.Character, false);
        }
        /// <summary>
        /// Accept this request
        /// </summary>
        public void Accept()
        {
            if (this._replied) 
                return;
            this._replied = true;
            this._chat.SendPacket(new PrivateChannelStatusPacket(this.CharacterID, true));
        }
        /// <summary>
        /// Decline this request
        /// </summary>
        public void Decline()
        {
            if (this._replied) 
                return;
            this._replied = true;
            this._chat.SendPacket(new PrivateChannelStatusPacket(this.CharacterID, false));
        }
    }
}
