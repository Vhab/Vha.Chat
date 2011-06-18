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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// Identifies the type of an image of an ImageElement
    /// </summary>
    public enum ImageType
    {
        TDB, // Gui images
        RDB // Icons
    }

    /// <summary>
    /// An element that describes an image.
    /// Commonly known as IMG.
    /// </summary>
    public class ImageElement : Element
    {
        /// <summary>
        /// Returns the type of the image
        /// </summary>
        public readonly ImageType ImageType;
        
        /// <summary>
        /// Returns the string identifying the image
        /// </summary>
        public readonly string Image;

        /// <summary>
        /// Initializes a new instance of ImageElement
        /// </summary>
        /// <param name="type">The type of this element's image</param>
        /// <param name="image">The string identifying this element's image</param>
        public ImageElement(ImageType type, string image)
            : base(ElementType.Image, false)
        {
            this.ImageType = type;
            this.Image = image;
        }

        /// <summary>
        /// Creates a clone of this ImageElement
        /// </summary>
        /// <returns>A new ImageElement</returns>
        public override Element Clone()
        {
            return new ImageElement(this.ImageType, this.Image);
        }
    }
}
