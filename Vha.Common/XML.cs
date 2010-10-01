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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Vha.Common
{
    public static class XML
    {
        private static Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
        public static XmlSerializer GetSerializer(Type type)
        {
            lock (_serializers)
            {
                if (!_serializers.ContainsKey(type))
                    _serializers.Add(type, new XmlSerializer(type));
                return _serializers[type];
            }
        }



        /// <summary>
        /// Reads an xml file and deserializes it into object T
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="file">The relative or full path to the file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static Object FromFile(Type type, string file)
        {
            Stream stream = null;
            try
            {
                stream = (Stream)File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                Object obj = GetSerializer(type).Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch (Exception)
            {
                if (stream != null)
                    stream.Close();
                return null;
            }
        }

        /// <summary>
        /// Deserializes XML from stream
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="stream">Stream to deserialize</param>
        /// <param name="closeStream">Should we close stream?</param>
        /// <returns></returns>
        public static Object FromStream(Type type, Stream stream, bool closeStream)
        {
            try
            {
                Object obj = GetSerializer(type).Deserialize(stream);
                if (closeStream) stream.Close();
                return obj;
            }
            catch (Exception)
            {
                if (stream != null && closeStream)
                    stream.Close();
                return null;
            }
        }

        /// <summary>
        /// Deserializes XML from a string of xml
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="xml">XML data contained in a string</param>
        /// <returns></returns>
        public static Object FromString(Type type, string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;
            StringReader source = null;
            XmlTextReader reader = null;
            try
            {
                source = new StringReader(xml);
                reader = new XmlTextReader(source);
                Object obj = GetSerializer(type).Deserialize(reader);
                return obj;
            }
            catch (Exception) { return null; }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (source != null)
                    source.Close();
            }
        }

        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static Object FromUrl(Type type, string url) { return FromUrl(type, url, 30000, true); }
        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <param name="timeout">Request timeout in miliseconds</param>
        /// <param name="keepalive">Keep the connection alive</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static Object FromUrl(Type type, string url, int timeout, bool keepalive)
        {
            return FromString(type, Web.GetSource(url, timeout, keepalive));
        }

        /// <summary>
        /// Serializes an object to an XML file
        /// </summary>
        /// <param name="file">The relative or full path to the target xml file</param>
        /// <param name="type">Type of the object</param>
        /// <param name="obj">The object to be serialized</param>
        /// <returns>true on success, false on failure</returns>
        public static bool ToFile(string file, Type type, Object obj)
        {
            // Serialize the data
            MemoryStream memorystream = new MemoryStream(); //Stream to write serialized output to.
            Stream filestream = null;
            try
            {
                GetSerializer(type).Serialize(memorystream, obj);
            }
            catch (Exception)
            {
                // Serailization failed
                if (memorystream != null)
                    memorystream.Close();
                return false;
            }
            // Write serialized data to file
            try
            {
                filestream = (Stream)File.Open(file, FileMode.Create, FileAccess.Write);
                memorystream.WriteTo(filestream);
                filestream.Flush();
                filestream.Close();
                memorystream.Close();
                return true;
            }
            catch (Exception)
            {
                // Clean up and return
                if (memorystream != null)
                    memorystream.Close();
                if (filestream != null)
                    filestream.Close();
                return false;
            }
        }

        /// <summary>
        /// Writes to an already opened stream.
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        /// <param name="type">Type of the object</param>
        /// <param name="obj">Object to write to stream</param>
        /// <param name="closeStream">Should we close the stream when done?</param>
        /// <exception cref="ArgumentNullException">If any of the provided parameters are null</exception>
        /// <returns></returns>
        public static bool ToStream(Stream stream, Type type, Object obj, bool closeStream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (obj == null) throw new ArgumentNullException("obj");
            // Serialize the data
            try
            {
                GetSerializer(type).Serialize(stream, obj);
                if (closeStream) stream.Close();
                return true;
            }
            catch (Exception)
            {
                // Serailization failed
                if (closeStream && stream != null)
                    stream.Close();
                return false;
            }
        }

        /// <summary>
        /// Serialize object to an existing XmlWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToXmlWriter(XmlWriter writer, Type type, Object obj)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (obj == null) throw new ArgumentNullException("obj");
            try
            {
                GetSerializer(type).Serialize(writer, obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Serialize object to a string
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(Type type, Object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            MemoryStream stream = null;
            StreamWriter writer = null;
            StreamReader reader = null;
            try
            {
                stream = new MemoryStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
                GetSerializer(type).Serialize(writer, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();
            }
        }
    }

    public static class XML<T> where T : class
    {
        private static Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
        public static XmlSerializer GetSerializer()
        {
            return XML.GetSerializer(typeof(T));
        }

        /// <summary>
        /// Reads an xml file and deserializes it into object T
        /// </summary>
        /// <param name="file">The relative or full path to the file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromFile(string file)
        {
            return (T)XML.FromFile(typeof(T), file);
        }

        /// <summary>
        /// Deserializes XML from stream
        /// </summary>
        /// <param name="stream">Stream to deserialize</param>
        /// <param name="closeStream">Should we close stream?</param>
        /// <returns></returns>
        public static T FromStream(Stream stream, bool closeStream)
        {
            return (T)XML.FromStream(typeof(T), stream, closeStream);
        }

        /// <summary>
        /// Deserializes XML from a string of xml
        /// </summary>
        /// <param name="xml">XML data contained in a string</param>
        /// <returns></returns>
        public static T FromString(string xml)
        {
            return (T)XML.FromString(typeof(T), xml);
        }

        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromUrl(string url)
        {
            return (T)XML.FromUrl(typeof(T), url);
        }
        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <param name="timeout">Request timeout in miliseconds</param>
        /// <param name="keepalive">Keep the connection alive</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromUrl(string url, int timeout, bool keepalive)
        {
            return (T)XML.FromUrl(typeof(T), url, timeout, keepalive);
        }

        /// <summary>
        /// Serializes an object to an XML file
        /// </summary>
        /// <param name="file">The relative or full path to the target xml file</param>
        /// <param name="Obj">The object to be serialized</param>
        /// <returns>true on success, false on failure</returns>
        public static bool ToFile(string file, T obj)
        {
            return XML.ToFile(file, typeof(T), obj);
        }

        /// <summary>
        /// Writes to an already opened stream.
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        /// <param name="obj">Object to write to stream</param>
        /// <param name="CloseStream">Should we close the stream when done?</param>
        /// <exception cref="ArgumentNullException">If any of the provided parameters are null</exception>
        /// <returns></returns>
        public static bool ToStream(Stream stream, T obj, bool closeStream)
        {
            return XML.ToStream(stream, typeof(T), obj, closeStream);
        }

        /// <summary>
        /// Serialize object to an existing XmlWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToXmlWriter(XmlWriter writer, T obj)
        {
            return XML.ToXmlWriter(writer, typeof(T), obj);
        }

        /// <summary>
        /// Serialize object to a string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(T obj)
        {
            return XML.ToString(typeof(T), obj);
        }
    }   
}