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

namespace Vha.Chat
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

        protected bool _checkConnection()
        {
            if (this._chat.State != Vha.Net.ChatState.Connected)
            {
                this._form.AppendLine("Error", "Not connected");
                return false;
            }
            return true;
        }

        protected bool _checkUser(string user)
        {
            if (!_checkConnection()) return false;
            if (this._chat.GetUserID(user) == 0)
            {
                this._form.AppendLine("Error", "Unknown user: " + user);
                return false;
            }
            return true;
        }

        protected bool _checkChannel(string channel)
        {
            if (!_checkConnection()) return false;
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
                    if (!_checkUser(target)) break;
                    this._chat.SendPrivateMessage(target, message);
                    break;
                case ChatInputType.Channel:
                    if (!_checkChannel(target)) break;
                    this._chat.SendChannelMessage(target, message);
                    break;
                case ChatInputType.PrivateChannel:
                    if (!_checkUser(target)) break;
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
            if (command.StartsWith("/"))
                command = command.Substring(1);
            string[] args = command.Split(' ');
            command = args[0];
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
                case "addbuddy":
                    AddBuddyCommand(args);
                    break;
                case "rembuddy":
                    RemBuddyCommand(args);
                    break;
                case "cc":
                    CCCommand(args);
                    break;
                case "o":
                    OrgCommand(args);
                    break;
                case "mute":
                    MuteCommand(args);
                    break;
                case "unmute":
                    UnmuteCommand(args);
                    break;
                case "whois":
                    WhoisCommand(args);
                    break;
                case "help":
                    HelpCommand();
                    break;
                case "ignore":
                    IgnoreCommand(args);
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
            if (!_checkUser(args[1])) return;
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
            if (!_checkUser(args[1])) return;
            this._form.AppendLine("PG", "Kicking " + args[1] + " from your private channel");
            this._chat.SendPrivateChannelKick(args[1]);
        }

        protected void KickAllCommand()
        {
            if (!_checkConnection()) return;
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
            if (!_checkUser(args[1])) return;
            this._chat.SendPrivateChannelLeave(args[1]);
        }

        protected void TellCommand(string[] args)
        {
            if (args.Length < 3)
            {
                this._form.AppendLine("Error", "Correct usage: /tell [username] [message]");
                return;
            }
            if (!_checkUser(args[1])) return;
            this._chat.SendPrivateMessage(args[1], string.Join(" ", args, 2, args.Length - 2));
        }

        protected void IgnoreCommand(string[] args)
        {
            if (Program.Ignores == null)
            {
                this._form.AppendLine("Error", "The ignore list hasn't been initialized yet, please be patient");
                return;
            }
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage:<br>/ignore [username]<br>/ignore list");
                return;
            }
            if (args[1] == "list")
            {
                string[] ignoredusers = Program.Ignores.ToNameArray();
                switch (ignoredusers.Length)
                {
                    case 0:
                        this._form.AppendLine("Internal", "There are no users on your ignore list");
                        break;
                    case 1:
                        this._form.AppendLine("Internal", "You have ignored " + ignoredusers[0] + "");
                        break;
                    default:
                        this._form.AppendLine("Internal", "You have ignored " + ignoredusers.Length.ToString() + " users:<br>- " + string.Join("<br>- ", ignoredusers));
                        break;
                }
            }
            else
            {
                if (!_checkUser(args[1])) return;
                string name = args[1].Substring(0, 1).ToUpper() + args[1].Substring(1).ToLower();
                uint uid = _chat.GetUserID(name);
                string action = string.Empty;
                if (Program.Ignores.Toggle(uid, name))
                    this._form.AppendLine("Internal", "Added " + name + " to the ignore list");
                else
                    this._form.AppendLine("Internal", "Removed " + name + " from the ignore list");
            }
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

        protected void AddBuddyCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /addbuddy [username]");
                return;
            }
            if (!_checkUser(args[1])) return;
            this._chat.SendFriendAdd(args[1]);
        }

        protected void RemBuddyCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /rembuddy [username]");
                return;
            }
            if (!_checkUser(args[1])) return;
            this._chat.SendFriendRemove(args[1]);
        }

        protected void CCCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /cc [command]");
                return;
            }
            List<string> newArgs = new List<string>(args);
            newArgs.RemoveAt(0);
            this._chat.SendChatCommand(newArgs.ToArray());
        }

        protected void OrgCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /o [message]");
                return;
            }
            if (!_checkConnection()) return;
            if (string.IsNullOrEmpty(this._chat.Organization))
            {
                this._form.AppendLine("Error", "This character does not belong to an organization");
                return;
            }
            this._chat.SendChannelMessage(this._chat.OrganizationID, string.Join(" ", args, 1, args.Length - 1));
        }

        protected void MuteCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /mute [channel]");
                return;
            }
            string channel = string.Join(" ", args, 1, args.Length - 1);
            if (!this._checkChannel(channel)) return;
            this._chat.SendChannelMute(channel, true);
        }

        protected void UnmuteCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /unmute [channel]");
                return;
            }
            string channel = string.Join(" ", args, 1, args.Length - 1);
            if (!this._checkChannel(channel)) return;
            this._chat.SendChannelMute(channel, false);
        }

        protected void WhoisCommand(string[] args)
        {
            if (args.Length < 2)
            {
                this._form.AppendLine("Error", "Correct usage: /whois [username]");
                return;
            }
            if (!_checkUser(args[1])) return;
            this.Command("/tell helpbot whois " + args[1]);
        }

        protected void HelpCommand()
        {
            this._form.AppendLine("Internal",
                "The following commands are available:<br>" +
                "/tell [username] [message]<br>" +
                "/leave [private channel]<br>" +
                "/invite [username]<br>" +
                "/kick [username]<br>" +
                "/kickall<br>" +
                "/addbuddy [username]<br>" +
                "/rembuddy [username]<br>" +
                "/o [message]<br>" +
                "/ignore [username]<br>" +
                "/ignore list<br>" +
                "/mute [channel]<br>" +
                "/unmute [channel]<br>" +
                "/cc [command]<br>" +
                "/about");
        }
    }
}
