/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Vha.Chat
{
    public class ChatHtml
    {
        public string Template
        {
            get { return Properties.Resources.Chat; }
        }

        protected ChatForm _form;
        protected ChatInput _inputUtil;
        protected Net.Chat _chat;
        protected Dictionary<string, string> _texts;
        protected int _textsIndex = 0;
        protected Regex _textsRegex = null;
        protected Regex _charrefRegex = null;
        protected Regex _colorRegex = null;

        public ChatHtml(ChatForm form, ChatInput input, Net.Chat chat)
        {
            this._form = form;
            this._chat = chat;
            this._inputUtil = input;
            this._texts = new Dictionary<string, string>();
        }

        public string InvertColors(string html)
        {
            if (this._colorRegex == null)
                this._colorRegex = new Regex("color=(['\"]?)#([0-9a-fA-F]{6})\\1");
            MatchCollection matches = this._colorRegex.Matches(html);
            foreach (Match match in matches)
            {
                string seperator = match.Groups[1].Value;
                string color = match.Groups[2].Value;
                int r = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int g = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int b = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                string inverse = string.Format("{0:X2}{1:X2}{2:X2}", 255 - r, 255 - g, 255 - b);
                string replacement = string.Format("color={0}#{1}{0}", seperator, inverse);
                html = html.Substring(0, match.Index) + replacement + html.Substring(match.Index + replacement.Length);
            }
            return html;
        }

        public void SecureHtml(HtmlDocument document, HtmlElement element)
        {
            // Early out
            if (element.FirstChild == null) return;
            // Constraints
            List<string> validTags = new List<string>();
            validTags.Add("pre");
            validTags.Add("span");
            validTags.Add("div");
            validTags.Add("font");
            validTags.Add("a");
            validTags.Add("br");
            List<string> validAttributes = new List<string>();
            validAttributes.Add("style");
            validAttributes.Add("align");
            validAttributes.Add("className");
            validAttributes.Add("color");
            List<string> validHrefs = new List<string>();
            validHrefs.Add("text://");
            validHrefs.Add("itemref://");
            validHrefs.Add("channel://");
            validHrefs.Add("privchan://");
            validHrefs.Add("character://");
            validHrefs.Add("chatcmd://");
            // Gather all elements
            List<HtmlElement> elements = new List<HtmlElement>();
            Stack<HtmlElement> todo = new Stack<HtmlElement>();
            todo.Push(element.FirstChild);
            while (todo.Count > 0)
            {
                HtmlElement current = todo.Pop();
                while (current != null)
                {
                    HtmlElement next = current.NextSibling;
                    // Check type against white-list
                    string tag = current.TagName.ToLower();
                    if (!validTags.Contains(tag))
                    {
                        if (current.InnerHtml != null)
                            current.InnerHtml = "";
                        current.OuterHtml = "";
                    }
                    else
                    {
                        // Queue children for processing
                        if (current.FirstChild != null)
                        {
                            todo.Push(current.FirstChild);
                        }
                    }
                    elements.Add(current);
                    current = next;
                }
            }
            // Process elements
            while (elements.Count > 0)
            {
                // Get the last one
                HtmlElement current = elements[elements.Count - 1];
                elements.RemoveAt(elements.Count - 1);
                HtmlElement replacement = null;
                // Special case for fonts
                if (current.TagName.ToLower() == "font")
                {
                    replacement = document.CreateElement("span");
                    replacement.Style = "color: " + current.GetAttribute("color");
                }
                // Default case
                {
                    // Recreate element with white-listed attributes
                    replacement = document.CreateElement(current.TagName);
                    // - Copy over white-listed attributes
                    foreach (string attribute in validAttributes)
                    {
                        string value = current.GetAttribute(attribute);
                        if (string.IsNullOrEmpty(value)) continue;
                        replacement.SetAttribute(attribute, value);
                    }
                    // - Copy over white-listed href
                    string href = current.GetAttribute("href");
                    if (!string.IsNullOrEmpty(href))
                    {
                        foreach (string validHref in validHrefs)
                        {
                            if (href.ToLower().StartsWith(validHref))
                            {
                                replacement.SetAttribute("href", href);
                                break;
                            }
                        }
                    }
                }
                if (current.InnerText != null)
                {
                    replacement.InnerHtml = current.InnerHtml;
                }
                current.OuterHtml = replacement.OuterHtml;
                // Clean up
                replacement.OuterHtml = "";
                replacement = null;
            }
        }

        public void AppendHtml(HtmlDocument document, string html) { AppendHtml(document, html, false); }
        public void AppendHtml(HtmlDocument document, string html, bool oneElement)
        {
            if (document.Body == null)
                return;
            // Create regex parser
            if (this._charrefRegex == null)
                this._charrefRegex = new Regex("charref://[0-9]+/[0-9]+/");
            if (this._textsRegex == null)
                this._textsRegex = new Regex("href=(\"|')text://([^\\1]*?)\\1");
            // Some preprocessing
            html = html.Replace("\n", "<br>");
            html = this._charrefRegex.Replace(html, "text://");
            // Find text:// links and strip them out
            MatchCollection matches = this._textsRegex.Matches(html);
            foreach (Match match in matches)
            {
                // Store text
                this._textsIndex++;
                if (this._textsIndex > Program.MaximumTexts) this._textsIndex = 0; // Keep only 50 texts
                this._texts[this._textsIndex.ToString()] = match.Groups[2].Value;
                // Replace link
                string seperator = match.Groups[1].Value;
                string replacement = string.Format("href={0}text://{1}{0}", seperator, this._textsIndex);
                html = html.Replace(match.Groups[0].Value, replacement);
            }
            // Some hardcore cheating
            HtmlElement tag = document.CreateElement("div");
            // - Without the pre tags, double whitespaces will be stripped
            tag.InnerHtml = "<pre>" + html + "</pre>";
            // - FirstChild is our <pre> tag, we replace double whitespaces here to make them visible
            tag.FirstChild.InnerHtml = tag.FirstChild.InnerHtml.Replace("  ", "&nbsp; ");
            // - Final round of html fixing
            SecureHtml(document, tag.FirstChild);
            // - OneElement assumes the given html contains only a single element and ensures on a single element is added
            if (oneElement) html = tag.FirstChild.FirstChild.OuterHtml;
            else html = tag.FirstChild.InnerHtml;
            // Fill content
            document.Body.InnerHtml += html;
            // Process all links
            /*HtmlElement[] links = GetElements(document.Body, "a");
            foreach (HtmlElement link in links)
            {
                // Hook click event
                //link.Click += new HtmlElementEventHandler(Clicked);
                link.SetAttribute("title", link.GetAttribute("href"));
                // Handle FC's 'no decoration' style
                if (link.Style == null || link.Style == "") continue;
                if (link.Style.ToLower().Replace(" ", "").Contains("text-decoration:none"))
                {
                    HtmlElement parent = link.Parent;
                    string col = "";
                    while (parent != null)
                    {
                        if (parent.TagName.ToLower() == "font")
                        {
                            col = parent.GetAttribute("color");
                            if (col != "") break;
                        }
                        parent = parent.Parent;
                    }
                    if (col != "")
                    {
                        link.Style = "color: " + col + ";";
                        link.SetAttribute("className", "NoStyle");
                    }
                }
            }*/
            // Click handler
            document.Click -= new HtmlElementEventHandler(Clicked);
            document.Click += new HtmlElementEventHandler(Clicked);
        }

        protected HtmlElement[] GetElements(HtmlElement element, string tag)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            Stack<HtmlElement> remaining = new Stack<HtmlElement>();
            remaining.Push(element);
            while (remaining.Count > 0)
            {
                HtmlElement current = remaining.Pop();
                // Queue child if any
                foreach (HtmlElement child in current.Children)
                {
                    remaining.Push(child);
                }
                if (current.TagName.ToLower() == tag.ToLower())
                    list.Add(current);
            }
            return list.ToArray();
        }

        protected void Clicked(object sender, HtmlElementEventArgs e)
        {
            //HtmlElement element = (HtmlElement)sender;
            HtmlDocument document = (HtmlDocument)sender;
            HtmlElement element = document.GetElementFromPoint(e.ClientMousePosition);
            if (element == null || element.TagName.ToLower() != "a") return;
            // Get href
            string href = element.GetAttribute("href").Trim('/');
            if (string.IsNullOrEmpty(href)) return;
            Link(href);
        }

        public void Link(string link)
        {
            // Parse href
            int index = link.IndexOf("://");
            if (index <= 0) return;
            string type = link.Substring(0, index);
            string argument = link.Substring(index + 3);
            // Handle href
            switch (type)
            {
                case "text":
                    TextLink(argument);
                    break;
                case "chatcmd":
                    ChatCmdLink(argument);
                    break;
                case "itemref":
                    ItemRefLink(argument);
                    break;
                case "character":
                    CharacterLink(argument);
                    break;
                case "channel":
                    ChannelLink(argument);
                    break;
                case "privchan":
                    PrivateChannelLink(argument);
                    break;
                default:
                    this._form.AppendLine("Error", "Unknown link type: " + type);
                    break;
            }
        }

        protected void TextLink(string text)
        {
            if (!this._texts.ContainsKey(text))
            {
                this._form.AppendLine("Error", "Unable to locate text with id " + text);
                return;
            }
            InfoForm form = new InfoForm(this, this._texts[text]);
            FormUtils.InvokeShow(this._form, form);
        }
        
        protected void ChatCmdLink(string command)
        {
            this._inputUtil.Command(command);
        }

        protected void ItemRefLink(string item)
        {
            string url = "http://auno.org/ao/db.php?id={0}&id2={1}&ql={2}";
            string[] parts = item.Split(new char[] {'/'});
            if (parts.Length < 3)
            {
                this._form.AppendLine("Error", "Invalid itemref link: " + item);
                return;
            }
            Form form = new BrowserForm(string.Format(url, parts[0], parts[1], parts[2]));
            FormUtils.InvokeShow(this._form, form);
        }

        protected void CharacterLink(string character)
        {
            this._form.SetTarget(ChatInputType.Character, character);
        }

        protected void ChannelLink(string channel)
        {
            this._form.SetTarget(ChatInputType.Channel, channel);
        }

        protected void PrivateChannelLink(string channel)
        {
            this._form.SetTarget(ChatInputType.PrivateChannel, channel);
        }
    }
}
