/*
* Vha.MDB
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Vha.Common;

namespace Vha.MDB
{
    public static class Parser
    {
        /// <summary>
        /// Decodes a template message with header
        /// </summary>
        /// <param name="scrambled"></param>
        /// <returns></returns>
        public static Message Decode(string scrambled) { return Decode(scrambled, null); }
        /// <summary>
        /// Decodes a template message with header
        /// </summary>
        /// <param name="scrambled"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Message Decode(string scrambled, Reader reader)
        {
            if (scrambled.StartsWith("~"))
                scrambled = scrambled.Substring(1);

            if (scrambled.EndsWith("~"))
                scrambled = scrambled.Substring(0, scrambled.Length - 1);

            return Decode(Encoding.UTF8.GetBytes(scrambled), reader);
        }
        /// <summary>
        /// Decodes a template message with header
        /// </summary>
        /// <param name="data">Template message string converted to bytes</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Message Decode(Byte[] data, Reader reader)
        {
            MemoryStream stream = new MemoryStream(data);
            try
            {
                // Check header
                if (stream.ReadByte() != Convert.ToChar('&'))
                    throw new ArgumentException("Expecting valid template message header");
                // Read header
                int categoryId = Base85.Decode(Read(stream, 5));
                int entryId = Base85.Decode(Read(stream, 5));
                // Read message
                Message message = Decode(categoryId, entryId, stream, reader);
                return message;
            }
            finally
            {
                stream.Close();
            }
        }
        /// <summary>
        /// Decodes a template message without header
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="entryId"></param>
        /// <param name="data"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Message Decode(int categoryId, int entryId, Byte[] data, Reader reader)
        {
            MemoryStream stream = new MemoryStream(data);
            try
            {
                Message message = Decode(categoryId, entryId, stream, reader);
                return message;
            }
            finally
            {
                stream.Close();
            }
        }
        /// <summary>
        /// Decodes a template message without header
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="entryId"></param>
        /// <param name="stream"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Message Decode(int categoryId, int entryId, Stream stream, Reader reader)
        {
            // Create reader
            if (reader == null)
                reader = new Reader();
            // Create 'empty' message
            Message descrambledMessage = new Message(categoryId, entryId);
            // And here the real magic happens!
            while (true)
            {
                if (stream.Position >= stream.Length)
                    break;

                char type = Convert.ToChar(stream.ReadByte());
                switch (type)
                {
                    case 'S':
                        Byte[] strSizeBuffer = new Byte[2];
                        stream.Read(strSizeBuffer, 0, 2);
                        Int16 strSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(strSizeBuffer, 0));
                        String str = Read(stream, strSize);
                        descrambledMessage.Append(new Argument(str));
                        break;
                    case 's': // String
                        Int32 textSize = stream.ReadByte();
                        String text = Read(stream, textSize - 1);
                        descrambledMessage.Append(new Argument(text));
                        break;
                    case 'I': // Raw Integer
                        Byte[] rawIntegerBuffer = new Byte[4];
                        stream.Read(rawIntegerBuffer, 0, 4);
                        Int32 rawInteger = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(rawIntegerBuffer, 0));
                        descrambledMessage.Append(new Argument(rawInteger));
                        break;
                    case 'i': // Integer
                        Int32 integer = Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(integer));
                        break;
                    case 'u': // Unsigned Integer
                        UInt32 unsignedInteger = (UInt32)Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(unsignedInteger));
                        break;
                    case 'f': // Float
                        Single single = (Single)Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(single));
                        break;
                    case 'R': // Reference, Category ID and Entry ID
                        String referenceMessage = string.Empty;
                        Int32 referenceCategoryID = Base85.Decode(Read(stream, 5));
                        Int32 referenceEntryID = Base85.Decode(Read(stream, 5));
                        if (reader != null)
                        {
                            Entry referenceEntry = reader.GetEntry(referenceCategoryID, referenceEntryID);
                            if (referenceEntry != null)
                                referenceMessage = referenceEntry.Message;
                        }
                        Argument reference = new Argument(
                            referenceCategoryID,
                            referenceEntryID,
                            referenceMessage
                        );
                        descrambledMessage.Append(reference);
                        break;
                    case 'F': // Recursive, Complete new message
                        Int32 recursiveSize = stream.ReadByte();
                        String recursiveText = Read(stream, recursiveSize - 1);
                        descrambledMessage.Append(new Argument(Decode(recursiveText, reader)));
                        break;
                    case 'l': // System submessage
                        Byte[] systemBuffer = new Byte[4];
                        stream.Read(systemBuffer, 0, 4);
                        Int32 systemEntryID = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(systemBuffer, 0));
                        string systemMessage = "";
                        if (reader != null)
                        {
                            Entry entry = reader.GetEntry(20000, systemEntryID);
                            if (entry != null)
                                systemMessage = entry.Message;
                        }
                        descrambledMessage.Append(new Argument(20000, systemEntryID, systemMessage));
                        break;
                    default:
                        throw new Exception("Unknown type detected: " + (int)type);
                }
            }
            if (reader != null)
            {
                Entry entry = reader.GetEntry(descrambledMessage.CategoryID, descrambledMessage.EntryID);
                if (entry != null)
                {
                    try { descrambledMessage.Value = String.Format(PrintfToFormatString(entry.Message), descrambledMessage.Arguments); }
                    catch (Exception) { }
                }
                else
                {
                    descrambledMessage.Value = "";
                }
            }
            return descrambledMessage;
        }

        private static string Read(Stream stream, Int32 length)
        {
            if (stream.Length < stream.Position + length)
                throw new Exception("Trying to read beyond stream size! Size=" + stream.Length + " Position=" + stream.Position + " Length=" + length);
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }

        public static string PrintfToFormatString(string text)
        {
            text = text.Replace("{", "{{");
            text = text.Replace("}", "}}");
            Int32 counter = 0;
            string[] types = { "%s", "%u", "%d", "%f" };
            while (true)
            {
                bool typesLeft = false;
                foreach (string type in types)
                    if (text.Contains(type))
                        typesLeft = true;
                if (typesLeft == false)
                    break;

                Int32 index = text.Length;
                foreach (string type in types)
                {
                    Int32 tmpIndex = text.IndexOf(type);
                    if (tmpIndex < index && tmpIndex != -1)
                        index = tmpIndex;
                }
                text = text.Substring(0, index) + "{" + counter + "}" + text.Substring(index + 2);
                counter++;
            }
            return text;
        }
    }
}
