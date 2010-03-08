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

namespace Vha.AOML
{
    /// <summary>
    /// This class represents an AOML opening tag and its associated attributes
    /// </summary>
    public class OpenNode : Node
    {
        /// <summary>
        /// Returns the type of this AOML opening tag
        /// </summary>
        public readonly string Name;
        
        /// <summary>
        /// Returns the amount of attributes contained within this node
        /// </summary>
        public int Count
        {
            get { return this._attributes.Count; }
        }

        /// <summary>
        /// Returns whether this node contains the specified attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Whether the specified attribute exists within this node</returns>
        public bool HasAttribute(string name)
        {
            return this._attributes.ContainsKey(name.ToLower());
        }

        /// <summary>
        /// Returns the specified attribute's value contained within this node
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>The value of the attribute or null if the attribute doesn't exist</returns>
        public string GetAttribute(string name)
        {
            if (HasAttribute(name) == false) return null;
            name = name.ToLower();
            return this._attributes[name];
        }

        #region Internal
        internal Dictionary<string, string> _attributes;

        internal OpenNode(string name)
            : base(NodeType.Open)
        {
            this.Name = name.ToLower();
            this._attributes = new Dictionary<string, string>();
        }

        internal void AddAttribute(string name, string value)
        {
            name = name.ToLower();
            this._attributes[name] = value;
        }

        internal void RemoveAttribute(string name)
        {
            if (HasAttribute(name) == false) return;
            name = name.ToLower();
            this._attributes.Remove(name);
        }
        #endregion
    }
}
