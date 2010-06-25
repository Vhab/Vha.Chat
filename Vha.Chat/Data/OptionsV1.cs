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
    [XmlRoot("Root")]
    public class OptionsV1 : Base
    {
        public int MaximumMessages = 250;
        public int MaximumTexts = 50;
        public int MaximumHistory = 20;
        public UI.ChatHtmlStyle TextStyle = UI.ChatHtmlStyle.Strip;

        public OptionsV1Proxy Proxy = null;

        public string LastDimension = "";
        public string LastAccount = "";
        [XmlElement("Account")]
        public List<OptionsV1Account> Accounts = new List<OptionsV1Account>();
        [XmlElement("Window")]
        public List<OptionsV1Window> Windows = new List<OptionsV1Window>();

        #region Implement Base
        public OptionsV1()
            : base("Options", 1, false, typeof(OptionsV1))
        { }

        public override Base Upgrade() { return null; }
        #endregion
    }

    public class OptionsV1Proxy
    {
        [XmlAttribute("Type")]
        public string Type;
        [XmlAttribute("Address")]
        public string Address;
        [XmlAttribute("Port")]
        public int Port;
        [XmlAttribute("Username")]
        public string Username;
        [XmlAttribute("Password")]
        public string Password;
    }

    public class OptionsV1Account
    {
        [XmlAttribute("Dimension")]
        public string Dimension = "";
        [XmlAttribute("Name")]
        public string Name = "";
        [XmlAttribute("Character")]
        public string Character = "";
    }

    public class OptionsV1Window
    {
        [XmlAttribute("Name")]
        public string Name = "";
        [XmlAttribute("X")]
        public int X = 0;
        [XmlAttribute("Y")]
        public int Y = 0;
        [XmlAttribute("Width")]
        public int Width = 0;
        [XmlAttribute("Height")]
        public int Height = 0;
    }
}
