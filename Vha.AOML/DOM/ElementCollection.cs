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
using System.Collections;
using System.Collections.Generic;

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A collection of elements
    /// </summary>
    public sealed class ElementCollection : IEnumerable<Element>
    {
        /// <summary>
        /// Returns the amount of elements currently contained in this collection.
        /// </summary>
        public int Count { get { return this.Elements.Count; } }

        /// <summary>
        /// Adds an element.
        /// </summary>
        /// <param name="element">The element to be added</param>
        /// <exception cref="System.InvalidOperationException">Thrown when attaching the given element would result in an invalid state</exception>
        public void Add(Element element)
        {
            element.OnAttach(this.Parent);
            this.Elements.Add(element);
        }

        /// <summary>
        /// Inserts an element at a specific index.
        /// </summary>
        /// <param name="index">The index the element will be added at</param>
        /// <param name="element">The element to be added</param>
        /// <exception cref="System.InvalidOperationException">Thrown when attaching the given element would result in an invalid state</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the given index is invalid</exception>
        public void Insert(int index, Element element)
        {
            element.OnAttach(this.Parent);
            try
            {
                // Attempt to insert this element
                this.Elements.Insert(index, element);
            }
            catch (IndexOutOfRangeException)
            {
                // Ensure the element is in a valid state after insertion failure
                element.OnDetach();
                throw; 
            }
        }

        /// <summary>
        /// Remove an element.
        /// </summary>
        /// <param name="element">The element to be removed</param>
        /// <exception cref="System.InvalidOperationException">Thrown when detaching the given element would result in an invalid state or if the element is not a child of this element</exception>
        public void Remove(Element element)
        {
            if (this.Contains(element) == false)
            {
                throw new InvalidOperationException("Not expecting to be detached. Given element is not attached to this element");
            }
            this.Elements.Remove(element);
            element.OnDetach();
        }

        /// <summary>
        /// Remove an element by index.
        /// </summary>
        /// <param name="index">Numerical index of the element to be removed</param>
        /// <exception cref="System.InvalidOperationException">Thrown when detaching the given element would result in an invalid state</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the given index is invalid</exception>
        public void RemoveAt(int index)
        {
            Element element = this.Elements[index];
            this.Elements.RemoveAt(index);
            element.OnDetach();
        }
        
        /// <summary>
        /// Returns whether this collection contains the given element
        /// </summary>
        /// <param name="element">The element to be checked</param>
        /// <returns>Returns true if the given element is part of this collection</returns>
        public bool Contains(Element element)
        {
            return this.Elements.Contains(element);
        }

        /// <summary>
        /// Converts this collection into a flat array
        /// </summary>
        /// <returns>A new array containing the elements contained in this collection</returns>
        public Element[] ToArray()
        {
            return this.Elements.ToArray();
        }

        /// <summary>
        /// Returns a type safe enumerator that iterates through this collection
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
        {
            return this.Elements.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through this collection
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Elements.GetEnumerator();
        }

        #region Internal
        internal List<Element> Elements { get; private set; }
        internal Element Parent { get; private set; }

        internal ElementCollection(Element parent)
        {
            this.Parent = parent;
            this.Elements = new List<Element>();
        }
        #endregion
    }
}
