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
        /// Returns whether this element is automatically closed
        /// </summary>
        public bool Closed
        {
            get { return this._closed; }
            internal set { this._closed = value; }
        }
        
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

        /// <summary>
        /// Returns the name of an attribute for iteration purposes
        /// </summary>
        /// <param name="index">A value 0 or greater and less than Count</param>
        /// <returns>The attribute name at a given index</returns>
        public string GetAttributeName(int index)
        {
            if (index < 0 || index >= this.Count)
                throw new IndexOutOfRangeException();
            foreach (string name in this._attributes.Keys)
            {
                if (index == 0)
                    return name;
                index--;
            }
            return null;
        }

        /// <summary>
        /// Creates a clone of the current node
        /// </summary>
        /// <returns>A new OpenNode instance</returns>
        public override Node Clone()
        {
            OpenNode node = new OpenNode(this.Name, "", this.Closed);
            for (int i = 0; i < this.Count; i++)
            {
                string attr = this.GetAttributeName(i);
                node.AddAttribute(attr, this.GetAttribute(attr));
            }
            return node;
        }

        #region Internal
        private Dictionary<string, string> _attributes;
        private bool _closed;

        internal OpenNode(string name, string raw, bool closed)
            : base(NodeType.Open, raw)
        {
            this.Name = name.ToLower();
            this._closed = closed;
            this._attributes = new Dictionary<string, string>();
        }

        internal void AddAttribute(string name, string value)
        {
            name = name.ToLower();
            if (this._attributes.ContainsKey(name))
                this._attributes[name] = value;
            else this._attributes.Add(name, value);
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
