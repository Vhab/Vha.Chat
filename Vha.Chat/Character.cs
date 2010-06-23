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
using System.Collections.Generic;
using System.Text;

namespace Vha.Chat
{
    public class Character
    {
        public readonly string Name;
        public readonly UInt32 ID;
        public readonly UInt32 Level;
        public readonly bool Online;

        public Character(string name, UInt32 id, UInt32 level, bool online)
        {
            this.Name = name;
            this.ID = id;
            this.Level = level;
            this.Online = online;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder(this.Name);
            if (this.Online)
                s.Append(" (Online)");
            return s.ToString();
        }
    }
}
