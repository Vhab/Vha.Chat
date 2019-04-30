/*
* Vha.Net
* Copyright (C) 2005-2011 Remco van Oosterhout
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Vha.Net.Events
{
    /// <summary>
    /// Class for holding event args for AO Chat Amd Mux Info message events.
    /// </summary>
    public class AmdMuxInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">An unknown message</param>
        public AmdMuxInfoEventArgs(BigInteger message)
        {
            this.Message = message;
        }

        /// <summary>
        /// An unknown message
        /// </summary>
        public BigInteger Message { get; private set; }
    }
}
