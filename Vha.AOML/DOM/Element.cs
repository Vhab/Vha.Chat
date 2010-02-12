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

namespace Vha.AOML.DOM
{
    public enum ElementType
    {
        Text,
        Color,
        Align,
        Link,
        Image,
        Break
    }

    public abstract class Element
    {
        /// <summary>
        /// Returns the type of this element
        /// </summary>
        public ElementType Type { get { return this._type; } }

        /// <summary>
        /// Returns a collection of all children of this element.
        /// If this element doesn't support children, this member will return null.
        /// </summary>
        public ElementCollection Children { get { return this._children; } }

        /// <summary>
        /// Returns the parent of this element.
        /// If this element is not a child of any element, this member will return null.
        /// </summary>
        public Element Parent { get { return this._parent; } }

        /// <summary>
        /// Returns true if this element supports children.
        /// </summary>
        public bool SupportsChildren { get { return this._supportsChildren; } }

        /// <summary>
        /// Returns a clone of the current element, including all its children.
        /// The cloned element will not be attached to any element. Its parent value will always default to null.
        /// </summary>
        /// <returns>An element with the same properties and children as this element</returns>
        public abstract Element Clone();

        #region Internal
        private ElementType _type;
        private ElementCollection _children = null;
        private Element _parent = null;
        private bool _supportsChildren;

        internal Element(ElementType type, bool supportsChildren)
        {
            this._type = type;
            this._supportsChildren = supportsChildren;
            if (supportsChildren)
                this._children = new ElementCollection(this);
        }

        /// <summary>
        /// Notify the element of being attached to another element.
        /// This method is expected to be triggered before actually attaching the element.
        /// </summary>
        /// <param name="parent">The element this element will be attached to as child</param>
        /// <exception cref="System.InvalidOperationException">Thrown when attaching this element would result in an invalid state</exception>
        internal void OnAttach(Element parent)
        {
            if (parent == this)
                throw new InvalidOperationException("Can't attach an element to itself");
            if (this._parent != null)
                throw new InvalidOperationException("Not expecting to be attached. This element already has a parent");
            if (parent.SupportsChildren == false)
                throw new InvalidOperationException("Not expecting to be attached. The supplied parent doesn't support children");
            if (parent.Children.Contains(this))
            {
                this._parent = parent;
                throw new InvalidOperationException("Not expecting to be attached. This element already is attached to the parent");
            }
            this._parent = parent;
        }

        /// <summary>
        /// Notify the element of being detached from another element.
        /// This method is expected to be triggered after actually detaching the element.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when detaching this element would result in an invalid state</exception>
        internal void OnDetach()
        {
            if (this._parent == null)
                throw new InvalidOperationException("Not expecting to be detached. This element has no parent");
            if (this._parent.Children.Contains(this))
                throw new InvalidOperationException("Not expecting to be detached. This element is still a child of its parent");
            this._parent = null;
        }
        #endregion
    }
}
