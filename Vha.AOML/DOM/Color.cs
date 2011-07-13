/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
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
using System.IO;
using System.Xml;

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
        public Byte Red { get; private set; }

        /// <summary>
        /// Returns the green component of this color in a range of 0 to 256
        /// </summary>
        public Byte Green { get; private set; }

        /// <summary>
        /// Returns the blue component of this color in a range of 0 to 256
        /// </summary>
        public Byte Blue { get; private set; }

        #region Constructors
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
            if (color == null) { throw new ArgumentNullException(); }
            this.Red = color.Red;
            this.Green = color.Green;
            this.Blue = color.Blue;
        }
        #endregion

        /// <summary>
        /// Creates a new color from an HTML formatted color string.
        /// This method assumes a named web color or # followed by a 6-digit hex value.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>A new color or null for failure</returns>
        public static Color FromString(string color)
        {
            // Load TextColors.xml
            if (colors == null)
            {
                MemoryStream stream = new MemoryStream(Properties.Resources.TextColors);
                XmlDocument xml = new XmlDocument();
                xml.Load(stream);
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                // Fetch colors
                XmlNodeList rawColors = xml.GetElementsByTagName("HTMLColor");
                foreach (XmlNode node in rawColors)
                {
                    // Some safety
                    if (node.Attributes["color"] == null) { continue; }
                    if (node.Attributes["name"] == null) { continue; }
                    dictionary.Add(
                        node.Attributes["name"].Value.ToLower(),
                        node.Attributes["color"].Value.ToLower());
                }
                colors = dictionary;
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
                if (color.Length == 8)
                {
                    return FromHex(color.Substring(2));
                }
                else if (color.Length == 6)
                {
                    return FromHex(color);
                }
                else
                {
                    return null;
                }
            }
            // Fetch named color
            color = color.ToLower();
            if (colors.ContainsKey(color))
            {
                return FromString(colors[color]);
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

        public override string ToString()
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", this.Red, this.Green, this.Blue);
        }

        #region Internal
        private static Dictionary<string, string> colors = null;
        #endregion
    }
}
