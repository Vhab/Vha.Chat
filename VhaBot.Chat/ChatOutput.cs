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
            this._chat.ChannelMessageEvent += new ChannelMessageEventHandler(_chat_ChannelMessageEvent);
            this._chat.PrivateMessageEvent += new PrivateMessageEventHandler(_chat_PrivateMessageEvent);
            this._chat.PrivateChannelMessageEvent += new PrivateChannelMessageEventHandler(_chat_PrivateChannelMessageEvent);
            this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent);
            this._chat.VicinityMessageEvent += new VicinityMessageEventHandler(_chat_VicinityMessageEvent);
            this._chat.StatusChangeEvent += new StatusChangeEventHandler(_chat_StatusChangeEvent);
        }

        private void _chat_ChannelMessageEvent(VhaBot.Net.Chat chat, ChannelMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"channel://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, e.Message);
            this._form.AppendLine(e.Type.ToString(), message);
        }

        private void _chat_PrivateMessageEvent(VhaBot.Net.Chat chat, PrivateMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
               e.Character, e.Message);
            if (e.Outgoing) message = "To " + message;
            this._form.AppendLine("PM", message);
        }

        void _chat_PrivateChannelMessageEvent(VhaBot.Net.Chat chat, PrivateChannelMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, e.Message);
            this._form.AppendLine("PG", message);
        }

        void _chat_PrivateChannelStatusEvent(VhaBot.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a> has {2} the channel",
                e.Channel, e.Character, e.Join ? "joined" : "left");
            this._form.AppendLine("PG", message);
        }

        private void _chat_VicinityMessageEvent(VhaBot.Net.Chat chat, VicinityMessageEventArgs e)
        {
            string message = string.Format(
               "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
              e.Character, e.Message);
            this._form.AppendLine("Vicinity", message);
        }

        private void _chat_StatusChangeEvent(VhaBot.Net.Chat chat, StatusChangeEventArgs e)
        {
            this._form.AppendLine("Error", "State changed to: " + e.State.ToString());
        }
    }
}
