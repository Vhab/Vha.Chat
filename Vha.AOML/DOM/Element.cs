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

namespace Vha.AOML.DOM
{
    /// <summary>
    /// Identifies the type of an Element
    /// </summary>
    public enum ElementType
    {
        Container,
        Text,
        Color,
        Align,
        Underline,
        Italic,
        Link,
        Image,
        Break
    }

    /// <summary>
    /// Base class for all elements
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Returns the type of this element
        /// </summary>
        public ElementType Type { get; private set; }
        /// <summary>
        /// Returns a collection of all children of this element.
        /// If this element doesn't support children, this member will return null.
        /// </summary>
        public ElementCollection Children { get; private set; }
        /// <summary>
        /// Returns the parent of this element.
        /// If this element is not a child of any element, this member will return null.
        /// </summary>
        public Element Parent { get; private set; }
        /// <summary>
        /// Returns true if this element supports children
        /// </summary>
        public bool SupportsChildren { get; private set; }

        /// <summary>
        /// Recursively checks if the given element is a parent of this element
        /// </summary>
        /// <param name="element">The element to check against</param>
        /// <returns>True if the given element is a parent of this element</returns>
        public bool IsParent(Element element)
        {
            if (this.Parent == null) { return false; }
            if (element == this.Parent) { return true; }
            return this.Parent.IsParent(element);
        }

        /// <summary>
        /// Recursively checks if the given element is a child of this element
        /// </summary>
        /// <param name="element">The element to check against</param>
        /// <returns>True if the given element is a child of this element</returns>
        public bool IsChild(Element element)
        {
            if (this.SupportsChildren == false) { return false; }
            if (this.Children.Contains(element)) { return true; }
            foreach (Element child in this.Children)
            {
                if (child.IsChild(element))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Recursively checks if this element has a parent of the specified type
        /// </summary>
        /// <param name="type">The type to check against</param>
        /// <returns>True if this elements has a parent of the specified type</returns>
        public bool HasParent(ElementType type)
        {
            if (this.Parent == null) { return false; }
            if (this.Parent.Type == type) { return true; }
            return this.Parent.HasParent(type);
        }

        /// <summary>
        /// Recursively checks if this element has a parent of the specified type
        /// </summary>
        /// <param name="type">The type to check against</param>
        /// <returns>True if this elements has a parent of the specified type</returns>
        public bool HasChild(ElementType type)
        {
            if (this.SupportsChildren == false) { return false; }
            foreach (Element child in this.Children)
            {
                if (child.Type == type) { return true; }
                if (child.HasChild(type)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Returns a clone of the current element, including all its children.
        /// The cloned element will not be attached to any element. Its parent value will always default to null.
        /// </summary>
        /// <returns>An element with the same properties and children as this element</returns>
        public abstract Element Clone();

        #region Internal
        internal Element(ElementType type, bool supportsChildren)
        {
            this.Type = type;
            this.SupportsChildren = supportsChildren;
            if (supportsChildren)
            {
                this.Children = new ElementCollection(this);
            }
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
            {
                throw new InvalidOperationException("Can't attach an element to itself");
            }
            if (this.Parent != null)
            {
                throw new InvalidOperationException("Not expecting to be attached. This element already has a parent");
            }
            if (parent.SupportsChildren == false)
            {
                throw new InvalidOperationException("Not expecting to be attached. The supplied parent doesn't support children");
            }
            if (parent.Children.Contains(this))
            {
                this.Parent = parent;
                throw new InvalidOperationException("Not expecting to be attached. This element already is attached to the parent");
            }
            if (this.IsChild(parent))
            {
                throw new InvalidOperationException("Not expecting to be attached. Can not attach this element to one of its children");
            }
            this.Parent = parent;
        }

        /// <summary>
        /// Notify the element of being detached from another element.
        /// This method is expected to be triggered after actually detaching the element.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when detaching this element would result in an invalid state</exception>
        internal void OnDetach()
        {
            if (this.Parent == null)
            {
                throw new InvalidOperationException("Not expecting to be detached. This element has no parent");
            }
            if (this.Parent.Children.Contains(this))
            {
                throw new InvalidOperationException("Not expecting to be detached. This element is still a child of its parent");
            }
            this.Parent = null;
        }
        #endregion
    }
}
