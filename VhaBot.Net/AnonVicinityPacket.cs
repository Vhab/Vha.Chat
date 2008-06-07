/*
* VhaBot - Barbaric Edition
* Copyright (C) 2005-2008 Remco van Oosterhout
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
using System.Net;

namespace VhaBot.Net
{
    internal class AnonVicinityPacket : Packet
    {
        internal AnonVicinityPacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal AnonVicinityPacket(String str, String formattedText) : base(Packet.Type.ANON_MESSAGE)
        {
            this.AddData(new NetString("\0"));
            this.AddData(new NetString(formattedText));
            this.AddData((byte)0);
        }
        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 6) { return; }

            int offset = 0;
            this.AddData(popString(ref data, ref offset).ToString());
            this.AddData(popString(ref data, ref offset).ToString());
        }
        internal String UnknownString { get { return (String)this.Data[0]; } }
        internal string Message { get { return (String)this.Data[1]; } }
    }

    /// <summary>
    /// Class for holding event args for anonymous vicinity messages.
    /// </summary>
    public class AnonVicinityEventArgs : EventArgs
    {
        private readonly string _str = null;
        private readonly string _message = null;

        /// <summary>
        /// The event argument constructor
        /// </summary>
        /// <param name="str">an unknown string</param>
        /// <param name="text">the text of the message</param>
        public AnonVicinityEventArgs(String str, string mesage)
        {
            this._str = str;
            this._message = mesage;
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
