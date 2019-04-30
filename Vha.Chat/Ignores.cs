/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all acknowledgements.
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
using System.IO;
using Vha.Chat.Data;
using Vha.Chat.Events;
using Vha.Common;

namespace Vha.Chat
{
    // Placeholder till I have the chance to port the Ignore system
    public class Ignores
    {
        /// <summary>
        /// Fires because of modifications to the configuration or from the outside.
        /// This event means the entire ignore list has been reloaded and any number of characters may have been added or removed.
        /// </summary>
        public event Handler ReloadedEvent;

        /// <summary>
        /// Fires when a character is added to the ignore list
        /// </summary>
        public event Handler<IgnoreEventArgs> AddedEvent;

        /// <summary>
        /// Fires when a character is removed from the ignore list
        /// </summary>
        public event Handler<IgnoreEventArgs> RemovedEvent;

        /// <summary>
        /// Whether it's currently sensible to query the ignore list
        /// </summary>
        public bool Active
        {
            get
            {
                if (this._context.State != ContextState.Connected &&
                this._context.State != ContextState.CharacterSelection)
                    return false;
                return true;
            }
        }
        /// <summary>
        /// Returns the amount of characters currently being actively ignored
        /// </summary>
        public int Count
        {
            get
            {
                lock (this) return this._ignored.Count;
            }
        }
        /// <summary>
        /// Checks if the given character is ignored by this client.
        /// This method will also account for any name changes as long as the character id remains the same.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool Contains(string character)
        {
            if (!this.Active) return false;
            // Find the character id
            character = Format.UppercaseFirst(character);
            UInt32 characterID = this._context.Chat.GetCharacterID(character);
            // We can't ignore ourselves
            if (characterID == this._context.CharacterID) return false;
            // I don't think we're ignoring characters who don't exist
            if (characterID == 0) return false;
            lock (this)
            {
                if (!this._ignored.ContainsKey(characterID))
                    return false;
                // Check for name change
                if (this._ignored[characterID] != character)
                    this.Add(character);
                return true;
            }
        }

        public void Add(string character)
        {
            if (!this.Active)
                throw new InvalidOperationException("The ignore list cannot be modified in the current state");
            // Find the character id
            character = Format.UppercaseFirst(character);
            UInt32 characterID = this._context.Chat.GetCharacterID(character);
            // Can't ignore that which does not exist
            if (characterID == 0) return;
            // And now, the action!
            lock (this)
            {
                // Remove previous conflicting entries
                bool changed;
                do
                {
                    changed = false;
                    foreach (IgnoresV1Entry entry in this._data.Entries)
                    {
                        if (entry.Dimension != this._context.Dimension)
                            continue;
                        if (entry.Account != this._context.Account)
                            continue;
                        if (entry.ID != this._context.CharacterID)
                            continue;
                        if (entry.CharacterID != characterID)
                            continue;
                        changed = true;
                        this._data.Entries.Remove(entry);
                        break;
                    }
                }
                while (changed);
                // Add new entry
                IgnoresV1Entry newEntry = new IgnoresV1Entry();
                newEntry.Dimension = this._context.Dimension;
                newEntry.Account = this._context.Account;
                newEntry.ID = this._context.CharacterID;
                newEntry.Character = character;
                newEntry.CharacterID = characterID;
                this._data.Entries.Add(newEntry);
                // Update cache
                if (this._ignored.ContainsKey(characterID))
                    this._ignored[characterID] = character;
                else this._ignored.Add(characterID, character);
                // Save changes
                this._save();
            }
            // Fire event
            if (this.AddedEvent != null)
                this.AddedEvent(this._context, new IgnoreEventArgs(character, characterID, true));
        }

        public void Remove(string character)
        {
            if (!this.Active)
                throw new InvalidOperationException("The ignore list cannot be modified in the current state");
            // Find the character id
            character = Format.UppercaseFirst(character);
            UInt32 characterID = this._context.Chat.GetCharacterID(character);
            // Remove the ignore
            lock (this)
            {
                // Remove all matching entries
                bool changed;
                do
                {
                    changed = false;
                    foreach (IgnoresV1Entry entry in this._data.Entries)
                    {
                        if (!this._matchesIgnoreMethod(entry))
                            continue;
                        if (entry.CharacterID != characterID && entry.Character != character)
                            continue;
                        changed = true;
                        this._data.Entries.Remove(entry);
                        // Remove from cache by id
                        this._ignored.Remove(entry.CharacterID);
                        break;
                    }
                }
                while (changed);
                // Save changes
                this._save();
            }
            // Fire event
            if (this.RemovedEvent != null)
                this.RemovedEvent(this._context, new IgnoreEventArgs(character, characterID, false));
        }

