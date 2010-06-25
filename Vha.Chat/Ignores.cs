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

#define SOMETHING

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Vha.Chat
{
#if SOMETHING
    // Placeholder till I have the chance to port the Ignore system
    public class Ignores
    {
        public bool Contains(string user)
        {
            return false;
        }

        internal Ignores(Context context)
        {

        }
    }
#else
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
        private string _file = string.Empty;
        private FileSystemWatcher _fsWatcher;
        private IgnoreMethod _method;
        /// <summary>
        /// My account name
        /// </summary>
        private string _account;
        public string Account { get { return this._account; } set { this._account = value; } }
        /// <summary>
        /// My UserID
        /// </summary>
        private uint _id;
        public uint ID { get { return this._id; } set { this._id = value; } }
        /// <summary>
        /// Where do I store my data?
        /// </summary>
        public string FilePath { get { return this._file; } }

        /// <summary>
        /// Which ignore method are we using?
        /// </summary>
        public IgnoreMethod Method { get { return this._method; } set { this._method = value; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize an empty Ignore object.
        /// </summary>
        public Ignore()
        {
            this._entries = new Entries();
            this._file = "";
            this._method = IgnoreMethod.None;
        }

        /// <summary>
        /// Initialize a new Ignore object, the simple way
        /// </summary>
        /// <param name="file">Path to xml file representing this ignore list</param>
        /// <param name="method">Which method to use when checking/removing ignore entries?</param>
        public Ignore(string file, IgnoreMethod method, string myAcc, uint myID)
        {
            this._account = myAcc;
            this._id = myID;
            if (!File.Exists(file)) //File doesn't exist, create it.
            {
                this._entries = new Entries();
                try
                {
                    File.Create(file);
                }
                catch { this._method = IgnoreMethod.None; return; }
            }
            else //File exists, load it.
            {
                this._entries = Common.XML<Entries>.FromFile(file);
            }
            if (this._entries == null) this._entries = new Entries();
            this._file = file;
            InitFSWatcher();
            this._method = method;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the filesystem watcher.
        /// </summary>
        private void InitFSWatcher()
        {
            //Locate directory part of path.
            int loc = this._file.LastIndexOfAny("/\\".ToCharArray()); //Find the last occurance of either / or \
            string dir = this._file.Substring(0, loc); //0 to char before last / or \
            string file = this._file.Substring(loc + 1); //Last / or \ plus one.
            this._fsWatcher = new FileSystemWatcher(dir, file);
            this._fsWatcher.Changed += new FileSystemEventHandler(_onfsEventHandler);
            this._fsWatcher.EnableRaisingEvents = true;
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
            //Fixme: Implement some way of *ensuring* changes are written out.
            //This is probably best added in the add/remove methods, or we'd end up overwriting things.
            if (string.IsNullOrEmpty(path)) return false;
            if (UpdateFilePath) //Should we update the file path?
                lock (this._file)
                {
                    this._file = path;
                    this.InitFSWatcher();
                }
            lock (this._entries)
            {
                //Check if the given file is marked as read only.
                FileInfo fi = new FileInfo(path);
                if (fi.IsReadOnly) return false;
                this._fsWatcher.EnableRaisingEvents = false;
                bool success = Common.XML<Entries>.ToFile(path, this._entries);
                this._fsWatcher.EnableRaisingEvents = true;
                return success;
            }
        }

        /// <summary>
        /// If already ignored, unignore. If not ignored, ignore. Returns: true if user becomes ignored, false otherwise.
        /// </summary>
        /// <param name="uid">UserID to ignore</param>
        /// <returns>true when ignoring, false when unignoring.</returns>
        public IgnoreResult Toggle(uint uid, string name)
        {
            if (uid == uint.MinValue || uid == uint.MaxValue) return IgnoreResult.Error;
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            if (this.Contains(uid, name))
            {
                this.Remove(uid, name);
                return IgnoreResult.Removed;
            }
            else
            {
                this.Add(uid, name);
                return IgnoreResult.Added;
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

        #region FS watch stuff
        /// <summary>
        /// Handles reports of changes to the file we are using.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void _onfsEventHandler(object obj, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                this._entries = Common.XML<Entries>.FromFile(this._file);
            }
        }
        #endregion

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
                bool changed;
                do
                {
                    changed = false;
                    foreach (Entry ie in _entries.Items)
                    {
                        if (this.MatchCurrentIgnoreMethod(ie))
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
                } while (changed);
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
                Entry tmp = new Entry(this._account, this._id, uid, name);
                if (this._entries.Items.Contains(tmp)) this.Remove(uid, name); //Remove any previous matches.
                this._entries.Items.Add(tmp);
                if (!string.IsNullOrEmpty(this._file)) this.Save();
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
                bool changed;
                do
                {
                    changed = false;
                    foreach (Entry ie in this._entries.Items)
                    {
                        if (this.MatchCurrentIgnoreMethod(ie))
                        {
                            if (ie.ID == uid || ie.Name == name)
                            {
                                this._entries.Items.Remove(ie);
                                changed = true;
                                break;
                            }
                        }
                    }
                } while (changed);

                if (!string.IsNullOrEmpty(this._file)) this.Save();
                return false;
            }
        }

        /// <summary>
        /// Find out if this entry is relevant for our current ignore method.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool MatchCurrentIgnoreMethod(Entry e)
        {
            switch (this._method)
            {
                case IgnoreMethod.None:
                default:
                    return false;
                case IgnoreMethod.Dimension:
                    return true;
                case IgnoreMethod.Account:
                    if (e.MyAccount == this._account) return true;
                    else return false;
                case IgnoreMethod.Character: //don't have to check account, since uids are unique per dim, not per account.
                    if (e.MyID == this._id) return true;
                    else return false;
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
        [XmlRoot("Ignorelist")]
        public class Entries
        {
            [XmlElement("Entry")]
            public List<Entry> Items = new List<Entry>(); //This is so we can store uid + name. Name is to be able to list who's on ignore.
        }
        public struct Entry
        {
            /// <summary>
            /// Creates an ignore entry.
            /// </summary>
            /// <param name="myAccount">Account name to associate entry with</param>
            /// <param name="myID"></param>
            /// <param name="ignoreID"></param>
            /// <param name="ignoreName"></param>
            public Entry(string myAccount, uint myID, uint ignoreID, string ignoreName)
            {
                this.ID = ignoreID;
                this.Name = ignoreName;
                this.MyAccount = myAccount;
                this.MyID = myID;
            }
            [XmlAttribute("ID")]
            public uint ID;
            [XmlAttribute("Name")]
            public string Name;
            [XmlAttribute("MyAccount")]
            public string MyAccount;
            [XmlAttribute("MyID")]
            public uint MyID;
        }
        #endregion
    }
#endif
}