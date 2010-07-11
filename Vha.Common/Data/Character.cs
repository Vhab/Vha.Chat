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
using System.Xml.Serialization;

namespace Vha.Common.Data
{
    [XmlRoot("character")]
    public class Character
    {
        [XmlElement("name")]
        public CharacterName Name;
        [XmlElement("basic_stats")]
        public CharacterStats Stats;
        [XmlElement("pictureurl")]
        public string PictureUrl;
        [XmlElement("smallpictureurl")]
        public string SmallPictureUrl;
        [XmlElement("organization_membership")]
        public CharacterOrganization Organization;
        [XmlElement("last_updated")]
        public string LastUpdated;

        [XmlIgnore]
        public bool Valid
        {
            get
            {
                return (this.Name != null && this.Stats != null && this.Stats.Level > 0 && this.Stats.Profession != null && this.Stats.Profession != string.Empty);
            }
        }

        [XmlIgnore]
        public bool InOrganization
        {
            get
            {
                return (this.Organization != null && this.Organization.Name != null && this.Organization.Name != string.Empty);
            }
        }

        public override string ToString()
        {
            if (this.Name != null)
                return this.Name.ToString();
            return "";
        }
    }

    public class CharacterName
    {
        [XmlElement("firstname")]
        public string Firstname;
        [XmlElement("nick")]
        public string Nickname;
        [XmlElement("lastname")]
        public string Lastname;

        public override string ToString()
        {
            string result = "";
            if (this.Firstname != null && this.Firstname != string.Empty)
                result += this.Firstname + " ";

            result += string.Format("\"{0}\"", this.Nickname);

            if (this.Lastname != null && this.Lastname != string.Empty)
                result += " " + this.Lastname;

            return result;
        }
    }

    public class CharacterStats
    {
        [XmlElement("level", Type = typeof(Int32))]
        public Int32 Level;
        [XmlElement("breed")]
        public string Breed;
        [XmlElement("gender")]
        public string Gender;
        [XmlElement("faction")]
        public string Faction;
        [XmlElement("profession")]
        public string Profession;
        [XmlElement("profession_title")]
        public string Title;
        [XmlElement("defender_rank")]
        public string DefenderRank;
        [XmlElement("defender_rank_id", Type = typeof(Int32))]
        public Int32 DefenderLevel = 0;
    }

    public class CharacterOrganization
    {
        [XmlElement("organization_id", Type = typeof(Int32))]
        public Int32 ID = 0;
        [XmlElement("organization_name")]
        public string Name;
        [XmlElement("rank")]
        public string Rank;
        [XmlElement("rank_id", Type = typeof(Int32))]
        public Int32 RankID = 0;

        public override string ToString()
        {
            return this.Rank + " of " + this.Name;
        }
    }
}
