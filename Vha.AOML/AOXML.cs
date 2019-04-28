/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
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
using System.Xml;
using Vha.AOML.DOM;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Vha.AOML
{
    /// <summary>
    /// Transfroms AOXML to AOML
    /// </summary>
    public class AOXML
    {
        private const String Aoxml = "aoxml";
        private const String Window = "window";
        private const String Link = "link";
        private const String Content = "content";
        private const String Underline = "u";
        private const String Italic = "i";
        private const String Color = "color";
        private const String Image = "img";
        private const String Source = "src";
        private const String Rdb = "rdb://";
        private const String Tdb = "tdb://";
        private const String Block = "block";
        private const String Left = "left";
        private const String Center = "center";
        private const String Right = "right";
        private const String Command = "command";
        private const String Value = "value";
        private const String Customlink = "customlink";
        private const String Href = "href";
        private const String Break = "br";
        private const String Item = "item";
        private const String Lowid = "low";
        private const String Highid = "high";
        private const String Quality = "ql";
        private const String Entity = "entity";
        private const String Typeid = "type";
        private const String Instanceid = "instance";
        private const String User = "user";
        private const String Name = "name";
        private const String Style = "style";
        private const String Whitespace = "whitespace";
        private const String Count = "count";
        private const String Text = "text";

        private static List<String> StartBlockElements = new List<String> { Block, Left, Center, Right, Break };
        private static List<String> EmptyElements = new List<String> { Break, Image, Whitespace };

        private static Regex whitespaceNormalizeRegex = new Regex(@"\s{2,}", RegexOptions.None);

        /// <summary>
        /// Transforms AOXML string into Vha.AOML.Builder.
        /// Please refer to AOXML.xsd for AOXML format details.
        /// </summary>
        /// <param name="aoxml">AOXML string</param>
        /// <returns>Vha.AOML.Builder that represents given AOXML string</returns>
        /// <exception cref="Vha.AOML.AOXMLException">
        ///     Thrown when an invalid AOXML string is passed as argument"
        /// </exception>
        public static Builder ToBuilder(String aoxml)
        {
            Builder builder = new Builder();
            ToBuilder(aoxml, builder);
            return builder;
        }

        /// <summary>
        /// Transforms AOXML string into Vha.AOML.Builder.
        /// Please refer to AOXML.xsd for AOXML format details.
        /// </summary>
        /// <param name="aoxml">AOXML string</param>
        /// <param name="builder">A Builder instance to which the output will be written</param>
        /// <exception cref="Vha.AOML.AOXMLException">
        ///     Thrown when an invalid AOXML string is passed as argument"
        /// </exception>
        public static void ToBuilder(String aoxml, Builder builder)
        {
            if (aoxml == null)
            {
                throw new ArgumentNullException("aoxml");
            }
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try
            {
                doc.LoadXml(aoxml);
            }
            catch (XmlException ex)
            {
                throw new AOXMLException("AOXML string is not a valid xml", ex);
            }
            FixAdjacentWhitespaces(FixBlockWhitespaces(doc.DocumentElement));
            ParseElement(doc.DocumentElement, builder);
        }

        private static List<XmlCharacterData> FixBlockWhitespaces(XmlElement root)
        {
            //all the leaf text elements in the curent block
            List<XmlCharacterData> textElements = new List<XmlCharacterData>();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode child = root.ChildNodes[i];
                if (child is XmlCharacterData && !(child is XmlComment))
                {
                    textElements.Add(child as XmlCharacterData);
                }
                else if (child is XmlElement)
                {
                    if ((child as XmlElement).LocalName == Window)
                    {
                        //special treatment for <window> ans it contains two children
                        XmlElement content = GetChild((child as XmlElement), Content);
                        if (content == null)
                        {
                            throw new AOXMLException(String.Format("'{0}' element without '{1}' element child",
                                                                   Window, Content));
                        }
                        FixAdjacentWhitespaces(FixBlockWhitespaces(content));
                        XmlElement link = GetChild((child as XmlElement), Link);
                        if (link == null)
                        {
                            throw new AOXMLException(String.Format("'{0}' element without '{1}' element child",
                                                                   Window, Link));
                        }
                        textElements.AddRange(FixBlockWhitespaces(link));
                    }
                    else
                    {
                        //we stumbled on a new blocks start
                        if (StartBlockElements.Contains((child as XmlElement).LocalName))
                        {
                            FixAdjacentWhitespaces(textElements);
                            FixAdjacentWhitespaces(FixBlockWhitespaces((child as XmlElement)));
                            textElements.Clear();
                        }
                        else
                        {
                            textElements.AddRange(FixBlockWhitespaces(child as XmlElement));
                        }
                    }
                }
            }
            return textElements;
        }

        private static void FixAdjacentWhitespaces(List<XmlCharacterData> textElements)
        {
            //normalize spaces inside text elements
            foreach (var text in textElements)
            {
                text.Value = text.Value.Replace('\r', '\n').Replace('\n', ' ');
                text.Value = whitespaceNormalizeRegex.Replace(text.Value, " ");
            }
            //normalize spaces spanning between text elements
            int lastNonEmpty = 0;
            for (int i = 1; i < textElements.Count; i++)
            {
                if (textElements[lastNonEmpty].Value.EndsWith(" "))
                {
                    textElements[i].Value = textElements[i].Value.TrimStart();
                }
                if (!String.IsNullOrEmpty(textElements[i].Value))
                {
                    lastNonEmpty = i;
                }
            }
            //fix is so there is no leading spaces
            for (int i = 0; i < textElements.Count; i++)
            {
                if (textElements[i].Value.StartsWith(" "))
                {
                    textElements[i].Value = textElements[i].Value.TrimStart();
                }
                if (!String.IsNullOrEmpty(textElements[i].Value))
                {
                    break;
                }
            }
            //fix that there are no trailing spaces
            for (int i = textElements.Count - 1; i >= 0; i--)
            {
                if (textElements[i].Value.EndsWith(" "))
                {
                    textElements[i].Value = textElements[i].Value.TrimEnd();
                }
                if (!String.IsNullOrEmpty(textElements[i].Value))
                {
                    break;
                }
            }
        }

        private static void ParseElement(XmlElement node, Builder builder, int windowNesting = 0)
        {
            String name = node.LocalName;
            bool isText = false;
            switch (name)
            {
                case Aoxml:
                    HandleAoxml(builder, windowNesting);
                    break;
                case Window:
                    node = HandleWindow(node, builder, windowNesting);
                    break;
                case Content:
                    HandleContent(node, builder, windowNesting);
                    break;
                case Underline:
                    builder.BeginUnderline();
                    break;
                case Italic:
                    builder.BeginItalic();
                    break;
                case Color:
                    HandleColor(node, builder);
                    break;
                case Image:
                    HandleImage(node, builder, windowNesting);
                    break;
                case Command:
                    HandleCommand(node, builder, windowNesting);
                    break;
                case Customlink:
                    HandleCustomlink(node, builder, windowNesting);
                    break;
                case Block:
                    HandleAlign(node, builder, windowNesting, Alignment.Inherit);
                    break;
                case Left:
                    HandleAlign(node, builder, windowNesting, Alignment.Left);
                    break;
                case Center:
                    HandleAlign(node, builder, windowNesting, Alignment.Center);
                    break;
                case Right:
                    HandleAlign(node, builder, windowNesting, Alignment.Right);
                    break;
                case Break:
                    builder.Break();
                    break;
                case Item:
                    HandleItem(node, builder);
                    break;
                case Entity:
                    HandleEntity(node, builder);
                    break;
                case User:
                    HandleUser(node, builder, windowNesting);
                    break;
                case Whitespace:
                    HandleWhitespace(node, builder);
                    break;
                case Text:
                    isText = true;
                    break;
                default:
                    throw new AOXMLException(String.Format("Encountered unknown element '{0}'",
                                                           name));
            }
            if (!EmptyElements.Contains(name))
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode child = node.ChildNodes[i];
                    if (child is XmlCharacterData && !(child is XmlComment))
                    {
                        if (!String.IsNullOrEmpty((child as XmlCharacterData).Value))
                        {
                            builder.Text((child as XmlCharacterData).Value);
                        }
                    }
                    else if (child is XmlElement)
                    {
                        if (isText)
                        {
                            throw new AOXMLException(String.Format("{0} element can only have text as child",
                                                                   Text));
                        }
                        ParseElement((child as XmlElement), builder, windowNesting);
                    }
                }
                if (!isText)
                {
                    builder.End();
                }
            }
            else if (node.ChildNodes.Count > 0)
            {
                throw new AOXMLException(String.Format("Element '{0}' cannot have any children",
                                                       name));
            }
        }

        private static void HandleAlign(XmlElement node, Builder builder, int nesting, Alignment alignment)
        {
            if (nesting == 0)
            {
                throw new AOXMLException(String.Format("'{0}' element can only be inside window",
                                                       node.LocalName));
            }
            builder.BeginAlign(alignment);
        }

        private static void HandleWhitespace(XmlElement node, Builder builder)
        {
            XmlAttribute count = node.Attributes[Count];
            uint value;
            if (count == null)
            {
                value = 1;
            }
            else if (!uint.TryParse(count.Value, out value))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Count, count.Value));
            }
            builder.Text(new string(' ', (int)value));
        }

        private static void HandleUser(XmlElement node, Builder builder, int nesting)
        {
            XmlAttribute name = node.Attributes[Name];
            if (name == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       User, Name));
            }
            builder.BeginLink(new UserLink(name.Value), GetStyle(node));
        }

        private static void HandleItem(XmlElement node, Builder builder)
        {
            XmlAttribute lowid = node.Attributes[Lowid];
            uint low, high, ql;
            if (lowid == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Item, Lowid));
            }
            else if (!uint.TryParse(lowid.Value, out low))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Lowid, lowid.Value));
            }
            XmlAttribute highid = node.Attributes[Highid];
            if (highid == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Item, Highid));
            }
            else if (!uint.TryParse(highid.Value, out high))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Highid, highid.Value));
            }
            XmlAttribute quality = node.Attributes[Quality];
            if (quality == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Item, Quality));
            }
            else if (!uint.TryParse(quality.Value, out ql))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Quality, quality.Value));
            }
            builder.BeginLink(new ItemLink(low, high, ql), GetStyle(node));
        }

        private static void HandleEntity(XmlElement node, Builder builder)
        {
            XmlAttribute typeid = node.Attributes[Typeid];
            uint type, instance;
            if (typeid == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Entity, Typeid));
            }
            else if (!uint.TryParse(typeid.Value, out type))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Typeid, typeid.Value));
            }
            XmlAttribute instanceid = node.Attributes[Instanceid];
            if (instanceid == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Entity, Instanceid));
            }
            else if (!uint.TryParse(instanceid.Value, out instance))
            {
                throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                       Instanceid, instanceid.Value));
            }
            builder.BeginLink(new EntityLink(type, instance), GetStyle(node));
        }

        private static void HandleCustomlink(XmlElement node, Builder builder, int nesting)
        {
            if (nesting == 0)
            {
                throw new AOXMLException(String.Format("'{0}' element can only be inside window",
                                                       Customlink));
            }
            XmlAttribute href = node.Attributes[Href];
            if (href == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Customlink, Href));
            }
            builder.BeginOtherLink(href.Value, GetStyle(node));
        }

        private static void HandleCommand(XmlElement node, Builder builder, int nesting)
        {
            if (nesting == 0)
            {
                throw new AOXMLException(String.Format("'{0}' element can only be inside window",
                                                       Command));
            }
            XmlAttribute value = node.Attributes[Value];
            if (value == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Command, Value));
            }
            builder.BeginCommandLink(value.Value, GetStyle(node));
        }

        private static void HandleImage(XmlElement node, Builder builder, int nesting)
        {
            if (nesting == 0)
            {
                throw new AOXMLException(String.Format("'{0}' element can only be inside window",
                                                       Image));
            }
            XmlAttribute source = node.Attributes[Source];
            if (source == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Image, Source));
            }
            if (source.Value.StartsWith(Rdb))
            {
                builder.Image(ImageType.RDB, source.Value.Substring(Rdb.Length));
            }
            else if (source.Value.StartsWith(Tdb))
            {
                builder.Image(ImageType.TDB, source.Value.Substring(Tdb.Length));
            }
            else
            {
                throw new AOXMLException(String.Format("Bad image source '{0}'",
                                                       source.Value));
            }
        }

        private static void HandleColor(XmlElement node, Builder builder)
        {
            XmlAttribute value = node.Attributes[Value];
            if (value == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' attribute",
                                                       Color, Value));
            }
            builder.BeginColor(value.Value);
        }

        private static void HandleContent(XmlElement node, Builder builder, int nesting)
        {
            if (nesting == 0 || node.ParentNode.Name != Window)
            {
                throw new AOXMLException(String.Format("'{0}' is only valid inside window",
                                                       Content));
            }
            builder.Begin();
        }

        private static XmlElement HandleWindow(XmlElement node, Builder builder, int nesting)
        {
            if (nesting >= 2)
            {
                throw new AOXMLException("Can only have windows nested up to depth of 2.");
            }
            XmlElement content = GetChild(node, Content);
            if (content == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' element child",
                                                       Window, Content));
            }
            Builder popupBuilder = new Builder();
            ParseElement(content, popupBuilder, nesting + 1);
            XmlElement link = GetChild(node, Link);
            if (link == null)
            {
                throw new AOXMLException(String.Format("'{0}' element without '{1}' element child",
                                                       Window, Link));
            }
            builder.BeginWindowLink(popupBuilder.Dominize(), GetStyle(link));
            node = link;
            return node;
        }

        private static void HandleAoxml(Builder builder, int nesting)
        {
            if (nesting != 0)
            {
                throw new AOXMLException(String.Format("'{0}' is only valid as root element",
                                                       Aoxml));
            }
            builder.Begin();
        }

        private static bool GetStyle(XmlElement node)
        {
            XmlAttribute style = node.Attributes[Style];
            if (style == null)
            {
                return true;
            }
            else
            {
                bool value;
                if (!bool.TryParse(style.Value, out value))
                {
                    throw new AOXMLException(String.Format("'{0}' attribute has invalid value {1}",
                                                           Style, style.Value));
                }
                return value;
            }
        }

        private static XmlElement GetChild(XmlElement node, string childName)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child is XmlElement && child.Name == childName)
                {
                    return child as XmlElement;
                }
            }
            return null;
        }
    }
}
