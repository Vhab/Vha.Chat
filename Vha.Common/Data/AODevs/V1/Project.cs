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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vha.Common.Data.AODevs.V1
{
    [XmlRoot("Root")]
    public class Project : Base
    {
        [XmlElement("Name")]
        public string ProjectName;

        [XmlElement("Tagline")]
        public string Tagline;

        [XmlElement("Description")]
        public string Description;

        [XmlElement("License")]
        public string License;

        [XmlArray("Pages")]
        [XmlArrayItem("Page")]
        public List<Page> Pages = new List<Page>();

        [XmlArray("Staff")]
        [XmlArrayItem("Member")]
        public List<Member> Staff = new List<Member>();

        [XmlArray("Links")]
        [XmlArrayItem("Link")]
        public List<Link> Links = new List<Link>();

        [XmlArray("Versions")]
        [XmlArrayItem("Version")]
        public List<Version> Versions = new List<Version>();

        [XmlArray("Packages")]
        [XmlArrayItem("Package")]
        public List<Package> Packages = new List<Package>();

        #region Implement Base
        public Project()
            : base("Project", 1, false, typeof(Project))
        { }

        public override Base Upgrade() { return null; }
        #endregion
    }
}
