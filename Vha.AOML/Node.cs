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
    /// Identifies the type of a Node
    /// </summary>
    public enum NodeType
    {
        Open,
        Close,
        Content
    }

    /// <summary>
    /// A base class for nodes produced by Parser
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Returns the type of this Node
        /// </summary>
        public NodeType Type { get; private set; }
        /// <summary>
        /// The raw unparsed HTML that forms this node.
        /// This value is not garanteed to be filled and might be left empty.
        /// </summary>
        public string Raw { get; private set; }
        /// <summary>
        /// Creates a clone of the current node
        /// </summary>
        /// <returns>A new Node instance</returns>
        public abstract Node Clone();

        #region Internal
        internal Node(NodeType type, string raw)
        {
            this.Type = type;
            this.Raw = raw;
        }
        #endregion
    }
}
