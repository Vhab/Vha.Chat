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
    /// Summary description for TellPacket.
    /// </summary>
    internal class TellPacket : Packet
    {
        private UInt32 _characterID = 0;

        internal TellPacket(Packet.Type type, byte[] data) : base(type, data) { }

        /// <summary>
        /// Constructor for outgoing packet data
        /// </summary>
        /// <param name="characterID">The id of the character to who to send the message.</param>
        /// <param name="formattedText">The contents of the message.  Can contain click links and color formatting in HTML.</param>
        internal TellPacket(UInt32 characterID, String formattedText)
            : base(Packet.Type.PRIVATE_MESSAGE)
        {
            this._characterID = characterID;
            this.AddData(NetConvert.HostToNetworkOrder(this._characterID));
            this.AddData(new NetString(formattedText));
            this.AddData(new NetString("\0"));
        }
        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 4) { return; }

            int offset = 0;
            this._characterID = popUnsignedInteger(ref data, ref offset);
            this.AddData(this._characterID);
            this.AddData(popString(ref data, ref offset));
        }
        internal UInt32 CharacterID
        {
            get
            {
                if (this._characterID != 0)
                    return this._characterID;
                return (UInt32)this.Data[0];
            }
        }
        internal string Message { get { return ((NetString)this.Data[1]).ToString(); } }
    }

    /// <summary>
    /// Holds event args for tell messages.
    /// </summary>
    public class TellEventArgs : EventArgs
    {
        private readonly UInt32 _characterID = 0;
        private readonly string _character;
        private readonly string _msg = null;
        private readonly bool _outgoing = false;

        /// <summary>
        /// Tell message event argument constructor
        /// </summary>
        /// <param name="CharacterID">The character id of the sender.</param>
        /// <param name="Message">Message containing text and click links</param>
        /// <param name="VoiceCommand">voice blob in the message, if any</param>
        public TellEventArgs(UInt32 characterID, string character, string message, bool outgoing)
        {
            this._characterID = characterID;
            this._character = character;
            this._msg = message;
            this._outgoing = outgoing;
        }

        /// <summary>
        /// The character id of the sender.
        /// </summary>
        public UInt32 CharacterID { get { return this._characterID; } }
        public string Character { get { return this._character; } }
        /// <summary>
        /// The originating message containing any text and click links.
        /// </summary>
        public string Message { get { return this._msg; } }
        /// <summary>
        /// Whether the message is an outgoing or incomming message
        /// </summary>
        public bool Outgoing { get { return this._outgoing; } }
    }
}