        public IgnoreResult Toggle(string character)
        {
            // Find the character id
            character = Format.UppercaseFirst(character);
            UInt32 characterID = this._context.Chat.GetCharacterID(character);
            if (characterID == 0) return IgnoreResult.Error;
            lock (this)
            {
                bool exists = false;
                foreach (IgnoresV1Entry entry in this._data.Entries)
                {
                    if (!this._matchesIgnoreMethod(entry))
                        continue;
                    if (entry.CharacterID != characterID && entry.Character != character)
                        continue;
                    exists = true;
                    break;
                }
                if (exists)
                {
                    this.Remove(character);
                    return IgnoreResult.Removed;
                }
                else
                {
                    this.Add(character);
                    return IgnoreResult.Added;
                }
            }
        }

        public string[] GetCharacters()
        {
            List<string> characters = new List<string>();
            lock (this)
            {
                foreach (string character in this._ignored.Values)
                    characters.Add(character);
            }
            characters.Sort();
            return characters.ToArray();
        }

        #region Internal
        internal Ignores(Context context)
        {
            this._context = context;
            this._context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            if (!this._context.Configuration.ReadOnly)
            {
                string path = this._context.Configuration.IgnoresFilePath;
                this._watcher = new Watcher(new IgnoresV1(), path);
            }
            else
            {
                this._watcher = new Watcher(new IgnoresV1());
            }
            this._watcher.LoadedEvent += new WatcherHandler(_watcher_LoadedEvent);
            this._watcher.Load();
            this._data = (IgnoresV1)this._watcher.Data;
            this._context.Options.ModifiedEvent += new Handler<Options>(_options_ModifiedEvent);
        }

        #region Private
        private Context _context;
        private Watcher _watcher;
        private Dictionary<UInt32, string> _ignored = new Dictionary<uint, string>();
        private IgnoresV1 _data = null;

        private void _rebuild()
        {
            // Rebuild the ignore cache
            lock (this)
            {
                this._ignored.Clear();
                foreach (IgnoresV1Entry entry in this._data.Entries)
                {
                    if (!this._matchesIgnoreMethod(entry)) continue;
                    if (this._ignored.ContainsKey(entry.CharacterID)) continue;
                    this._ignored.Add(entry.CharacterID, entry.Character);
                }
            }
            // Fire event
            if (this.ReloadedEvent != null)
                this.ReloadedEvent(this._context);
        }

        private void _save()
        {
            this._watcher.Save();
        }

        private bool _matchesIgnoreMethod(IgnoresV1Entry entry)
        {
            // Check connection state
            if (!this.Active) return false;
            // Check IgnoreMethod
            switch (this._context.Options.IgnoreMethod)
            {
                case IgnoreMethod.None:
                    return false;
                case IgnoreMethod.Dimension:
                    return (entry.Dimension == this._context.Dimension);
                case IgnoreMethod.Account:
                    if (entry.Dimension != this._context.Dimension) return false;
                    return (entry.Account == this._context.Account);
                case IgnoreMethod.Character:
                    if (entry.Dimension != this._context.Dimension) return false;
                    // Character id's are unique per dimension,
                    // so there is no need to check the account value
                    return (entry.ID == this._context.CharacterID);
                default:
                    return false;
            }
        }

        void _context_StateEvent(Context context, StateEventArgs args)
        {
            // Whenever the state changes, rebuild the cache
            this._rebuild();
        }

        private void _watcher_LoadedEvent(Watcher watcher)
        {
            this._data = (IgnoresV1)watcher.Data;
            // The data has been modified from the outside
            this._rebuild();
        }

        void _options_ModifiedEvent(Context context, Options args)
        {
            // Rebuild in case ignore method was changed
            this._rebuild();
        }
        #endregion
        #endregion
    }
}