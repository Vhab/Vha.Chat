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

namespace Vha.Net.Events
{
    public class PrivateChannelStatusEventArgs : EventArgs
    {
        public PrivateChannelStatusEventArgs(UInt32 channelID, string channel, UInt32 characterID, string character, bool join, bool local)
        {
            this.ChannelID = channelID;
            this.Channel = channel;
            this.CharacterID = characterID;
            this.Character = character;
            this.Join = join;
            this.Local = local;
        }

        /// <summary>
        /// ID of the owner of the private channel the event occurs in.
        /// </summary>
        public UInt32 ChannelID { get; private set; }
        /// <summary>
        /// Name of the owner of the private channel the event occurs in.
        /// </summary>
        public string Channel { get; private set; }
        /// <summary>
        /// ID of the character who is triggering this event.
        /// </summary>
        public UInt32 CharacterID { get; private set; }
        /// <summary>
        /// Name of the character who is triggering this event.
        /// </summary>
        public string Character { get; private set; }
        /// <summary>
        /// Whether someone has joined or left this channel. True if the character has joined, false when the character left.
        /// </summary>
        public bool Join { get; private set; }
        /// <summary>
        /// Whether this channel is owned by our character.
        /// </summary>
        public bool Local { get; private set; }
        /// <summary>
        /// Returns the combined private channel data
        /// </summary>
        /// <returns></returns>
        public PrivateChannel GetPrivateChannel()
        {
            return new PrivateChannel(this.ChannelID, this.Channel, this.Local);
        }
    }
}
