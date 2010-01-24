/*
* Vha.MDB
* Copyright (C) 2005-2010 Remco van Oosterhout
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Vha.MDB
{
    [XmlRoot("MMDB")]
    public class Reader
    {
        private string _file;
        private Int32 _endOfCategories = 0;
        private Int32 _endOfEntries = 0;
        private List<Category> _categories;
        private bool _isLoaded;
        private Mode _mode = Mode.Unknown;

        [XmlIgnore()]
        public string MmdbFile { get { return this._file; } }
        [XmlElement("Category")]
        public Category[] Categories
        {
            get { return this._categories.ToArray(); }
            set
            {
                this._categories = new List<Category>();
                foreach (Category category in value)
                    this._categories.Add(category);
            }
        }
        [XmlIgnore()]
        public bool IsLoaded { get { return this._isLoaded; } }
        [XmlIgnore()]
        public Mode Mode { get { return this._mode; } }

        public Reader()
        {
            this._categories = new List<Category>();
            this._file = "";
        }

        public Reader(string file)
        {
            this._categories = new List<Category>();
            this._file = file;
        }

        public bool Read()
        {
            Stream stream = this.Open(this._file);
            if (stream == null)
                return false;

            try
            {
                this.DetectType(stream);
                if (this._mode == Mode.MMDB)
                    return this.MmdbRead(stream);
                else if (this._mode == Mode.MLDB)
                    return this.MldbRead(stream);
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public Entry SpeedRead(Int32 categoryID, Int32 entryID)
        {
            Stream stream = this.Open(this._file);
            if (stream == null)
                return null;

            try
            {
                this.DetectType(stream);
                if (this._mode != Mode.MMDB)
                    throw new NotImplementedException("Speed Reading is only available for MMDB files!");

                stream.Seek(8, SeekOrigin.Begin);

                while (true)
                {
                    Int32 catID = 0;
                    Int32 catOffset = 0;

                    if (!this.MmdbReadKey(stream, ref catID, ref catOffset))
                        return null;

                    if (catID == -1)
                        return null;

                    if (catID == categoryID)
                    {
                        stream.Seek(catOffset, SeekOrigin.Begin);
                        while (true)
                        {
                            Int32 entID = 0;
                            Int32 entOffset = 0;

                            if (!this.MmdbReadKey(stream, ref entID, ref entOffset))
                                return null;

                            if (entID == entryID)
                            {
                                String message = String.Empty;
                                if (!this.MmdbReadString(stream, entOffset, ref message))
                                    return null;

                                return new Entry(entID, entOffset, message);
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return null;
        }

        private Stream Open(string file)
        {
            Stream stream;
            try
            {
                if (file != "") stream = File.OpenRead(file);
                else stream = new MemoryStream(Properties.Resources.Text);
            }
            catch
            {
                Trace.WriteLine("[MDB] Unable to open " + this._file);
                return null;
            }

            // Some Checks
            if (!stream.CanRead)
            {
                Trace.WriteLine("[MDB] Unable to read " + this._file);
                return null;
            }
            if (!stream.CanSeek)
            {
                Trace.WriteLine("[MDB] Unable to seek " + this._file);
                return null;
            }
            return stream;
        }

        private void DetectType(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                String type = Encoding.UTF8.GetString(buffer);
                if (type.ToLower() == "mmdb")
                {
                    this._mode = Mode.MMDB;
                }
                if (type.ToLower() == "mldb")
                {
                    this._mode = Mode.MLDB;
                }
            }
            catch { }
        }

        #region MMDB Reader
        protected bool MmdbRead(Stream stream)
        {
            // Go to start position
            stream.Seek(8, SeekOrigin.Begin);
            try
            {
                // Read categories
                Int32 currentLocation = 8;
                bool notFinishedCategories = true;
                while (notFinishedCategories)
                {
                    Int32 categoryID = 0;
                    Int32 offset = 0;

                    if (!this.MmdbReadKey(stream, ref categoryID, ref offset))
                        throw new Exception();

                    currentLocation += 8;

                    if (categoryID == -1)
                    {
                        this._endOfCategories = currentLocation;
                        this._endOfEntries = offset;
                        notFinishedCategories = false;
                    }
                    else
                    {
                        Category category = new Category(categoryID, offset);
                        this._categories.Add(category);
                    }

                }

                // Read Entries
                bool notFinishedEntries = true;
                while (notFinishedEntries)
                {
                    stream.Seek(currentLocation, SeekOrigin.Begin);

                    Int32 entryID = 0;
                    Int32 offset = 0;
                    string message = String.Empty;

                    if (!this.MmdbReadKey(stream, ref entryID, ref offset))
                        throw new Exception();

                    if (!this.MmdbReadString(stream, offset, ref message))
                        throw new Exception();

                    Entry entry = new Entry(entryID, offset, message);

                    Category category = null;
                    foreach (Category cat in this._categories)
                    {
                        if (cat.Offset > currentLocation)
                            break;
                        category = cat;
                    }
                    if (category != null)
                        category.Add(entry);

                    currentLocation += 8;
                    if (currentLocation >= this._endOfEntries)
                        break;
                }
            }
            catch
            {
                Trace.WriteLine("[MDB] Error during reading " + this._file);
                return false;
            }
            if (stream != null)
                stream.Close();

            this._isLoaded = true;
            return true;
        }

        protected bool MmdbReadKey(Stream stream, ref Int32 ID, ref Int32 Offset)
        {
            try
            {
                byte[] buffer = new byte[4];

                // Read ID
                stream.Read(buffer, 0, buffer.Length);
                ID = BitConverter.ToInt32(buffer, 0);

                // Read Offset
                stream.Read(buffer, 0, buffer.Length);
                Offset = BitConverter.ToInt32(buffer, 0);

                return true;
            }
            catch { return false; }
        }

        protected bool MmdbReadString(Stream stream, Int32 offset, ref string Message)
        {
            try
            {
                byte[] buffer = new byte[1];

                stream.Seek(offset, SeekOrigin.Begin);
                while (true)
                {
                    stream.Read(buffer, 0, buffer.Length);
                    if ((Int32)buffer[0] == 0)
                        break;

                    Message += Encoding.UTF8.GetString(buffer);
                }
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region MLDB Reader
        protected bool MldbRead(Stream stream)
        {
            stream.Seek(4, SeekOrigin.Begin);
            try
            {
                while (true)
                {
                    Int32 categoryID = 0;
                    if (!this.MldbReadInteger(stream, ref categoryID))
                        return false;

                    if (categoryID == 0)
                    {
                        this._isLoaded = true;
                        return true;
                    }

                    Category category = this.GetCategory(categoryID);
                    if (category == null)
                    {
                        category = new Category(categoryID, 0);
                        this._categories.Add(category);
                    }

                    Int32 entryID = 0;
                    if (!this.MldbReadInteger(stream, ref entryID))
                        return false;

                    Int32 size = 0;
                    if (!this.MldbReadInteger(stream, ref size))
                        return false;

                    byte[] buffer = new byte[size];
                    stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer);

                    Entry entry = new Entry(entryID, 0, message);
                    category.Add(entry);
                }
            }
            catch { }
            return false;
        }

        protected bool MldbReadInteger(Stream stream, ref Int32 Integer)
        {
            try
            {
                byte[] buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                Integer = BitConverter.ToInt32(buffer, 0);
                return true;
            }
            catch { return false; }
        }
        #endregion

        public Category GetCategory(Int32 categoryID)
        {
            if (this._categories != null)
                lock (this._categories)
                    foreach (Category category in this._categories)
                        if (category.CategoryID == categoryID)
                            return category;

            return null;
        }

        public Entry GetEntry(Int32 categoryID, Int32 entryID)
        {
            if (!this._isLoaded)
                return this.SpeedRead(categoryID, entryID);

            Category category = this.GetCategory(categoryID);
            if (category == null)
                return null;

            return category.GetEntry(entryID);
        }

        public void ToXml(ref Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Reader));
            serializer.Serialize(stream, this);
        }
    }
}