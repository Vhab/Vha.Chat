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
using System.Text;
using System.Xml.Serialization;

namespace Vha.Chat
{
    public class Ignore
    {
        /*
         * All methods *really* only need the uid field.
         * However, since there is no way to lookup an uid into a name, we have to store the name that the user ignored,
         * so that we can list sensible character names to the user.
         * -- Demoder
         */
        #region Members
        private Entries _entries = null;
        private bool _synchronized = false;
        private string _file = string.Empty;

        /// <summary>
        /// Am I syncronizing to disk?
        /// </summary>
        public bool Syncronized { get { return this._synchronized; } set { this._synchronized = value; } }
        /// <summary>
        /// Where do I store my data?
        /// </summary>
        public string File { get { return this._file; } }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize an empty ignore list.
        /// </summary>
        public Ignore()
        {
            this._entries = new Entries();
            this._file = "";
            this._synchronized = false;
        }

        /// <summary>
        /// Initialize a new Ignore object, the simple way
        /// </summary>
        /// <param name="file">Path to xml file representing this ignore list</param>
        /// <param name="synchronized">Should ignores/unignores be synchronized to disk immediately?</param>
        public Ignore(string file, bool synchronized)
        {
            this._entries = Common.XML<Entries>.FromFile(file);
            if (this._entries == null) this._entries = new Entries();
            this._file = file;
            this._synchronized = synchronized;
        }

        /// <summary>
        /// Saves the ignore list to the file path associated with this instance
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            lock (this._file)
            {
                if (string.IsNullOrEmpty(this._file)) return false;
                return this.Save(this._file, false);
            }
        }
        /// <summary>
        /// Saves the ignore list to file, updates internal record of file location to provided path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Save(string path) { return this.Save(path, true); }
        

        /// <summary>
        /// Saves the ignore list to file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="UpdateFilePath">Should we update the default file path?</param>
        /// <returns></returns>
        public bool Save(string path, bool UpdateFilePath)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (UpdateFilePath) //Should we update the file path?
                lock (this._file)
                    this._file = path;
            lock (this._entries)
                return Common.XML<Entries>.ToFile(path, this._entries);
        }

        /// <summary>
        /// If already ignored, unignore. If not ignored, ignore. Returns: true if user becomes ignored, false otherwise.
        /// </summary>
        /// <param name="uid">UserID to ignore</param>
        /// <returns>true when ignoring, false when unignoring.</returns>
        public bool Toggle(uint uid, string name)
        {
            if (uid == uint.MinValue || uid == uint.MaxValue) return false;
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            if (this.Contains(uid, name))
            {
                this.Remove(uid, name);
                return false;
            }
            else
            {
                this.Add(uid, name);
                return true;
            }
        }

        /// <summary>
        /// Retrieve an uint[] containing all ignored uids
        /// </summary>
        /// <returns></returns>
        public uint[] ToIDArray()
        {
            lock (this._entries)
            {
                List<uint> output = new List<uint>();
                foreach (Entry ie in this._entries.Items)
                {
                    output.Add(ie.ID);
                }
                output.Sort(delegate(uint A, uint B) { return A.CompareTo(B); });
                return output.ToArray();
            }
        }

        /// <summary>
        /// Retrieve a string[] containing all ignored unames
        /// </summary>
        /// <returns></returns>
        public string[] ToNameArray()
        {
            lock (this._entries)
            {
                List<string> output = new List<string>();
                foreach (Entry ie in this._entries.Items)
                {
                    output.Add(ie.Name);
                }
                output.Sort(delegate(string A, string B) { return A.CompareTo(B); });
                return output.ToArray();
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Combines two ignore lists into one. Ignore.File will be nilled.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Ignore operator +(Ignore i1, Ignore i2)
        {
            Ignore ignore = new Ignore();
            // Add first
            uint[] ids1 = i1.ToIDArray();
            string[] names1 = i1.ToNameArray();
            for (int i = 0; i < ids1.Length; i++)
            {
                ignore.Add(ids1[i], names1[i]);
            }
            // Add second
            uint[] ids2 = i2.ToIDArray();
            string[] names2 = i2.ToNameArray();
            for (int i = 0; i < ids2.Length; i++)
            {
                ignore.Add(ids2[i], names2[i]);
            }
            if (i1._synchronized || i2._synchronized) ignore.Syncronized = true;
            return ignore;
        }
        #endregion

        /// <summary>
        /// Find out if a given uid is ignored.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(uint uid, string name)
        {
            if (uid == uint.MinValue || uid == uint.MaxValue) return false;
            lock (this._entries)
            {
                name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
                bool changed = true;
                while (changed)
                {
                    changed = false;
                    foreach (Entry ie in _entries.Items)
                    {
                        //If entries are found where uid!=name, remove.
                        if (ie.ID == uid && ie.Name != name)
                        { //UID is in list, but with the wrong name. Update the name.
                            this._entries.Items.Remove(ie);
                            this.Add(uid, name);
                            return true;
                        }
                        else if (ie.ID != uid && ie.Name == name)
                        {
                            this._entries.Items.Remove(ie);
                            changed = true;
                            break;
                        }
                        else if (uid == ie.ID) { return true; }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Ignore a given user. Always returns true.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns>always true</returns>
        public bool Add(uint uid, string name)
        {
            if (uid == uint.MinValue || uid == uint.MaxValue) return false;
            lock (this)
            {
                name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
                Entry tmp = new Entry();
                tmp.ID = uid;
                tmp.Name = name;
                if (this._entries.Items.Contains(tmp)) this.Remove(uid, name); //Remove any previous matches.
                this._entries.Items.Add(tmp);
                if (this._synchronized && !string.IsNullOrEmpty(this._file)) Common.XML<Entries>.ToFile(this._file, this._entries);
                return true;
            }
        }

        /// <summary>
        /// Unignore a given user. Always returns false
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns>false </returns>
        public bool Remove(uint uid, string name)
        {
            if (uid == uint.MinValue || uid == uint.MaxValue) return false;
            lock (this)
            {
                //Remove all entries where (uid == uid || name==name).
                name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
                bool changed = true;
                while (changed)
                {
                    changed = false;
                    foreach (Entry ie in this._entries.Items)
                    {
                        if (ie.ID == uid || ie.Name == name)
                        {
                            this._entries.Items.Remove(ie);
                            changed = true;
                            break;
                        }
                    }
                }
                if (this._synchronized && !string.IsNullOrEmpty(this._file)) Common.XML<Entries>.ToFile(this._file, this._entries);
                return false;
            }
        }

        /// <summary>
        /// Retrieve a comma-separated list of ignored characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            lock (this._entries)
            {
                List<string> output = new List<string>();
                foreach (Entry ie in this._entries.Items)
                {
                    output.Add(ie.Name);
                }
                output.Sort(delegate(string A, string B) { return A.CompareTo(B); });
                return string.Join(", ", output.ToArray());
            }
        }
        public int Count()
        {
            lock (this._entries)
            {
                return this._entries.Items.Count;
            }
        }

        #region Classes
        [XmlRoot("Root")]
        public class Entries
        {
            [XmlElement("Entry")]
            public List<Entry> Items = new List<Entry>(); //This is so we can store uid + name. Name is to be able to list who's on ignore.
        }
        public class Entry
        {
            [XmlAttribute("ID")]
            public uint ID;
            [XmlAttribute("Name")]
            public string Name;
        }
        #endregion
    }
}