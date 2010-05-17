/*
* Vha.Common
* Copyright (C) 2005-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Vha.Common
{
    public class Item
    {
        public readonly string Name;
        public readonly Int32 LowID;
        public readonly Int32 HighID;
        public readonly Int32 QL;
        public readonly string Raw;

        public Item(string name, Int32 lowid, Int32 highid, Int32 ql, string raw)
        {
            this.Name = name;
            this.LowID = lowid;
            this.HighID = highid;
            this.QL = ql;
            this.Raw = raw;
        }

		/// <summary>
		/// Create a representation for an item with the given parameters and automatically generate the raw string for the item.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lowid"></param>
		/// <param name="highid"></param>
		/// <param name="ql"></param>
		public Item(string name, Int32 lowid, Int32 highid, Int32 ql)
		{
			this.Name = name;
			this.LowID = lowid;
			this.HighID = highid;
			this.QL = ql;
			this.Raw = this.ToLink();
		}

        public override string ToString()
        {
            return string.Format("QL {0} {1}", this.QL, this.Name);
        }

        public string ToLink()
        {
            return HTML.CreateItem(this.Name, this.LowID, this.HighID, this.QL);
        }

        private static Regex Regex;
        public static Item[] ParseString(string raw)
        {
            if (raw == null || raw == string.Empty)
                return new Item[0];
            if (Item.Regex == null)
                Item.Regex = new Regex("<a href=\"itemref://([0-9]+)/([0-9]+)/([0-9]{1,3})\">([^<]+)</a>");

            List<Item> items = new List<Item>();
            MatchCollection matches = Item.Regex.Matches(raw);
            foreach (Match match in matches)
            {
                try
                {
                    string name = match.Groups[4].Value;
                    Int32 lowid = Convert.ToInt32(match.Groups[1].Value);
                    Int32 highid = Convert.ToInt32(match.Groups[2].Value);
                    Int32 ql = Convert.ToInt32(match.Groups[3].Value);
                    items.Add(new Item(name, lowid, highid, ql, match.Groups[0].Value));
                }
                catch { }
            }
            return items.ToArray();
        }
    }
}
