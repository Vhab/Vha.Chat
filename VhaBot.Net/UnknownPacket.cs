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
    /// <summary>
    /// Class used when unknown packets are received.
    /// </summary>
    internal class UnknownPacket : Packet
    {
        internal UnknownPacket(Packet.Type type, byte[] data) : base(type, data) { }
        override protected void BytesToData(byte[] data)
        {
            this.AddData(new NetString(data, 0, (short)data.Length).Value);
        }

        /// <summary>
        /// The message received, whatever that was
        /// </summary>
        internal String UnknownData { get { return BitConverter.ToString(this.DataToBytes()); } }
    }

    /// <summary>
    /// Class for holding event args for unknown events.
    /// </summary>
    public class UnknownPacketEventArgs : EventArgs
    {
        /// <summary>
        /// Private member to store the message.
        /// </summary>
        private readonly String _text = null;
        private readonly Packet.Type _type = 0;

        /// <summary>
        /// Constructor for creating event args
        /// </summary>
        /// <param name="Message"></param>
        public UnknownPacketEventArgs(Packet.Type type, String Message)
        {
            this._type = type;
            this._text = Message;
        }

        /// <summary>
        /// Message from unknown event
        /// </summary>
        /// <value>returns the message</value>
        public String Message { get { return this._text; } }
        public Packet.Type PacketType { get { return this._type; } }
    }
}
