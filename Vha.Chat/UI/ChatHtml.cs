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
using Vha.Chat;

namespace Vha.Chat.UI
{
    public class ChatHtml
    {
        protected Context _context;
        protected ChatForm _form;

        public ChatHtml(Context context, ChatForm form)
        {
            this._context = context;
            this._form = form;
        }

        public void Link(string type, string argument)
        {
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
                    this._context.Write(MessageClass.Error, "Unknown link type: " + type);
                    break;
            }
        }

        protected void TextLink(string text)
        {
            InfoForm form = new InfoForm(this._context, this, text);
            Utils.InvokeShow(this._form, form);
        }
        
        protected void ChatCmdLink(string command)
        {
            this._context.Input.Command(command);
        }

        protected void ItemRefLink(string item)
        {
            //string url = "http://auno.org/ao/db.php?id={0}&id2={1}&ql={2}";
            string url = "http://www.xyphos.com/ao/aodb.php?id={0}&id2={1}&ql={2}&minimode=1";
            string[] parts = item.Split(new char[] {'/'});
            if (parts.Length < 3)
            {
                this._context.Write(MessageClass.Error, "Invalid itemref link: " + item);
                return;
            }
            Form form = new BrowserForm(this._context, string.Format(url, parts[0], parts[1], parts[2]));
            Utils.InvokeShow(this._form, form);
        }

        protected void CharacterLink(string character)
        {
            this._form.SetTarget(new MessageTarget(MessageType.Character, character));
        }

        protected void ChannelLink(string channel)
        {
            this._form.SetTarget(new MessageTarget(MessageType.Channel, channel));
        }

        protected void PrivateChannelLink(string channel)
        {
            this._form.SetTarget(new MessageTarget(MessageType.PrivateChannel, channel));
        }
    }
}
