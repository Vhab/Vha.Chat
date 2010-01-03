/*
* VhaBot.Chat
* Copyright (C) 2009 Remco van Oosterhout
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
using VhaBot.Net;
using VhaBot.Net.Events;
using VhaBot.Common;

namespace VhaBot.Chat
{
    public class ChatOutput
    {
        protected ChatForm _form;
        protected Net.Chat _chat;

        public ChatOutput(ChatForm form, Net.Chat chat)
        {
            this._form = form;
            this._chat = chat;
            // Hook events
            this._chat.ChannelMessageEvent += new ChannelMessageEventHandler(Chat_ChannelMessageEvent);
            this._chat.PrivateMessageEvent += new PrivateMessageEventHandler(Chat_PrivateMessageEvent);
            this._chat.PrivateChannelMessageEvent += new PrivateChannelMessageEventHandler(Chat_PrivateChannelMessageEvent);
            this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(Chat_PrivateChannelStatusEvent);
            this._chat.VicinityMessageEvent += new VicinityMessageEventHandler(Chat_VicinityMessageEvent);
        }

        private void Chat_ChannelMessageEvent(VhaBot.Net.Chat chat, ChannelMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"channel://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, e.Message);
            this._form.AppendLine(e.Type.ToString(), message);
        }

        private void Chat_PrivateMessageEvent(VhaBot.Net.Chat chat, PrivateMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
               e.Character, e.Message);
            if (e.Outgoing) message = "To " + message;
            this._form.AppendLine("PM", message);
        }

        void Chat_PrivateChannelMessageEvent(VhaBot.Net.Chat chat, PrivateChannelMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, e.Message);
            this._form.AppendLine("PG", message);
        }

        void Chat_PrivateChannelStatusEvent(VhaBot.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a> has {2} the channel",
                e.Channel, e.Character, e.Join ? "joined" : "left");
            this._form.AppendLine("PG", message);
        }

        private void Chat_VicinityMessageEvent(VhaBot.Net.Chat chat, VicinityMessageEventArgs e)
        {
            string message = string.Format(
               "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
              e.Character, e.Message);
            this._form.AppendLine("Vicinity", message);
        }
    }
}
