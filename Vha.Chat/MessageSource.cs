/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.Text;

namespace Vha.Chat
{
    public class MessageSource
    {
        public readonly MessageType Type;
        public readonly string Channel;
        public readonly string Character;
        public readonly bool Outgoing;

        public MessageSource()
        {
            this.Type = MessageType.None;
            this.Channel = null;
            this.Character = null;
            this.Outgoing = false;
        }

        public MessageSource(MessageType type, string channel, string character, bool outgoing)
        {
            this.Type = type;
            this.Channel = channel;
            this.Character = character;
            this.Outgoing = outgoing;
        }

        public override string ToString()
        {
            if (this.Type == MessageType.None)
                return "NONE";
            if (this.Type == MessageType.Character)
                return this.Character;
            return this.Character + "@" + this.Channel;
        }

        public bool Equals(MessageSource source)
        {
            if (source == null) return false;
            if (this.Type != source.Type) return false;
            if (this.Channel != source.Channel) return false;
            if (this.Character != source.Character) return false;
            if (this.Outgoing != source.Outgoing) return false;
            return true;
        }

        /// <summary>
        /// Returns the target associated with this source
        /// </summary>
        /// <returns></returns>
        public MessageTarget GetTarget()
        {
            MessageType type = this.Type;
            string target = null;
            switch (this.Type)
            {
                case MessageType.Channel:
                case MessageType.PrivateChannel:
                    target = this.Channel;
                    break;
                case MessageType.Character:
                    target = this.Character;
                    break;
            }
            return new MessageTarget(type, target);
        }
    }
}
