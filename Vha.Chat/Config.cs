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
        public Ignore.Method IgnoreMethod = Ignore.Method.Dimension;
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
