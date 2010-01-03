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

namespace VhaBot.Chat
{
    public enum ChatInputType
    {
        None,
        Character,
        Channel,
        PrivateChannel
    }

    public class ChatTarget
    {
        public readonly ChatInputType Type;
        public readonly string Target;

        public ChatTarget(ChatInputType type, string target)
        {
            this.Type = type;
            this.Target = target;
        }

        public override string ToString()
        {
            return this.Target;
        }

        public bool Equals(ChatTarget target)
        {
            if (this.Type != target.Type) return false;
            if (this.Target != target.Target) return false;
            return true;
        }
    }

    public class ChatInput
    {
        protected ChatForm _form;
        protected Net.Chat _chat;

        public ChatInput(ChatForm form, Net.Chat chat)
        {
            this._form = form;
            this._chat = chat;
        }

        protected bool CheckUser(string user)
        {
            if (this._chat.GetUserID(user) == 0)
            {
                this._form.AppendLine("Error", "Unknown user: " + user);
                return false;
            }
            return true;
        }

        protected bool CheckChannel(string channel)
        {
            if (this._chat.GetChannelID(channel) == 0)
            {
                this._form.AppendLine("Error", "Unknown channel: " + channel);
                return false;
            }
            return true;
        }

        public void Send(ChatInputType type, string target, string message)
        {
            // Empty message
            if (message.Length == 0)
                return;
            // Regular message
            switch (type)
            {
                case ChatInputType.None: // Echo
                    this._form.AppendLine("Text", message);
                    break;
                case ChatInputType.Character:
                    if (!CheckUser(target)) break;
                    this._chat.SendPrivateMessage(target, message);
                    break;
                case ChatInputType.Channel:
                    if (!CheckChannel(target)) break;
                    this._chat.SendChannelMessage(target, message);
                    break;
                case ChatInputType.PrivateChannel:
                    if (!CheckUser(target)) break;
                    this._chat.SendPrivateChannelMessage(target, message);
                    break;
            }
        }

        public void Command(string command)
        {
            // Empty message
            if (command.Length == 0)
                return;
            // Split
            string[] args = command.Split(' ');
            command = args[0];
            if (command.StartsWith("/"))
                command = command.Substring(1);
            // Handle
            Command(command, args);
        }

        public void Command(string command, string[] args)
        {
            switch (command)
            {
                case "invite":
                    InviteCommand(args);
                    break;
                case "kick":
                    KickCommand(args);
                    break;
                case "kickall":
                    KickAllCommand();
                    break;
                case "leave":
                    LeaveCommand(args);
                    break;
                case "tell":
                    TellCommand(args);
                    break;
                case "about":
                    AboutCommand();
                    break;
                case "text":
                    TextCommand(args);
                    break;
                default:
                    this._form.AppendLine("Error", "Unknown command: /" + command);
                    break;
            }
        }

        protected void InviteCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /invite [username]");
                return;
            }
            if (!CheckUser(args[1])) return;
            this._form.AppendLine("PG", "Inviting " + args[1] + " to your private channel");
            this._chat.SendPrivateChannelInvite(args[1]);
        }

        protected void KickCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /kick [username]");
                return;
            }
            if (!CheckUser(args[1])) return;
            this._form.AppendLine("PG", "Kicking " + args[1] + " from your private channel");
            this._chat.SendPrivateChannelKick(args[1]);
        }

        protected void KickAllCommand()
        {
            this._form.AppendLine("PG", "Kicking all users from your private channel");
            this._chat.SendPrivateChannelKickAll();
        }

        protected void LeaveCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /leave [private channel]");
                return;
            }
            if (!CheckUser(args[1])) return;
            this._chat.SendPrivateChannelLeave(args[1]);
        }

        protected void TellCommand(string[] args)
        {
            if (args.Length < 3)
            {
                this._form.AppendLine("Error", "Correct usage: /tell [username] [message]");
                return;
            }
            if (!CheckUser(args[1])) return;
            this._chat.SendPrivateMessage(args[1], string.Join(" ", args, 2, args.Length - 2));
        }

        delegate void AboutCommandDelegate();
        protected void AboutCommand()
        {
            if (this._form.InvokeRequired)
            {
                this._form.BeginInvoke(new AboutCommandDelegate(AboutCommand));
                return;
            }
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        protected void TextCommand(string[] args)
        {
            this._form.AppendLine("Internal", string.Join(" ", args, 1, args.Length - 1));
        }
    }
}
