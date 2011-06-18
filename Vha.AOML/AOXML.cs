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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Vha.AOML.DOM;

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
        private const String User = "user";
        private const String Name = "name";
        private const String Style = "style";

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
            try
            {
                doc.LoadXml(aoxml);
            }
            catch (XmlException ex)
            {
                throw new AOXMLException("AOXML string is not a valid xml", ex);
            }
            ParseElement(doc.DocumentElement, builder, 0);
        }

        private static Builder ParseElement(XmlElement node, Builder builder, int nesting)
        {
            String name = node.LocalName;
            bool canHaveChildren = true;
            switch (name)
            {
                case Aoxml:
                    HandleAoxml(builder, nesting);
                    break;
                case Window:
                    node = HandleWindow(node, builder, nesting);
                    break;
                case Content:
                    HandleContent(node, builder, nesting);
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
                    HandleImage(node, builder, nesting);
                    canHaveChildren = false;
                    break;
                case Command:
                    HandleCommand(node, builder, nesting);
                    break;
                case Customlink:
                    HandleCustomlink(node, builder, nesting);
                    break;
                case Block:
                    builder.BeginAlign(Alignment.Inherit);
                    break;
                case Left:
                    builder.BeginAlign(Alignment.Left);
                    break;
                case Center:
                    builder.BeginAlign(Alignment.Center);
                    break;
                case Right:
                    builder.BeginAlign(Alignment.Right);
                    break;
                case Break:
                    builder.Break();
                    canHaveChildren = false;
                    break;
                case Item:
                    HandleItem(node, builder);
                    break;
                case User:
                    HandleUser(node, builder, nesting);
                    break;
                default:
                    throw new AOXMLException(String.Format("Encountered unknown element '{0}'",
                                                           name));
            }
            if (canHaveChildren)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child is XmlText)
                    {
                        builder.Text((child as XmlText).Value.Trim());
                    }
                    else if (child is XmlElement)
                    {
                        ParseElement((child as XmlElement), builder, nesting);
                    }
                }
                builder.End();
            }
            else if (node.ChildNodes.Count > 0)
            {
                throw new AOXMLException(String.Format("Element '{0}' cannot have any children",
                                                       name));
            }
            return builder;
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
