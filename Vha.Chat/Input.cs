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
using Vha.Chat.Commands;
using Vha.Common;

namespace Vha.Chat
{
    public class Input
    {
        public string Prefix { get { return this._prefix; } }

        #region Input sanity commands
        public bool CheckArguments(string trigger, int count, int minimum, bool output)
        {
            if (count >= minimum) return true;
            if (output)
            {
                string message = "";
                if (!this.HasTrigger(trigger))
                {
                    // Generic response
                    string arguments = minimum == 1 ? "argument" : "arguments";
                    message = "Expecting at least " + minimum + " " + arguments;
                }
                else
                {
                    Command c = this.GetCommandByTrigger(trigger);
                    // Command specific response
                    message = "Expected usage: ";
                    foreach (string usage in c.Usage)
                        message += "\n" + this.Prefix + usage;
                }
                message += string.Format("\nUse '{0}help {1}' for more information", this.Prefix, trigger);
                this._context.Write(MessageClass.Error, message);
            }
            return false;
        }

        public bool CheckConnection(bool output)
        {
            if (this._context.State == ContextState.Connected) return true;
            if (output)
                this._context.Write(MessageClass.Error, "Not connected");
            return false;
        }

        public bool CheckOrganization(bool output)
        {
            if (this._context.HasOrganization) return true;
            if (output)
                this._context.Write(MessageClass.Error, "Not a member of an organization");
            return false;
        }

        public bool CheckCharacter(string character, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.Chat.GetCharacterID(character) != 0) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown character: " + Format.UppercaseFirst(character));
            }
            return false;
        }

        public bool CheckChannel(string channel, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.HasChannel(channel)) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown channel: " + channel);
            }
            return false;
        }

        public bool CheckPrivateChannel(string channel, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (this._context.HasPrivateChannel(channel)) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, "Unknown private channel: " + channel);
            }
            return false;
        }

        public bool CheckIgnore(string character, bool output)
        {
            if (!CheckConnection(output)) return false;
            if (!this._context.Ignores.Contains(character)) return true;
            if (output)
            {
                this._context.Write(MessageClass.Error, Format.UppercaseFirst(character) + " is on your ignore list");
            }
            return false;
        }
        #endregion

        #region Commands for sending message or command input
        public bool Send(MessageTarget target, string message, bool allowCommands)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            // Check for empty messages
            if (message.Trim().Length == 0)
            {
                this._context.Write(MessageClass.Error, "Can't send an empty message");
                return false;
            }
            // Check for commands
            if (message.StartsWith(this.Prefix) && allowCommands)
            {
                return this.Command(message);
            }
            // Handle sending
            switch (target.Type)
            {
                case MessageType.Character:
                    if (!this.CheckCharacter(target.Target, true)) return false;
                    if (!this.CheckIgnore(target.Target, true)) return false;
                    this._context.Chat.SendPrivateMessage(target.Target, message);
                    break;
                case MessageType.Channel:
                    if (!this.CheckChannel(target.Target, true)) return false;
                    this._context.Chat.SendChannelMessage(target.Target, message);
                    break;
                case MessageType.PrivateChannel:
                    if (!this.CheckPrivateChannel(target.Target, true)) return false;
                    this._context.Chat.SendPrivateChannelMessage(target.Target, message);
                    break;
                case MessageType.None:
                    this._context.Write(MessageClass.Text, message);
                    break;
            }
            return true;
        }

        public bool Command(string command)
        {
            // Clean up the command
            command = command.TrimEnd();
            if (command.Length == 0) return false;
            if (command.StartsWith(this.Prefix))
                command = command.Substring(1);
            // Break up the command
            List<string> args = new List<string>(command.Split(' '));
            string message = "";
            if (args.Count > 1)
                message = command.Substring(args[0].Length + 1);
            string trigger = args[0].ToLower();
            args.RemoveAt(0);
            // Check if the trigger exists
            Command c = this.GetCommandByTrigger(trigger);
            if (c == null)
            {
                string error = string.Format(
                    "Unknown command '{1}{0}'. Use '{1}help' for more information",
                    trigger, this.Prefix);
                this._context.Write(MessageClass.Error, error);
                return false;
            }
            // Trigger the command
            return c.Process(this._context, trigger, message, args.ToArray());
        }

        public bool RegisterCommand(Command command)
        {
            // Check if we can safely register this command
            if (command == null)
                throw new ArgumentNullException();
            lock (this)
            {
                if (this.HasCommand(command.Name))
                    throw new ArgumentException("Duplicate command name: " + command.Name);
                foreach (string trigger in command.Triggers)
                {
                    if (this.HasTrigger(trigger))
                    {
                        string error = string.Format(
                            "Duplicate trigger '{0}' while registering command '{1}'. Trigger is already part of '{2}'",
                            trigger, command.Name, this.GetCommandByTrigger(trigger).Name);
                        throw new ArgumentException(error);
                    }
                }
                // Register command
                this._commands.Add(command.Name, command);
                foreach (string trigger in command.Triggers)
                {
                    this._triggers.Add(trigger.ToLower(), command);
                }
                return true;
            }
        }

        public void UnregisterCommand(string name)
        {
            lock (this)
            {
                if (!this._commands.ContainsKey(name))
                    throw new ArgumentException("Unknown command: " + name);
                Command command = this._commands[name];
                foreach (string trigger in command.Triggers)
                    this._triggers.Remove(trigger.ToLower());
                this._commands.Remove(name);
            }
        }

        public void UnregisterCommandByTrigger(string trigger)
        {
            lock (this)
            {
                Command command = GetCommandByTrigger(trigger);
                if (command == null)
                    throw new ArgumentException("Unknown trigger: " + trigger);
                UnregisterCommand(command.Name);
            }
        }

        public Command[] GetCommands()
        {
            List<Command> commands = new List<Command>();
            lock (this)
            {
                foreach (Command command in this._commands.Values)
                    commands.Add(command);
            }
            return commands.ToArray();
        }

        public bool HasCommand(string name)
        {
            lock (this)
            {
                return this._commands.ContainsKey(name);
            }
        }

        public Command GetCommand(string name)
        {
            lock (this)
            {
                if (!this._commands.ContainsKey(name))
                    return null;
                return this._commands[name];
            }
        }

        public bool HasTrigger(string trigger)
        {
            lock (this)
            {
                return this._triggers.ContainsKey(trigger.ToLower());
            }
        }

        public string[] GetTriggers()
        {
            List<string> triggers = new List<string>();
            lock (this)
            {
                foreach (string trigger in this._triggers.Keys)
                    triggers.Add(trigger);
            }
            return triggers.ToArray();
        }

        public Command GetCommandByTrigger(string trigger)
        {
            string trig = trigger.ToLower();
            lock (this)
            {
                if (!this._triggers.ContainsKey(trig))
                    return null;
                return this._triggers[trig];
            }
        }
        #endregion

        #region Internal
        internal Input(Context context, string prefix)
        {
            this._context = context;
            this._prefix = prefix;
            this._commands = new SortedDictionary<string, Command>();
            this._triggers = new SortedDictionary<string, Command>();
        }

        private Context _context;
        private string _prefix;
        private SortedDictionary<string, Command> _commands;
        private SortedDictionary<string, Command> _triggers;
        #endregion
    }
}
