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
using VhaBot.Common;

namespace VhaBot.Net
{
    internal class ChannelMessagePacket : Packet
    {
        internal ChannelMessagePacket(Packet.Type type, byte[] data) : base(type, data) { }
        internal ChannelMessagePacket(BigInteger channelID, String text)
            : base(Packet.Type.CHANNEL_MESSAGE)
        {
            this.AddData(channelID);
            this.AddData(new NetString(text));
            this.AddData(new NetString("\0")); // Investigate if we still need this!
        }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 4) { return; }

            int offset = 0;
            this.AddData(popChannelID(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popString(ref data, ref offset).ToString());
        }

        internal BigInteger ChannelID { get { return (BigInteger)this.Data[0]; } }
        internal UInt32 CharacterID { get { return (UInt32)this.Data[1]; } }
        internal string Message { get { return (String)this.Data[2]; } }
    }

    /// <summary>
    /// Class for holding event args for channel message events.
    /// </summary>
    public class ChannelMessageEventArgs : EventArgs
    {
        private readonly BigInteger _channelID = 0;
        private readonly string _channel;
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly string _message = null;
        private ChannelType _type = ChannelType.Unknown;

        /// <summary>
        /// Constructor for channel message events
        /// </summary>
        /// <param name="channelID">5-byte channel id</param>
        /// <param name="channel">channel name</param>
        /// <param name="characterID">character id</param>
        /// <param name="character">character name</param>
        /// <param name="message">message</param>
        public ChannelMessageEventArgs(BigInteger channelID, string channel, UInt32 characterID, string character, string message, ChannelType type)
        {
            this._channelID = channelID;
            this._channel = channel;
            this._characterID = characterID;
            this._character = character;
            this._message = message;
            this._type = type;
        }

        public BigInteger ChannelID { get { return this._channelID; } }
        public string Channel { get { return this._channel; } }
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        public string Message { get { return this._message; } }
        public ChannelType Type { get { return this._type; } }
    }
}
