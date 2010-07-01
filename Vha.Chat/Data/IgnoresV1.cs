/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.Xml.Serialization;

namespace Vha.Chat.Data
{
    [XmlRoot("Root")]
    public class IgnoresV1 : Base
    {
        [XmlElement("Entry")]
        public List<IgnoresV1Entry> Entries = new List<IgnoresV1Entry>();

        #region Implement Base
        public IgnoresV1()
            : base("Ignores", 1, false, typeof(IgnoresV1))
        { }

        public override Base Upgrade() { return null; }
        #endregion
    }

    public class IgnoresV1Entry
    {
        /// <summary>
        /// The dimension on which this character was ignored
        /// </summary>
        [XmlAttribute("Dimension")]
        public string Dimension = null;
        /// <summary>
        /// The account this character was ignored by
        /// </summary>
        [XmlAttribute("Account")]
        public string Account = null;
        /// <summary>
        /// The character id this character was ignored by
        /// </summary>
        [XmlAttribute("ID")]
        public uint ID = 0;
        /// <summary>
        /// The character that is ignored's id
        /// </summary>
        [XmlAttribute("CharacterID")]
        public UInt32 CharacterID = 0;
        /// <summary>
        /// The character that is ignored
        /// </summary>
        [XmlAttribute("Character")]
        public string Character = null;
    }
}
