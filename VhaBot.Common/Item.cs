/*
* VhaBot - Barbaric Edition
* Copyright (C) 2005-2008 Remco van Oosterhout
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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace VhaBot.Common
{
    public class Item
    {
        public readonly string Name;
        public readonly Int32 LowID;
        public readonly Int32 HighID;
        public readonly Int32 QL;
        public readonly string Hash1;
        public readonly string Hash2;
        public readonly string Raw;

        public Item(string name, Int32 lowid, Int32 highid, Int32 ql, string hash1, string hash2, string raw)
        {
            this.Name = name;
            this.LowID = lowid;
            this.HighID = highid;
            this.QL = ql;
            this.Hash1 = hash1;
            this.Hash2 = hash2;
            this.Raw = raw;
        }

        public override string ToString()
        {
            return HTML.StripTags(this.Name);
        }

        public string ToLink()
        {
            return HTML.CreateItem(HTML.StripTags(this.Name), this.LowID, this.HighID, this.QL, this.Hash1, this.Hash2);
        }

        private static Regex Regex;
        public static Item[] ParseString(string raw)
        {
            if (raw == null || raw == string.Empty)
                return new Item[0];
            if (Item.Regex == null)
                Item.Regex = new Regex("<a([^>]*)href=\"itemref://([0-9]+)/([0-9]+)/([0-9]{1,4})/([^/\"]+)/([^\"]+)\">(.+?)</a>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<Item> items = new List<Item>();
            MatchCollection matches = Item.Regex.Matches(raw);
            foreach (Match match in matches)
            {
                try
                {
                    string name = match.Groups[7].Value;
                    Int32 lowid = Convert.ToInt32(match.Groups[2].Value);
                    Int32 highid = Convert.ToInt32(match.Groups[3].Value);
                    Int32 ql = Convert.ToInt32(match.Groups[4].Value);
                    string hash1 = match.Groups[5].Value;
                    string hash2 = match.Groups[6].Value;
                    items.Add(new Item(name, lowid, highid, ql, hash1, hash2, match.Groups[0].Value));
                }
                catch { }
            }
            return items.ToArray();
        }
    }
}
