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
using System.Text;

namespace Vha.Chat.Commands
{
    public abstract class Command
    {
        /// <summary>
        /// The name of the command.
        /// This name is used for documentation purposes.
        /// Be sure to use a properly capitalized and clear name.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// A list of command prefixes this command responds to.
        /// These commands have to be unique and can't conflict with other commands.
        /// </summary>
        public readonly string[] Triggers;
        /// <summary>
        /// An array of usage patterns for the command (excluding the command prefix).
        /// For example:
        /// {"command [username]", "command [quantity] [username]"}
        /// </summary>
        public readonly string[] Usage;
        /// <summary>
        /// An array of examples for the command (excluding the command prefix).
        /// For example:
        /// {"command Vhab", "command 100 Vhab"}
        /// </summary>
        public readonly string[] Examples;
        /// <summary>
        /// A general discription of the command(s) contained by this object.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Implements and executes the command(s) contained by this object.
        /// </summary>
        /// <param name="context">The chat client context</param>
        /// <param name="command">The command that triggered this callback</param>
        /// <param name="arguments">The arguments passed to the command</param>
        /// <returns>True for success, false for failure</returns>
        public abstract bool Process(Context context, string trigger, string message, string[] args);

        /// <summary>
        /// Initializes a new command.
        /// Only usable by derived classes.
        /// </summary>
        protected Command(string name, string[] triggers, string[] usage, string[] examples, string description)
        {
            this.Name = name;
            this.Triggers = triggers;
            this.Usage = usage;
            this.Examples = examples;
            this.Description = description;
        }
    }
}
