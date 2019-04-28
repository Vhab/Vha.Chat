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
using System.IO;
using System.Net;
using System.Threading;

namespace Vha.Common
{
    public static class Web
    {
        private static Dictionary<string, Limit> _limits = new Dictionary<string, Limit>();
        public static void SetLimit(string domain, double duration)
        {
            domain = domain.ToLower();
            lock (_limits)
            {
                if (duration > 0)
                {
                    // Set limit
                    if (!_limits.ContainsKey(domain))
                        _limits.Add(domain, new Limit(domain, duration));
                    else
                    {
                        _limits[domain].Domain = domain;
                        _limits[domain].Duration = duration;
                    }
                }
                else
                {
                    // Remove limit
                    if (_limits.ContainsKey(domain))
                        lock (_limits[domain])
                            _limits.Remove(domain);
                }
            }
        }

        private static Limit GetLimit(string domain)
        {
            domain = domain.ToLower();
            lock (_limits)
            {
                if (_limits.ContainsKey(domain))
                    return _limits[domain];
            }
            return null;
        }

        public static HttpWebResponse GetResponse(string url, int timeout, bool keepalive)
        {
            // Construct request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Vha.Common/1.0";
            request.KeepAlive = keepalive;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Timeout = timeout;
            HttpWebResponse response = null;
            // Take notice of any limits applied
            Limit limit = GetLimit(request.Address.Host);
            if (limit != null)
            {
                // Ensure this thread is the only one utilizing this limit
                lock (limit)
                {
                    // Apply limit
                    TimeSpan elapsed = DateTime.Now - limit.LastRequest;
                    if (elapsed.TotalSeconds < limit.Duration)
                    {
                        double sleep = limit.Duration - elapsed.TotalSeconds;
                        Thread.Sleep((int)(sleep * 1000.0));
                    }
                    // Request
                    try { response = (HttpWebResponse)request.GetResponse(); }
                    catch (Exception) { }
                    // Update time
                    limit.LastRequest = DateTime.Now;
                }
            }
            else
            {
                // No limits applied, just request it
                try { response = (HttpWebResponse)request.GetResponse(); }
                catch (Exception) { }
            }
            return response;
        }

        public static string GetSource(string url, int timeout, bool keepalive)
        {
            HttpWebResponse response = GetResponse(url, timeout, keepalive);
            // Handle failure
            if (response == null) return null;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                response.Close();
                return null;
            }
            // Some setup
            string source = null;
            StreamReader reader = null;
            MemoryStream data = new MemoryStream();
            byte[] buffer = new byte[1024];
            try
            {
                // Select encoding
                Encoding encoding = string.IsNullOrEmpty(response.CharacterSet)
                    ? Encoding.ASCII
                    : Encoding.GetEncoding(response.CharacterSet);
                // Extract data
                Stream stream = response.GetResponseStream();
                int read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    data.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                // Reader and decode data
                data.Seek(0, SeekOrigin.Begin);
                reader = new StreamReader(data, encoding);
                source = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                response.Close();
                data.Close();
            }
            return source;
        }

        private enum StripState
        {
            Text,
            InsideTag,
            InsideString
        }

        public static string StripTags(string text)
        {
            text = text.Replace("\r", "");
            text = text.Replace("\t", " ");
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            char[] charText = text.ToCharArray();
            string strippedText = String.Empty;
            StripState state = StripState.Text;
            int readFrom = 0;

            for (int i = 0; i < charText.Length; i++)
            {
                switch (charText[i])
                {
                    case '>':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.Text;
                            readFrom = i + 1;
                        }
                        break;
                    case '"':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.InsideString;
                            break;
                        }
                        if (state == StripState.InsideString)
                        {
                            state = StripState.InsideTag;
                        }
                        break;
                    case '<':
                        if (state == StripState.Text || state == StripState.InsideTag)
                        {
                            strippedText += text.Substring(readFrom, i - readFrom);
                            readFrom = i;
                            state = StripState.InsideTag;
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            strippedText += text.Substring(readFrom);
            return strippedText;
        }

        /// <summary>
        /// Replace a selection of risky characters with their html-safe equelants.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EscapeHtml(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("'", "&#039;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        /// <summary>
        /// Replace html-safe codes with their actual character values 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnescapeHtml(string text)
        {
            text = text.Replace("&gt;", ">");
            text = text.Replace("&lt;", "<");
            text = text.Replace("&#039;", "'");
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&amp;", "&");
            return text;
        }

        private class Limit
        {
            public string Domain;
            public double Duration;
            public DateTime LastRequest;

            public Limit(string domain, double duration)
            {
                this.Domain = domain;
                this.Duration = duration;
                this.LastRequest = DateTime.Now.AddSeconds(-duration);
            }
        }
    }
}
