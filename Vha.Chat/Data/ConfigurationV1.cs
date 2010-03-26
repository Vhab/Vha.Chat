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
using System.Xml.Serialization;

namespace Vha.Chat.Data
{
    public class ConfigurationV1 : Base
    {
        public string OptionsPath = "%APPDATA%\\Vha.Chat\\";
        public string OptionsFile = "Options.xml";
        public string IgnoresPath = "%APPDATA%\\Vha.Chat\\Ignores\\";
        [XmlElement("Server")]
        public List<ConfigurationV1Server> Servers;

        #region Implement Base
        public ConfigurationV1()
            : base("Configuration", 1, false, typeof(ConfigurationV1))
        { }

        public override Base Upgrade() { return null; }
        #endregion
    }

    public class ConfigurationV1Server
    {
        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("Address")]
        public string Address;
        [XmlAttribute("Port")]
        public int Port;
    }
}
