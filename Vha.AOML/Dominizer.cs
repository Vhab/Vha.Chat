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
using System.Text.RegularExpressions;
using Vha.AOML.DOM;
using Vha.Common;

namespace Vha.AOML
{
    /// <summary>
    /// This class allows you to transform a string of AOML into a DOM.Element tree
    /// </summary>
    public class Dominizer
    {
        public Parser Parser { get; private set; }
        
        #region Constructors
        public Dominizer()
        {
            this.Parser = new Parser();
            this.Parser.Mode = ParserMode.Compatibility;
            this.Parser.NewlineToBreak = true;
            this.Parser.InvalidElementsToContent = false;
        }
        #endregion

        public Element Parse(string aoml)
        {
            ContainerElement container = new ContainerElement();
            // Parse AOML
            NodeCollection nodes = this.Parser.Parse(aoml);
            this.Parser.Sanitize(nodes);
            this.Parser.Balance(nodes);
            // Transform AOML into a DOM tree
            Stack<Element> elements = new Stack<Element>();
            elements.Push(container);
            foreach (Node node in nodes)
            {
                Element element = null;
                switch (node.Type)
                {
                    case NodeType.Content:
                        // Content doesn't introduce a new level
                        // We'll simply add it as child at the current depth
                        ContentNode content = (ContentNode)node;
                        element = new TextElement(Web.UnescapeHtml(content.Value));
                        elements.Peek().Children.Add(element);
                        break;
                    case NodeType.Open:
                        OpenNode open = (OpenNode)node;
                        switch (open.Name)
                        {
                            case "br":
                                // Singular linebreak element
                                element = new BreakElement();
                                elements.Peek().Children.Add(element);
                                break;
                            case "img":
                                // Singular image element
                                element = this.CreateImageElement(open);
                                if (element != null)
                                    elements.Peek().Children.Add(element);
                                break;
                            case "font":
                                element = this.CreateFontElement(open);
                                elements.Peek().Children.Add(element);
                                if (!open.Closed) { elements.Push(element); }
                                break;
                            case "a":
                                element = this.CreateLinkElement(open);
                                elements.Peek().Children.Add(element);
                                if (!open.Closed) { elements.Push(element); }
                                break;
                            case "u":
                                element = new UnderlineElement();
                                elements.Peek().Children.Add(element);
                                if (!open.Closed) { elements.Push(element); }
                                break;
                            case "i":
                                element = new ItalicElement();
                                elements.Peek().Children.Add(element);
                                if (!open.Closed) { elements.Push(element); }
                                break;
                            case "center":
                            case "left":
                            case "right":
                            case "div":
                                Alignment alignment = Alignment.Inherit;
                                if (open.Name == "center" || open.GetAttribute("align") == "center")
                                {
                                    alignment = Alignment.Center;
                                }
                                else if (open.Name == "left" || open.GetAttribute("align") == "left")
                                {
                                    alignment = Alignment.Left;
                                }
                                else if (open.Name == "right" || open.GetAttribute("align") == "right")
                                {
                                    alignment = Alignment.Right;
                                }
                                element = new AlignElement(alignment);
                                elements.Peek().Children.Add(element);
                                if (!open.Closed) { elements.Push(element); }
                                break;
                            default:
                                throw new ArgumentException("Unexpected tag: " + open.Name);
                        }
                        break;
                    case NodeType.Close:
                        // Closing a node means consuming an element from the stack
                        // But first check whether the node matches the element on the stack
                        CloseNode close = (CloseNode)node;
                        switch (close.Name)
                        {
                            case "br":
                                throw new ArgumentException("Unexpected 'br' closing tag");
                            case "img":
                                throw new ArgumentException("Unexpected 'img' closing tag");
                            case "font":
                                if (elements.Peek().Type == ElementType.Color) { break; }
                                if (elements.Peek().Type == ElementType.Container) { break; }
                                throw new ArgumentException("Unexpected 'font' closing tag");
                            case "a":
                                if (elements.Peek().Type == ElementType.Link) { break; }
                                if (elements.Peek().Type == ElementType.Container) { break; }
                                throw new ArgumentException("Unexpected 'a' closing tag");
                            case "u":
                                if (elements.Peek().Type == ElementType.Underline) { break; }
                                throw new ArgumentException("Unexpected 'u' closing tag");
                            case "i":
                                if (elements.Peek().Type == ElementType.Italic) { break; }
                                throw new ArgumentException("Unexpected 'i' closing tag");
                            case "div":
                            case "center":
                            case "left":
                            case "right":
                                if (elements.Peek().Type == ElementType.Align) { break; }
                                throw new ArgumentException("Unexpected '" + close.Name + "' closing tag");
                            default:
                                throw new ArgumentException("Unexpected tag: " + close.Name);
                        }
                        if (elements.Count <= 1)
                        {
                            throw new ArgumentException("Unexpected closing tag");
                        }
                        // Go 1 step back down in the tree
                        elements.Pop();
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected node type");
                }
            }
            // Let's report our progress
            if (container.Children.Count == 0)
            {
                return null;
            }
            if (container.Children.Count == 1)
            {
                return container.Children.ToArray()[0];
            }
            return container;
        }

       

