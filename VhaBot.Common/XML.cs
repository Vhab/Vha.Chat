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
            if (!_serializers.ContainsKey(T)) _serializers.Add(T, new XmlSerializer(T));
            return _serializers[T];
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
                T obj = GetSerializer().Deserialize(stream);
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
                T obj = GetSerializer().Deserialize(stream);
                response.Close();
                return obj;
            }
            catch { return default(T); }
        }
    }
}
