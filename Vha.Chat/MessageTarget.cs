﻿/*
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
    public class MessageTarget : IComparable<MessageTarget>
    {
        public readonly MessageType Type;
        public readonly string Target;

        public MessageTarget()
        {
            this.Type = MessageType.None;
            this.Target = null;
        }

        public MessageTarget(MessageType type, string target)
        {
            this.Type = type;
            this.Target = target;
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
    }
}