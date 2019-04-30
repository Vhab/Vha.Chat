/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
        #region Public accessors & properties
        /// <summary>
        /// Returns the type of this AOML opening tag
        /// </summary>
        public string Name {get; private set; }
        /// <summary>
        /// Returns whether this element is automatically closed
        /// </summary>
        public bool Closed { get; internal set; }
        /// <summary>
        /// Returns the amount of attributes contained within this node
        /// </summary>
        public int Count
        {
            get { return this.attributes.Count; }
        }
        #endregion

        /// <summary>
        /// Returns whether this node contains the specified attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Whether the specified attribute exists within this node</returns>
        public bool HasAttribute(string name)
        {
            return this.attributes.ContainsKey(name.ToLower());
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
            return this.attributes[name];
        }

        /// <summary>
        /// Returns the name of an attribute for iteration purposes
        /// </summary>
        /// <param name="index">A value 0 or greater and less than Count</param>
        /// <returns>The attribute name at a given index</returns>
        public string GetAttributeName(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new IndexOutOfRangeException();
            }
            foreach (string name in this.attributes.Keys)
            {
                if (index == 0)
                {
                    return name;
                }
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
        private Dictionary<string, string> attributes;

        internal OpenNode(string name, string raw, bool closed)
            : base(NodeType.Open, raw)
        {
            this.Name = name.ToLower();
            this.Closed = closed;
            this.attributes = new Dictionary<string, string>();
        }

        internal void AddAttribute(string name, string value)
        {
            name = name.ToLower();
            if (this.attributes.ContainsKey(name))
            {
                this.attributes[name] = value;
            }
            else this.attributes.Add(name, value);
        }

        internal void RemoveAttribute(string name)
        {
            if (HasAttribute(name) == false) { return; }
            name = name.ToLower();
            this.attributes.Remove(name);
        }
        #endregion
    }
}
