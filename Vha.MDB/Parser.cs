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
            int offset = 0;
            // Check header
            if (Binary.ReadByte(ref data, ref offset) != Convert.ToChar('&'))
                throw new ArgumentException("Expecting valid template message header");
            // Read header
            int categoryId = Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
            int entryId = Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
            // Read message
            byte[] moreData = new byte[data.Length - offset];
            Array.Copy(data, offset, moreData, 0, data.Length - offset);
            Message message = Decode(categoryId, entryId, moreData, reader);
            return message;
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
            // Create reader
            if (reader == null)
                reader = new Reader();
            // Create 'empty' message
            Message descrambledMessage = new Message(categoryId, entryId);
            // And here the real magic happens!
            int offset = 0;
            while (true)
            {
                // Finished reading data
                if (offset >= data.Length)
                    break;

                char type = Binary.ReadChar(ref data, ref offset);
                switch (type)
                {
                    case 'S': // Long string
                        String str = Binary.ReadString(ref data, ref offset, Encoding.UTF8, Endianness.BigEndian);
                        descrambledMessage.Append(new Argument(str));
                        break;
                    case 's': // Short string
                        Int32 textSize = Binary.ReadByte(ref data, ref offset);
                        String text = Binary.ReadString(ref data, ref offset, textSize - 1, Encoding.UTF8);
                        descrambledMessage.Append(new Argument(text));
                        break;
                    case 'I': // Raw Integer
                        Int32 rawInteger = Binary.ReadInt32(ref data, ref offset, Endianness.BigEndian);
                        descrambledMessage.Append(new Argument(rawInteger));
                        break;
                    case 'i': // Integer
                        Int32 integer = Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
                        descrambledMessage.Append(new Argument(integer));
                        break;
                    case 'u': // Unsigned Integer
                        UInt32 unsignedInteger = (UInt32)Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
                        descrambledMessage.Append(new Argument(unsignedInteger));
                        break;
                    case 'f': // Float
                        // TODO: figure out syntax and implement properly
                        throw new NotImplementedException("Float types are not yet supported by this parser");
                    case 'R': // Reference, Category ID and Entry ID
                        String referenceMessage = string.Empty;
                        Int32 referenceCategoryID = Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
                        Int32 referenceEntryID = Base85.Decode(Binary.ReadString(ref data, ref offset, 5, Encoding.UTF8));
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
                        Int32 recursiveSize = Binary.ReadByte(ref data, ref offset);
                        byte[] recursiveData = new byte[recursiveSize];
                        Array.Copy(data, offset, recursiveData, 0, recursiveSize);
                        descrambledMessage.Append(new Argument(Decode(recursiveData, reader)));
                        break;
                    case 'l': // System submessage
                        Int32 systemEntryID = Binary.ReadInt32(ref data, ref offset, Endianness.BigEndian);
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
