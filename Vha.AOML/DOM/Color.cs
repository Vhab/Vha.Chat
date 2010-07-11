/*
* Vha.AOML
* Copyright (C) 2010 Remco van Oosterhout
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
using System.Xml;
using System.IO;

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A container for 3-component 24-bit colors
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Returns the red component of this color in a range of 0 to 256
        /// </summary>
        public readonly Byte Red;

        /// <summary>
        /// Returns the green component of this color in a range of 0 to 256
        /// </summary>
        public readonly Byte Green;

        /// <summary>
        /// Returns the blue component of this color in a range of 0 to 256
        /// </summary>
        public readonly Byte Blue;

        /// <summary>
        /// Initializes a new color
        /// </summary>
        public Color()
        {
            this.Red = 0;
            this.Green = 0;
            this.Blue = 0;
        }

        /// <summary>
        /// Initializes a new color
        /// </summary>
        /// <param name="red">The red component of the color in a range of 0 to 256</param>
        /// <param name="green">The green component of the color in a range of 0 to 256</param>
        /// <param name="blue">The blue component of the color in a range of 0 to 256</param>
        public Color(Byte red, Byte green, Byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        /// <summary>
        /// Initializes a new color
        /// </summary>
        /// <param name="color">The desired color</param>
        public Color(Color color)
        {
            if (color == null)
                throw new ArgumentNullException();
            this.Red = color.Red;
            this.Green = color.Green;
            this.Blue = color.Blue;
        }

        /// <summary>
        /// Creates a new color from an HTML formatted color string.
        /// This method assumes a named web color or # followed by a 6-digit hex value.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>A new color or null for failure</returns>
        public static Color FromString(string color)
        {
            // Load TextColors.xml
            if (_colors == null)
            {
                MemoryStream stream = new MemoryStream(Properties.Resources.TextColors);
                _colors = new XmlDocument();
                _colors.Load(stream);
            }
            // Check for # prefix
            if (color.StartsWith("#"))
            {
                return FromHex(color.Substring(1));
            }
            // Check for 0x prefix
            if (color.StartsWith("0x"))
            {
                color = color.Substring(2);
                if (color.Length == 8) return FromHex(color.Substring(2));
                else if (color.Length == 6) return FromHex(color);
                else return null;
            }
            // Fetch colors
            XmlNodeList colors = _colors.GetElementsByTagName("HTMLColor");
            foreach (XmlNode node in colors)
            {
                // Some safety
                if (node.Attributes["color"] == null) continue;
                if (node.Attributes["name"] == null) continue;
                // Check if this is the one
                if (string.Equals(node.Attributes["name"].Value, color, StringComparison.CurrentCultureIgnoreCase) == false) continue;
                // Convert the value to something sensible
                return FromString(node.Attributes["color"].Value);
            }
            return null;
        }

        /// <summary>
        /// Creates a new color from a 6-digit hex string
        /// </summary>
        /// <param name="hex">A 6-digit hex string without any prefix or suffix</param>
        /// <returns>A new color or null for failure</returns>
        public static Color FromHex(string hex)
        {
            // The description says 6-digit, enforce it!
            if (hex.Length != 6) return null;
            // Try to convert the color
            try
            {
                int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                return new Color((Byte)r, (Byte)g, (Byte)b);
            }
            catch (FormatException)
            {
                // Someone failed to read the instructions...
                return null;
            }
        }

        #region Internal
        private static XmlDocument _colors = null;
        #endregion
    }
}
