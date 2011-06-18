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

namespace Vha.AOML
{
    /// <summary>
    /// This class represents content outside and between AOML tags
    /// </summary>
    public class ContentNode : Node
    {
        /// <summary>
        /// Returns the text content associated with this node
        /// </summary>
        public readonly string Value;
        /// <summary>
        /// Creates a clone of the current node
        /// </summary>
        /// <returns>A new ContentNode instance</returns>
        public override Node Clone()
        {
            return new ContentNode(this.Value, "");
        }

        #region Internal
        internal ContentNode(string value, string raw)
            : base(NodeType.Content, raw)
        {
            this.Value = value;
        }
        #endregion
    }
}
