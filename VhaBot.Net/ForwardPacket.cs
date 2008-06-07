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
using System.Collections;

namespace VhaBot.Net
{
    internal class ForwardPacket : Packet
    {
        internal ForwardPacket(Packet.Type type, byte[] data) : base(type, data) { }
        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 5) { return; }
            int offset = 0;
            this.AddData(popInteger(ref data, ref offset));
            this.AddData(popInteger(ref data, ref offset));
        }
        internal Int32 ID1 { get { return (Int32)this.Data[0]; } }
        internal Int32 ID2 { get { return (Int32)this.Data[1]; } }
    }

    public class ForwardEventArgs : EventArgs
    {
        private readonly Int32 _id1 = 0;
        private readonly Int32 _id2 = 0;
        public ForwardEventArgs(Int32 ID1, Int32 ID2)
        {
            this._id1 = ID1;
            this._id2 = ID2;
        }
        public Int32 ID1 { get { return this._id1; } }
        public Int32 ID2 { get { return this._id2; } }
    }
}
