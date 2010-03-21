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

namespace Vha.Chat
{
    [XmlRoot("Config"), Serializable]
    public class Config
    {
        public int MaximumMessages = 999;
        public int MaximumTexts = 99;
        public int MaximumHistory = 99;
        public ChatHtmlStyle TextStyle = ChatHtmlStyle.Default;
        public string Proxy;
        /// <summary>
        /// Unique ignore lists per...
        /// </summary>
        public IgnoreMethod IgnoreMethod = IgnoreMethod.Dimension;

        /// <summary>
        /// Is the ignore system enabled?
        /// </summary>
        public bool IgnoreEnabled = true;

        /// <summary>
        /// Last logged in dimension.
        /// </summary>
        public string Dimension = "";
        /// <summary>
        /// Last logged in account
        /// </summary>
        public string Account = "";
        /// <summary>
        /// Last logged in characters per account
        /// </summary>
        public List<ConfigAccount> Accounts = new List<ConfigAccount>();
    }

    public class ConfigAccount
    {
        [XmlAttribute("account")]
        public string Account = "";
        [XmlAttribute("character")]
        public string Character = "";
    }
}
