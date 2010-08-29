/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Vha.Common.Data
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

        public abstract Base Upgrade();

        #region Static methods
        public static void Register(string name, int version, Type type)
        {
            lock (_types)
            {
                string id = name + "_" + version.ToString();
                if (_types.ContainsKey(id)) _types[id] = type;
                else _types.Add(id, type);
            }
        }

        public static void Unregister(string name, int version)
        {
            lock (_types)
            {
                string id = name + "_" + version.ToString();
                if (_types.ContainsKey(id)) _types.Remove(id);
            }
        }

        public static bool IsRegistered(string name, int version)
        {
            lock (_types)
            {
                string id = name + "_" + version.ToString();
                return _types.ContainsKey(id);
            }
        }

        public static Base FromStream(Stream stream)
        {
            // Check stream
            if (stream == null)
                throw new ArgumentNullException();
            if (!stream.CanRead)
                throw new ArgumentException("Stream can not be read");
            XmlReader reader = null;
            try
            {
                // Start reading XML
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
                Type dataType = null;
                lock (_types)
                {
                    if (_types.ContainsKey(type))
                        dataType = _types[type];
                }
                // Read data
                Base data = (Base)XML.FromStream(dataType, stream, false);
                if (data == null)
                    return null;
                // Upgrade data if needed
                while (data.CanUpgrade)
                    data = data.Upgrade();
                return data;
            }
            catch (Exception) { return null; }
            finally
            {
                // Close anything left open
                if (reader != null)
                    reader.Close();
            }
        }

        private static Dictionary<string, Type> _types = new Dictionary<string, Type>();
        #endregion

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
