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
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Vha.Common;

namespace Vha.Chat.Data
{
    [XmlRoot("Root"), Serializable]
    public abstract class Base
    {
        [XmlAttribute]
        public string Name { get { return this._name; } set { } }
        [XmlAttribute]
        public int Version { get { return this._version; } set { } }
        [XmlIgnore]
        public bool CanUpgrade { get { return this._canUpgrade; } }
        [XmlIgnore]
        public Type Type { get { return this._type; } }

        public static Base Load(string file)
        {
            // Some setup
            XmlReader reader = null;
            Stream stream = null;
            try
            {
                // Open file and start reading XML
                stream = (Stream)File.Open(file, FileMode.Open, FileAccess.Read);
                reader = XmlReader.Create(stream);
                if (reader.ReadToFollowing("Root") == false)
                    return null;
                // Determine the type of the XML file
                string name = reader.GetAttribute("Name");
                string version = reader.GetAttribute("Version");
                // XML file doesn't meet our expected format
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(version))
                    return null;
                string type = name + "_" + version;
                // Load file as known type
                stream.Seek(0, SeekOrigin.Begin);
                Base data;
                switch (type)
                {
                    case "Options_1":
                        data = XML<OptionsV1>.FromStream(stream, false);
                        break;
                    case "Configuration_1":
                        data = XML<ConfigurationV1>.FromStream(stream, false);
                        break;
                }
                // Check if valid data is received
                if (data == null)
                    return null;
                // Upgrade data if needed
                while (data.CanUpgrade)
                    data = data.Upgrade();
                return data;
            }
            catch { return null; }
            finally
            {
                // Close anything left open
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
            }
        }

        public bool Save(string file)
        {
            return XML.ToFile(file, this.Type, this);
        }

        public abstract Base Upgrade();

        #region Internal
        [XmlIgnore]
        private string _name;
        [XmlIgnore]
        private int _version;
        [XmlIgnore]
        private bool _canUpgrade;
        [XmlIgnore]
        private Type _type;

        protected Base(string name, int version, bool canUpgrade, Type type)
        {
            this._name = name;
            this._version = version;
            this._canUpgrade = canUpgrade;
            this._type = type;
        }
        #endregion
    }
}
