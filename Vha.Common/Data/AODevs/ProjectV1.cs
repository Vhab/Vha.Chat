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

namespace Vha.Common.Data.AODevs
{
    [XmlRoot("Root")]
    public class ProjectV1 : Base
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
        public List<PageV1> Pages = new List<PageV1>();

        [XmlArray("Staff")]
        [XmlArrayItem("Member")]
        public List<MemberV1> Staff = new List<MemberV1>();

        [XmlArray("Links")]
        [XmlArrayItem("Link")]
        public List<LinkV1> Links = new List<LinkV1>();

        [XmlArray("Versions")]
        [XmlArrayItem("Version")]
        public List<VersionV1> Versions = new List<VersionV1>();

        [XmlArray("Packages")]
        [XmlArrayItem("Package")]
        public List<PackageV1> Packages = new List<PackageV1>();

        #region Implement Base
        public ProjectV1()
            : base("Project", 1, false, typeof(ProjectV1))
        { }

        public override Base Upgrade() { return null; }
        #endregion
    }
}
