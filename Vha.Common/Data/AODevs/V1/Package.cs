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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vha.Common.Data.AODevs.V1
{
    public class Package
    {
        [XmlAttribute("Name")]
        public string Name;

        [XmlIgnore]
        public DateTime Date = DateTime.MinValue;

        [XmlAttribute("Date")]
        public string DateWrapper
        {
            set { this.Date = DateTime.Parse(value); }
            get
            {
                if (this.Date == DateTime.MinValue) return null;
                return string.Format(
                    "{0:00}-{1:00}-{2:0000}",
                    this.Date.Day, this.Date.Month, this.Date.Year);
            }
        }

        [XmlElement(ElementName = "Link", Type = typeof(Link)),
        XmlElement(ElementName = "File", Type = typeof(File)),
        XmlElement(ElementName = "Repository", Type = typeof(Repository))]
        public List<Reference> References = new List<Reference>();
    }
}
