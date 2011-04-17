/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
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

namespace Vha.Net.Events
{
    /// <summary>
    /// Class for holding event args for broadcast messages.
    /// </summary>
    public class BroadcastMessageEventArgs : EventArgs
    {
        private readonly string _str = null;
        private readonly string _message = null;

        /// <summary>
        /// The event argument constructor
        /// </summary>
        /// <param name="str">an unknown string</param>
        /// <param name="message">the text of the message</param>
        public BroadcastMessageEventArgs(String str, string message)
        {
            this._str = str;
            this._message = message;
        }

        /// <summary>
        /// An unknown string
        /// </summary>
        public String UnknownString { get { return this._str; } }
        /// <summary>
        /// The text of the message containing text and click links.
        /// </summary>
        public string Message { get { return this._message; } }
    }
}
