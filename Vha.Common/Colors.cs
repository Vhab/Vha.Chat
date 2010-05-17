/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

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
