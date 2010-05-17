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
using Vha.Common;

namespace Vha.MDB
{
    public class Parser
    {
        public static Message Decode(string scrambledString) { return Decode(scrambledString, false, null); }
        public static Message Decode(string scrambledString, bool recursive, Reader reader)
        {
            if (reader == null)
                reader = new Reader();

            if (scrambledString.Length < 2)
                throw new Exception("Invalid message length!");

            if (!scrambledString.StartsWith("~"))
                throw new Exception("Invalid message start! Expecting: ~");

            if (!scrambledString.EndsWith("~") && !recursive)
                throw new Exception("Invalid message end! Expecting: ~");

            scrambledString = scrambledString.Substring(1);
            if (!recursive)
                scrambledString = scrambledString.Substring(0, scrambledString.Length - 1);

            Message descrambledMessage = new Message(0, 0, null);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(scrambledString));

            while (true)
            {
                if (stream.Position >= stream.Length)
                    break;

                //string type = char.ConvertFromUtf32(stream.ReadByte()); /* Not Supported on Mono */
                string type = Convert.ToChar(stream.ReadByte()).ToString();
                switch (type)
                {
                    case "&": // Message start, Category ID and Entry ID
                        descrambledMessage = new Message(
                            Base85.Decode(Read(stream, 5)),
                            Base85.Decode(Read(stream, 5)),
                            "~" + scrambledString + "~"
                        );
                        break;
                    case "s": // String
                        Int32 textSize = stream.ReadByte();
                        String text = Read(stream, textSize - 1);
                        descrambledMessage.Append(new Argument(text));
                        break;
                    case "i": // Integer
                        Int32 integer = Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(integer));
                        break;
                    case "u": // Unsigned Integer
                        UInt32 unsignedInteger = (UInt32)Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(unsignedInteger));
                        break;
                    case "f": // Float
                        Single single = (Single)Base85.Decode(Read(stream, 5));
                        descrambledMessage.Append(new Argument(single));
                        break;
                    case "R": // Reference, Category ID and Entry ID
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
                    case "F": // Recursive, Complete new message
                        Int32 recursiveSize = stream.ReadByte();
                        String recursiveText = Read(stream, recursiveSize - 1);
                        descrambledMessage.Append(new Argument(Decode(recursiveText, true, reader)));
                        break;
                    default:
                        throw new Exception("Unknown type detected: " + type);
                }
            }
            if (reader != null)
            {
                Entry entry = reader.GetEntry(descrambledMessage.CategoryID, descrambledMessage.EntryID);
                if (entry != null)
                {
                    try { descrambledMessage.Value = String.Format(PrintfToFormatString(entry.Message), descrambledMessage.Arguments); }
                    catch { }
                }
                else
                {
                    descrambledMessage.Value = String.Format("UNKNOWN_MDB:{0}:{1}", descrambledMessage.CategoryID, descrambledMessage.EntryID);
                }
            }
            return descrambledMessage;
        }

        private static string Read(MemoryStream stream, Int32 length)
        {
            if (stream.Length < stream.Position + length)
                throw new Exception("Trying to read beyond stream size! Size=" + stream.Length + " Position=" + stream.Position + " Length=" + length);
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }

        public static string PrintfToFormatString(string text)
        {
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
