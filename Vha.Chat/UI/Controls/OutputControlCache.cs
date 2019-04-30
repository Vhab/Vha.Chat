/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.Text;
using Vha.AOML.DOM;

namespace Vha.Chat.UI.Controls
{
    public class OutputControlCache
    {
        public int ElementCacheSize
        {
            get { return this._elementCacheSize; }
            set
            {
                if (value < 0)
                    throw new IndexOutOfRangeException();
                lock (this._elements)
                {
                    this._elementCacheSize = value;
                    while (this._elements.Count > value)
                    {
                        if (this._elementCacheCurrent >= this._elements.Count)
                            this._elementCacheCurrent = 0;
                        this._elements.RemoveAt(this._elementCacheCurrent);
                    }
                }
            }
        }

        public int CacheElement(Element element)
        {
            int result = -1;
            lock (this._elements)
            {
                // Cache element
                if (this._elements.Count <= this._elementCacheCurrent)
                {
                    result = this._elementCacheCurrent = this._elements.Count;
                    this._elements.Add(element);
                }
                else
                {
                    result = this._elementCacheCurrent;
                    this._elements[result] = element;
                }
                // Update value for the next addition
                this._elementCacheCurrent++;
                if (this._elementCacheCurrent >= this._elementCacheSize)
                    this._elementCacheCurrent = 0;
            }
            // Return index
            return result;
        }

        public Element GetElement(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();
            lock (this._elements)
            {
                if (index >= this._elements.Count)
                    return null;
                return this._elements[index];
            }
        }

        private int _elementCacheSize = 50;
        private int _elementCacheCurrent = 0;
        private List<Element> _elements = new List<Element>();
    }
}
