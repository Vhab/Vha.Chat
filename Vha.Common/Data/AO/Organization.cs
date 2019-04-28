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

namespace Vha.Common.Data.AO
{
    [XmlRoot("organization")]
    public class Organization
    {
        [XmlElement("id", Type = typeof(Int32))]
        public Int32 ID;
        [XmlElement("name")]
        public string Name;
        [XmlElement("side")]
        public string Faction;
        [XmlElement("last_updated")]
        public string LastUpdated;
        [XmlArray("members")]
        [XmlArrayItem("member")]
        public OrganizationMember[] Members;
        [XmlIgnore]
        public OrganizationMember Leader
        {
            get
            {
                OrganizationMember retMember = null;
                foreach (OrganizationMember member in this.Members)
                {
                    if (member.RankID == 0)
                    {
                        // If we have already assigned a leader, and we stumble across another one; there's multiple leaders. Return null.
                        if (retMember != null)
                            return null;
                        retMember = member;
                    }
                    // If this member doesn't have rank 0, and retMember is set, break the loop
                    else if (retMember != null)
                        break;
                }
                // Return the chosen member
                return retMember;
            }
        }

        public OrganizationMember GetMember(string nickname)
        {
            foreach (OrganizationMember member in this.Members)
                if (member.Nickname.ToLower() == nickname.ToLower())
                    return member;
            return null;
        }
    }
}
