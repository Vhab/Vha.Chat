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
using System.IO;

namespace Vha.Chat.Data
{

    public delegate void WatcherHandler(Watcher watcher);

    public class Watcher
    {
        /// <summary>
        /// The directory this watcher is currently observing
        /// </summary>
        public string Directory { get { return this._directory; } }
        /// <summary>
        /// The file this watcher is currently observing
        /// </summary>
        public string File { get { return this._file; } }
        /// <summary>
        /// The combined directory and file name this watcher is currently observing
        /// </summary>
        public string FullPath { get { return this._path; } }
        /// <summary>
        /// Either the data supplied at creation or freshly on-demand loaded data
        /// </summary>
        public Base Data { get { return this._data; } }
        /// <summary>
        /// Triggered when the current data is written to disk
        /// </summary>
        public event WatcherHandler SavedEvent;
        /// <summary>
        /// Triggered when new data is read from disk
        /// </summary>
        public event WatcherHandler LoadedEvent;
        /// <summary>
        /// Saves the current data to disk
        /// </summary>
        public void Save()
        {
            lock (this)
            {
                this._watcher.EnableRaisingEvents = false;
                this._data.Save(this._path);
                this._watcher.EnableRaisingEvents = true;
            }
            if (this.SavedEvent != null)
                this.SavedEvent(this);
        }
        /// <summary>
        /// Loads new data from disk
        /// </summary>
        public void Load()
        {
            lock (this)
            {
                Base data = Base.Load(this._path);
                if (data == null) return;
                if (data.Type != this._data.Type)
                {
                    throw new InvalidOperationException(
                        "Watcher loaded data file of type '" +
                        data.Type.ToString() +
                        "' but was expecting data of type '" +
                        this._data.Type.ToString() +
                        "' for file: " + this.File);
                }
                this._data = data;
            }
            if (this.LoadedEvent != null)
                this.LoadedEvent(this);
        }
        /// <summary>
        /// Reset the watcher with a new set of data
        /// </summary>
        /// <param name="data"></param>
        public void Reset(Base data)
        {
            lock (this)
            {
                if (data == null)
                    throw new ArgumentNullException();
                if (data.Type != this._data.Type)
                {
                    throw new InvalidOperationException(
                        "Received new data of type '" +
                        data.Type.ToString() +
                        "' but was expecting data of type '" +
                        this._data.Type.ToString());
                }
                this._data = data;
            }
            // Trigger load event
            if (this.LoadedEvent != null)
                this.LoadedEvent(this);
            // Save data to disk
            this.Save();
        }

        public Watcher(Base data, string path)
        {
            this._directory = Path.GetDirectoryName(path);
            this._file = Path.GetFileName(path);
            this._path = path;
            this._data = data;
            this._watcher = new FileSystemWatcher(this._directory, this._file);
            this._watcher.NotifyFilter = NotifyFilters.LastWrite;
            this._watcher.Changed += new FileSystemEventHandler(_watcher_Changed);
            this._watcher.EnableRaisingEvents = true;
        }

        #region Internal
        private string _directory;
        private string _file;
        private string _path;
        private Base _data;
        private FileSystemWatcher _watcher;

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            this.Load();
        }
        #endregion
    }
}
