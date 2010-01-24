/*
* Vha.Common
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

namespace Vha.Common
{
    /// <summary>
    /// Returns html color tags of the pre-defined colors
    /// </summary>
    public static class Colors
    {
        public static string Black { get { return HTML.CreateColorStart(ColorsHex.Black); } }
        public static string Green { get { return HTML.CreateColorStart(ColorsHex.Green); } }
        public static string Silver { get { return HTML.CreateColorStart(ColorsHex.Silver); } }
        public static string Lime { get { return HTML.CreateColorStart(ColorsHex.Lime); } }
        public static string Gray { get { return HTML.CreateColorStart(ColorsHex.Gray); } }
        public static string Olive { get { return HTML.CreateColorStart(ColorsHex.Olive); } }
        public static string White { get { return HTML.CreateColorStart(ColorsHex.White); } }
        public static string Yellow { get { return HTML.CreateColorStart(ColorsHex.Yellow); } }
        public static string Maroon { get { return HTML.CreateColorStart(ColorsHex.Maroon); } }
        public static string Navy { get { return HTML.CreateColorStart(ColorsHex.Navy); } }
        public static string Red { get { return HTML.CreateColorStart(ColorsHex.Red); } }
        public static string Blue { get { return HTML.CreateColorStart(ColorsHex.Blue); } }
        public static string Purple { get { return HTML.CreateColorStart(ColorsHex.Purple); } }
        public static string Teal { get { return HTML.CreateColorStart(ColorsHex.Teal); } }
        public static string Fuchsia { get { return HTML.CreateColorStart(ColorsHex.Fuchsia); } }
        public static string Aqua { get { return HTML.CreateColorStart(ColorsHex.Aqua); } }
    }

    /// <summary>
    /// Returns the hex values of the pre-defined colors
    /// </summary>
    public static class ColorsHex
    {
        public static string Black { get { return "000000"; } }
        public static string Green { get { return "008000"; } }
        public static string Silver { get { return "C0C0C0"; } }
        public static string Lime { get { return "00FF00"; } }
        public static string Gray { get { return "808080"; } }
        public static string Olive { get { return "808000"; } }
        public static string White { get { return "FFFFFF"; } }
        public static string Yellow { get { return "FFFF00"; } }
        public static string Maroon { get { return "800000"; } }
        public static string Navy { get { return "000080"; } }
        public static string Red { get { return "ff0000"; } }
        public static string Blue { get { return "0000FF"; } }
        public static string Purple { get { return "800080"; } }
        public static string Teal { get { return "008080"; } }
        public static string Fuchsia { get { return "FF00FF"; } }
        public static string Aqua { get { return "00FFFF"; } }
    }
}
