/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
        : IEquatable<MessageSource>
        , IComparable<MessageSource>
    {
        public readonly MessageType Type;
        public readonly string Channel;
        public readonly string Character;
        public readonly bool Outgoing;
        private readonly int _hashCode;

        public MessageSource()
        {
            this.Type = MessageType.None;
            this.Channel = null;
            this.Character = null;
            this.Outgoing = false;
            this._hashCode = 0;
        }

        public MessageSource(MessageType type, string channel, string character, bool outgoing)
        {
            this.Type = type;
            this.Channel = channel;
            this.Character = character;
            this.Outgoing = outgoing;
            this._hashCode =
                (this.Type.ToString() +
                this.Channel +
                this.Character +
                this.Outgoing.ToString())
                .GetHashCode();
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

        public override string ToString()
        {
            if (this.Type == MessageType.None)
                return "NONE";
            if (this.Type == MessageType.Character)
                return this.Character;
            return this.Character + "@" + this.Channel;
        }

        public int CompareTo(MessageSource source)
        {
            if (source == null)
                return 1;
            if (this.Type != source.Type)
                return this.Type.CompareTo(source.Type);
            if (this.Channel != source.Channel)
                return this.Channel.CompareTo(source.Channel);
            if (this.Character != source.Character)
                return this.Character.CompareTo(source.Character);
            if (this.Outgoing != source.Outgoing)
                return this.Outgoing.CompareTo(source.Outgoing);
            return 0;
        }

        public bool Equals(MessageSource right) { return this.CompareTo(right) == 0; }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);
            if (!(obj is MessageSource)) return false;
            return this.Equals((MessageSource)obj);
        }

        public override int GetHashCode() { return this._hashCode; }
    }
}
