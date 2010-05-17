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
using System.Xml.Serialization;

namespace Vha.Chat
{
    public class Dimension
    {
        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("Address")]
        public string Address;
        [XmlAttribute("Port")]
        public int Port;

        public Dimension() { }
        public Dimension(string name, string address, int port)
        {
            this.Name = name;
            this.Address = address;
            this.Port = port;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
