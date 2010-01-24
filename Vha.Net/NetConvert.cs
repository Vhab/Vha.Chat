/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
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
using System.Net;

namespace Vha.Net
{
    public static class NetConvert
    {
        public static UInt32 ToUInt32(Int32 value)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        }

        public static Int32 ToInt32(UInt32 value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }

        public static UInt32 HostToNetworkOrder(UInt32 value)
        {
            return ToUInt32(IPAddress.HostToNetworkOrder(ToInt32(value)));
        }

        public static UInt32 NetworkToHostOrder(UInt32 value)
        {
            return ToUInt32(IPAddress.NetworkToHostOrder(ToInt32(value)));
        }

        public static UInt16 ToUInt16(Int16 value)
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(value), 0);
        }

        public static Int16 ToInt16(UInt16 value)
        {
            return BitConverter.ToInt16(BitConverter.GetBytes(value), 0);
        }

        public static UInt16 HostToNetworkOrder(UInt16 value)
        {
            return ToUInt16(IPAddress.HostToNetworkOrder(ToInt16(value)));
        }

        public static UInt16 NetworkToHostOrder(UInt16 value)
        {
            return ToUInt16(IPAddress.NetworkToHostOrder(ToInt16(value)));
        }
    }
}
