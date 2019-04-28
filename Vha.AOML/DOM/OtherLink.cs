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
using System.Text;

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A link to something different than the other implementations of Link
    /// </summary>
    public class OtherLink : Link
    {
        /// <summary>
        /// A link
        /// </summary>
        public Uri Uri {get; private set; }

        public OtherLink(string uri)
            : base(LinkType.Other)
        {
            this.Uri = new Uri(uri);
        }

        public OtherLink(Uri uri)
            : base(LinkType.Other)
        {
            this.Uri = new Uri(uri.ToString());
        }

        /// <summary>
        /// Creates a clone of this OtherLink
        /// </summary>
        /// <returns>A new OtherLink</returns>
        public override Link Clone()
        {
            return new OtherLink(this.Uri);
        }
    }
}
