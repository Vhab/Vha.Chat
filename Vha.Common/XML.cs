/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
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
using System.IO;
using System.Net;

namespace Vha.Common
{
    public static class XML
    {
        private static Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
        public static XmlSerializer GetSerializer(Type type)
        {
            if (!_serializers.ContainsKey(type)) _serializers.Add(type, new XmlSerializer(type));
            return _serializers[type];
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
                stream = (Stream)File.Open(file, FileMode.Open, FileAccess.Read);
                Object obj = GetSerializer(type).Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch
            {
                if (stream != null)
                    stream.Close();
                return null;
            }
        }

        /// <summary>
        /// Deserializes XML from stream
        /// </summary>
        /// <param name="stream">Stream to deserialize</param>
        /// <param name="type">Type of the object</param>
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
            catch
            {
                if (stream != null && closeStream)
                    stream.Close();
                return null;
            }
        }

        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static Object FromUrl(Type type, string url) { return FromUrl(type, url, 30000); }
        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <param name="timeout">Request timeout in miliseconds</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static Object FromUrl(Type type, string url, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Vha.Common/1.0";
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Object obj = GetSerializer(type).Deserialize(stream);
                response.Close();
                return obj;
            }
            catch { return null; }
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
            catch
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
                filestream.Close();
                memorystream.Close();
                return true;
            }
            catch
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
            catch
            {
                // Serailization failed
                if (closeStream && stream != null)
                    stream.Close();
                return false;
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
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromUrl(string url, int timeout)
        {
            return (T)XML.FromUrl(typeof(T), url, timeout);
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
    }
}
