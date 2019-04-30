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
using System.Collections.Generic;

namespace Vha.Chat
{
    public class MessageTarget
        : IEquatable<MessageTarget>
        , IComparable<MessageTarget>
    {
        public readonly MessageType Type;
        public readonly string Target;
        private readonly int _hashCode;

        public MessageTarget()
        {
            this.Type = MessageType.None;
            this.Target = null;
            this._hashCode = (this.Type.ToString() + this.Target).GetHashCode();
        }

        public MessageTarget(MessageType type, string target)
        {
            this.Type = type;
            this.Target = target;
        }

        public bool Valid
        {
            get
            {
                switch (this.Type)
                {
                    case MessageType.Channel:
                    case MessageType.Character:
                    case MessageType.PrivateChannel:
                        return !string.IsNullOrEmpty(this.Target);
                    default:
                        return false;
                }
            }
        }

        public override string ToString()
        {
            if (this.Type == MessageType.None)
                return "NONE";
            return this.Target;
        }

        public int CompareTo(MessageTarget target)
        {
            if (target == null)
                return 1;
            if (this.Type != target.Type)
                return this.Type.CompareTo(target.Type);
            if (this.Target != target.Target)
                return this.Target.CompareTo(target.Target);
            return 0;
        }

        public bool Equals(MessageTarget right) { return this.CompareTo(right) == 0; }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);
            if (!(obj is MessageTarget)) return false;
            return this.Equals((MessageTarget)obj);
        }

        public override int GetHashCode() { return this._hashCode; }
    }
}
