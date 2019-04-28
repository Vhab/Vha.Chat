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
}
