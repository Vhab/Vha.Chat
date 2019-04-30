/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A link to a command to be executed.
    /// Commonly known as chatcmd://
    /// </summary>
    public class CommandLink : Link
    {
        /// <summary>
        /// Returns the command which should be executed
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of CommandLink
        /// </summary>
        /// <param name="command">The command to be contained within this link</param>
        public CommandLink(string command)
            : base(LinkType.Command)
        {
            this.Command = command;
        }

        /// <summary>
        /// Creates a clone of this CommandLink
        /// </summary>
        /// <returns>A new CommandLink</returns>
        public override Link Clone()
        {
            return new CommandLink(this.Command);
        }
    }
}
