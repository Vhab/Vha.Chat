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
    /// Summary description for SystemMessagePacket.
    /// </summary>
    public class SystemMessagePacket : Packet
    {
        internal SystemMessagePacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 6) { return; }
            int offset = 0;
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
            this.AddData(popUnsignedInteger(ref data, ref offset));
        }

        internal UInt32 ClientID { get { return (UInt32)this.Data[0]; } }
        internal UInt32 WindowID { get { return (UInt32)this.Data[1]; } }
        internal UInt32 MessageID { get { return (UInt32)this.Data[2]; } }
    }

    /// <summary>
    /// Class for holding event args for system messages.
    /// </summary>
    public class SystemMessagePacketEventArgs : EventArgs
    {
        private readonly UInt32 _client = 0;
        private readonly UInt32 _window = 0;
        private readonly UInt32 _msg = 0;
        private readonly String _msgText = String.Empty;

        public SystemMessagePacketEventArgs(UInt32 clientID, UInt32 windowID, UInt32 messageID)
        {
            this._client = clientID;
            this._window = windowID;
            this._msg = messageID;
        }

        public UInt32 ClientID { get { return this._client; } }
        public UInt32 WindowID { get { return this._window; } }
        public UInt32 MessageID { get { return this._msg; } }
    }
}