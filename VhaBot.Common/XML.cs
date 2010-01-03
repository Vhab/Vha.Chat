/*
* VhaBot.Common
* Copyright (C) 2005-2009 Remco van Oosterhout
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

namespace VhaBot.Common
{
    public static class XML<T>
    {
        private static Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();
        private static XmlSerializer GetSerializer()
        {
            if (!_serializers.ContainsKey(typeof(T))) _serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
            return _serializers[typeof(T)];
        }

        /// <summary>
        /// Reads an xml file and deserializes it into object T
        /// </summary>
        /// <param name="file">The relative or full path to the file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromFile(string file)
        {
            try
            {
                Stream stream = (Stream)File.Open(file, FileMode.Open, FileAccess.Read);
                T obj = (T)GetSerializer().Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch { return default(T); }
        }

        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromUrl(string url) { return FromUrl(url, 30000); }
        /// <summary>
        /// Reads xml from a web server and deserializes it into object T
        /// </summary>
        /// <param name="url">The url to the xml file to be deserialized</param>
        /// <param name="timeout">Request timeout in miliseconds</param>
        /// <returns>The deserialized object</returns>
        /// <remarks>Returns NULL when the operation fails</remarks>
        public static T FromUrl(string url, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "VhaBot/1.0";
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                T obj = (T)GetSerializer().Deserialize(stream);
                response.Close();
                return obj;
            }
            catch { return default(T); }
        }
    }
}
