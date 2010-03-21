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
using Vha;
using Vha.Net;
using Vha.Net.Events;
using Vha.Common;

namespace Vha.Chat
{
    public class ChatOutput
    {
        protected ChatForm _form;
        protected Net.Chat _chat;
        protected List<string> _ignoredChannels;
        /// <summary>
        /// List containing uids of private channels I am in.
        /// </summary>
        protected List<uint> _privateChannels = new List<uint>();

        public ChatOutput(ChatForm form, Net.Chat chat)
        {
            this._form = form;
            this._chat = chat;
            this._ignoredChannels = new List<string>();
            // Hook events
            this._chat.ChannelJoinEvent += new ChannelJoinEventHandler(_chat_ChannelJoinEvent);
            this._chat.ChannelMessageEvent += new ChannelMessageEventHandler(_chat_ChannelMessageEvent);
            this._chat.PrivateMessageEvent += new PrivateMessageEventHandler(_chat_PrivateMessageEvent);
            this._chat.PrivateChannelMessageEvent += new PrivateChannelMessageEventHandler(_chat_PrivateChannelMessageEvent);
            this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent);
            this._chat.VicinityMessageEvent += new VicinityMessageEventHandler(_chat_VicinityMessageEvent);
            this._chat.StatusChangeEvent += new StatusChangeEventHandler(_chat_StatusChangeEvent);
            this._chat.SystemMessageEvent += new SystemMessageEventHandler(_chat_SystemMessageEvent);
            this._chat.SimpleMessageEvent += new SimpleMessageEventHandler(_chat_SimpleMessageEvent);
        }

        private void _chat_ChannelJoinEvent(Vha.Net.Chat chat, ChannelJoinEventArgs e)
        {
            if (e.Muted)
            {
                if (!this._ignoredChannels.Contains(e.Name))
                {
                    this._ignoredChannels.Add(e.Name);
                }
            }
            else
            {
                if (this._ignoredChannels.Contains(e.Name))
                {
                    this._ignoredChannels.Remove(e.Name);
                }
            }
        }

        private void _chat_ChannelMessageEvent(Vha.Net.Chat chat, ChannelMessageEventArgs e)
        {
            // Check if channel is muted
            if (this._ignoredChannels.Contains(e.Channel)) return;
            // Process message
            string message = e.Message;
            if (e.Message.StartsWith("~"))
            {
                MDB.Message parsedMessage = null;
                try { parsedMessage = MDB.Parser.Decode(e.Message); }
                catch { }
                if (parsedMessage != null && !string.IsNullOrEmpty(parsedMessage.Value))
                    message = parsedMessage.Value;
            }
            string line = string.Format(
                "[<a href=\"channel://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, message);
            this._form.AppendLine(e.Type.ToString(), line);
        }

        private void _chat_PrivateMessageEvent(Vha.Net.Chat chat, PrivateMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
               e.Character, e.Message);
            if (e.Outgoing) message = "To " + message;
            this._form.AppendLine("PM", message);
        }

        void _chat_PrivateChannelMessageEvent(Vha.Net.Chat chat, PrivateChannelMessageEventArgs e)
        {
            string message = string.Format(
                "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a>: {2}",
                e.Channel, e.Character, e.Message);
            this._form.AppendLine("PG", message);
        }

        void _chat_PrivateChannelStatusEvent(Vha.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            // Check if we *should* report this message at all.
            bool report = true;
            if (e.Join && e.CharacterID == chat.ID)
            {
                if (this._privateChannels.Contains(e.ChannelID))
                    report = false;
                else
                    this._privateChannels.Add(e.ChannelID);
            }
            else if (e.CharacterID == chat.ID)
            {
                if (!this._privateChannels.Contains(e.ChannelID)) // We're not in this channel
                    report = false;
                else
                    this._privateChannels.Remove(e.ChannelID);
            }
            if (report)
            {
                string message = string.Format(
                    "[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] <a href=\"character://{1}\" class=\"Link\">{1}</a> has {2} the channel",
                    e.Channel, e.Character, e.Join ? "joined" : "left");
                this._form.AppendLine("PG", message);
            }
        }

        private void _chat_VicinityMessageEvent(Vha.Net.Chat chat, VicinityMessageEventArgs e)
        {
            string message = string.Format(
               "[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: {1}",
              e.Character, e.Message);
            this._form.AppendLine("Vicinity", message);
        }

        private void _chat_StatusChangeEvent(Vha.Net.Chat chat, StatusChangeEventArgs e)
        {
            this._form.AppendLine("Error", "State changed to: " + e.State.ToString());
        }

        private void _chat_SystemMessageEvent(Vha.Net.Chat chat, SystemMessageEventArgs e)
        {
            MDB.Reader reader = new MDB.Reader();
            MDB.Entry entry = reader.SpeedRead((int)e.CategoryID, (int)e.MessageID);
            // Failed to get the entry
            if (entry == null)
            {
                this._form.AppendLine("System", "Unknown system message " + e.MessageID);
                return;
            }
            // Format message
            string template = MDB.Parser.PrintfToFormatString(entry.Message);
            string message = string.Format(template, e.Arguments);
            this._form.AppendLine("System", message);
        }

        private void _chat_SimpleMessageEvent(Vha.Net.Chat chat, SimpleMessageEventArgs e)
        {
            this._form.AppendLine("System", e.Message);
        }
    }
}