        protected Element CreateImageElement(OpenNode node)
        {
            // Valid image?
            string src = node.GetAttribute("src");
            if (string.IsNullOrEmpty(src)) { return null; }
            if (!src.Contains("://")) { return null; }
            // Parse src value
            string[] parts = src.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) { return null; }
            // - Determine type
            ImageType type = ImageType.RDB;
            if (parts[0].ToLower() == "rdb")
            {
                type = ImageType.RDB;
            }
            else if (parts[0].ToLower() == "tdb")
            {
                type = ImageType.TDB;
                string prefix = "id:";
                if (parts[1].StartsWith(prefix))
                {
                    parts[1] = parts[1].Substring(prefix.Length);
                }
            }
            else { return null; }
            // Return image
            return new ImageElement(type, parts[1]);
        }

        protected Element CreateFontElement(OpenNode node)
        {
            string color = node.GetAttribute("color");
            if (string.IsNullOrEmpty(color)) { return new ContainerElement(); }
            Color c = Color.FromString(color);
            if (c == null) { return new ContainerElement(); }
            return new ColorElement(c);
        }

        protected Element CreateLinkElement(OpenNode node)
        {
            // Valid link?
            string href = node.GetAttribute("href");
            if (string.IsNullOrEmpty(href) || !href.Contains("://"))
            {
                return new ContainerElement();
            }
            // Parse link
            int index = href.IndexOf("://");
            if (index <= 0)
            {
                return new ContainerElement();
            }
            string type = href.Substring(0, index).ToLower();
            string argument = href.Substring(index + 3);
            Link link = null;
            // Verify header
            Regex validCharacters = new Regex("^[a-zA-Z]+$");
            if (!validCharacters.IsMatch(type))
            {
                // Invalid link
                link = new InvalidLink(href);
            }
            else
            {
                // Generate link
                switch (type)
                {
                    case "text":
                        link = new WindowLink(Parse(argument));
                        break;
                    case "charref":
                        index = argument.IndexOf("/");
                        if (index < 0) { break; }
                        index = argument.IndexOf("/", index + 1);
                        if (index <= 0) { break; }
                        argument = argument.Substring(index + 1);
                        link = new WindowLink(Parse(argument));
                        break;
                    case "itemref":
                        string[] parts = argument.Split('/');
                        if (parts.Length != 3) break;
                        uint lowId, highId, ql;
                        if (!uint.TryParse(parts[0], out lowId)) { break; }
                        if (!uint.TryParse(parts[1], out highId)) { break; }
                        if (!uint.TryParse(parts[2], out ql)) { break; }
                        link = new ItemLink(lowId, highId, ql);
                        break;
                    case "chatcmd":
                        link = new CommandLink(argument);
                        break;
                    case "user":
                        link = new UserLink(argument);
                        break;
                    default:
                        try
                        {
                            // Attempt to create an 'OtherLink'
                            // this operation will throw an exception if the 'href' is significantly malformed
                            link = new OtherLink(href);
                        }
                        catch (UriFormatException)
                        {
                            // The 'href' is malformed, store as InvalidLink
                            link = new InvalidLink(href);
                        }
                        break;
                }
            }
            if (link == null)
            {
                return new ContainerElement();
            }
            string style = node.GetAttribute("style");
            if (style != null)
            {
                style = style.Replace(" ", "").ToLower();
                if (!style.Contains("text-decoration:none"))
                {
                    style = null;
                }
            }
            return new LinkElement(link, style == null);
        }
    }
}
