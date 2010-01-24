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

namespace Vha.Net.Events
{
    public class PrivateChannelStatusEventArgs : EventArgs
    {
        private readonly UInt32 _channelID = 0;
        private readonly string _channel;
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly bool _join = false;

        public PrivateChannelStatusEventArgs(UInt32 channelID, string channel, UInt32 characterID, string character, bool join)
        {
            this._channelID = channelID;
            this._channel = channel;
            this._characterID = characterID;
            this._character = character;
            this._join = join;
        }

        public UInt32 ChannelID { get { return this._channelID; } }
        public string Channel { get { return this._channel; } }
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        public bool Join { get { return this._join; } }
    }
}
