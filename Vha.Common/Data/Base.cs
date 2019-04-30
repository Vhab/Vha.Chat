/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
using System.Xml.Schema;
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

        [XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string XsiSchemaLocation { get; set; }
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string XsiNoNamespaceSchemaLocation { get; set; }

        public abstract Base Upgrade();

        #region Static methods
        public static void Register(string name, int version, Type type) { Register(name, version, type, null, null); }
        public static void Register(string name, int version, Type type, string xsdNamespace, string xsdSchema)
        {
            string id = name + "_" + version.ToString();
            Registration r = new Registration();
            r.Name = name;
            r.Version = version;
            r.Type = type;
            r.Namespace = xsdNamespace;
            r.Schema = xsdSchema;
            lock (_types)
            {
                if (_types.ContainsKey(id))
                    throw new ArgumentException("A type has already been registered for " + name + " v" + version);
                _types.Add(id, r);
            }
        }

        public static void Unregister(string name, int version)
        {
            string id = name + "_" + version.ToString();
            lock (_types)
            {
                if (_types.ContainsKey(id)) _types.Remove(id);
            }
        }

        public static bool IsRegistered(string name, int version)
        {
            string id = name + "_" + version.ToString();
            lock (_types)
            {
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
                // Start reading XML header
                reader = XmlReader.Create(stream);
                if (reader.ReadToFollowing("Root") == false)
                    throw new ArgumentException("Unable to locate root element");
                // Determine the type of the XML file
                string name = reader.GetAttribute("Name");
                string version = reader.GetAttribute("Version");
                reader.Close();
                // XML file doesn't meet our expected format
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(version))
                    throw new ArgumentException("Unable to locate Name or Version attribute");
                string type = name + "_" + version;
                
                // Load file as known type
                Registration registration = null;
                lock (_types)
                {
                    if (_types.ContainsKey(type))
                        registration = _types[type];
                }
                if (registration == null)
                    throw new ArgumentException("No registered data class for type " + name + " v" + version);
                // Verify document
                if (!string.IsNullOrEmpty(registration.Schema))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    XmlReaderSettings readerSettings = new XmlReaderSettings();
                    readerSettings.ConformanceLevel = ConformanceLevel.Document;
                    // Configure XSD validation
                    readerSettings.ValidationType = ValidationType.Schema;
                    readerSettings.Schemas.Add(registration.Namespace, registration.Schema);
                    readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    // Validate document
                    stream.Seek(0, SeekOrigin.Begin);
                    reader = XmlReader.Create(stream, readerSettings);
                    while (reader.Read()) { }
                    reader.Close();
                }
                // Deserialize
                stream.Seek(0, SeekOrigin.Begin);
                reader = XmlReader.Create(stream);
                XmlSerializer serializer = XML.GetSerializer(registration.Type);
                ErrorCatcher errorCatcher = new ErrorCatcher();
                serializer.UnknownAttribute += new XmlAttributeEventHandler(errorCatcher.OnUnknownAttribute);
                serializer.UnknownElement += new XmlElementEventHandler(errorCatcher.OnUnknownElement);
                serializer.UnknownNode += new XmlNodeEventHandler(errorCatcher.OnUnknownNode);
                Base data = (Base)serializer.Deserialize(reader);
                // Verify output
                if (errorCatcher.Exceptions.Count > 0)
                    throw errorCatcher.Exceptions[0];
                if (data == null)
                    return null;
                // Upgrade data if needed
                while (data.CanUpgrade)
                    data = data.Upgrade();
                // Apply XSD information
                if (!string.IsNullOrEmpty(registration.Namespace) && !string.IsNullOrEmpty(registration.Schema))
                {
                    // TODO: figure out a way to set the 'xmlns' attribute
                    data.XsiSchemaLocation = registration.Namespace + " " + registration.Schema;
                }
                else if (!string.IsNullOrEmpty(registration.Schema))
                {
                    data.XsiNoNamespaceSchemaLocation = registration.Schema;
                }
                // Everything went right, pfew
                return data;
            }
            finally
            {
                // Close anything left open
                if (reader != null)
                    reader.Close();
            }
        }

        private static Dictionary<string, Registration> _types = new Dictionary<string, Registration>();
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

        private class Registration
        {
            public string Name;
            public int Version;
            public Type Type;
            public string Namespace;
            public string Schema;
        }

        private class ErrorCatcher
        {
            public List<Exception> Exceptions = new List<Exception>();
            public void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
            {
                Exceptions.Add(new Exception("Unexpected attribute " + e.Attr.Name + " at " + e.LineNumber + ":" + e.LinePosition));
            }
            public void OnUnknownElement(object sender, XmlElementEventArgs e)
            {
                Exceptions.Add(new Exception("Unexpected element " + e.Element.Name + " at " + e.LineNumber + ":" + e.LinePosition));
            }
            public void OnUnknownNode(object sender, XmlNodeEventArgs e)
            {
                Exceptions.Add(new Exception("Unexpected node " + e.Name + " at " + e.LineNumber + ":" + e.LinePosition));
            }
        }
        #endregion
    }
}
