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
using System.Xml.Serialization;
using Vha.Net;

namespace Vha.Chat
{
    [XmlRoot("Root"), Serializable]
    public class Options
    {
        public int MaximumMessages = 999;
        public int MaximumTexts = 99;
        public int MaximumHistory = 99;
        public ChatHtmlStyle TextStyle = ChatHtmlStyle.Default;
        
        public OptionsProxy Proxy = null;

        public string LastDimension = "";
        public string LastAccount = "";
        [XmlElement("Account")]
        public List<OptionsAccount> Accounts = new List<OptionsAccount>();
        [XmlElement("Window")]
        public List<OptionsWindow> Windows = new List<OptionsWindow>();

        public OptionsWindow GetWindow(string name) { return GetForm(name, false); }
        public OptionsWindow GetWindow(string name, bool create)
        {
            foreach (OptionsWindow window in this.Windows)
                if (window.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return window;
            if (!create) return null;
            OptionsWindow win = new OptionsWindow();
            win.Name = name;
            this.Windows.Add(win);
            return win;
        }

        public OptionsAccount GetAccount(string name) { return GetAccount(name, false); }
        public OptionsAccount GetAccount(string name, bool create)
        {
            foreach (OptionsAccount account in this.Accounts)
                if (account.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return account;
            if (!create) return null;
            OptionsAccount acc = new OptionsAccount();
            acc.Name = name;
            this.Accounts.Add(acc);
            return acc;
        }
    }

    public class OptionsProxy
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

    public class OptionsAccount
    {
        [XmlAttribute("Name")]
        public string Name = "";
        [XmlAttribute("Character")]
        public string Character = "";
    }

    public class OptionsWindow
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
