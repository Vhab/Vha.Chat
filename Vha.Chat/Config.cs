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
    }
}
