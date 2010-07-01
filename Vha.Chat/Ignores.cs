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
using Vha.Chat.Data;
using Vha.Common;

namespace Vha.Chat
{
    // Placeholder till I have the chance to port the Ignore system
    public class Ignores
    {
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
        /// Checks if the given user is ignored by this client.
        /// This method will also account for any name changes as long as the character id remains the same.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Contains(string user)
        {
            if (!this.Active) return false;
            // Find the character id
            user = Format.UppercaseFirst(user);
            UInt32 userId = this._context.Chat.GetUserID(user);
            // We can't ignore ourselves
            if (userId == this._context.CharacterId) return false;
            // I don't think we're ignoring characters who don't exist
            if (userId == 0) return false;
            lock (this)
            {
                if (!this._ignored.ContainsKey(userId))
                    return false;
                // Check for name change
                if (this._ignored[userId] != user)
                    this.Add(user);
                return true;
            }
        }

        public void Add(string user)
        {
            if (!this.Active)
                throw new InvalidOperationException("The ignore list cannot be modified in the current state");
            // Find the character id
            user = Format.UppercaseFirst(user);
            UInt32 userId = this._context.Chat.GetUserID(user);
            // Can't ignore that which does not exist
            if (userId == 0) return;
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
                        if (entry.ID != this._context.CharacterId)
                            continue;
                        if (entry.CharacterID != userId)
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
                newEntry.ID = this._context.CharacterId;
                newEntry.Character = user;
                newEntry.CharacterID = userId;
                this._data.Entries.Add(newEntry);
                // Update cache
                if (this._ignored.ContainsKey(userId))
                    this._ignored[userId] = user;
                else this._ignored.Add(userId, user);
                // Save changes
                this._save();
            }
        }

        public void Remove(string user)
        {
            if (!this.Active)
                throw new InvalidOperationException("The ignore list cannot be modified in the current state");
            // Find the character id
            user = Format.UppercaseFirst(user);
            UInt32 userId = this._context.Chat.GetUserID(user);
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
                        if (entry.CharacterID != userId && entry.Character != user)
                            continue;
                        changed = true;
                        this._data.Entries.Remove(entry);
                        break;
                    }
                }
                while (changed);
                // Remove from cache by id
                if (this._ignored.ContainsKey(userId))
                {
                    this._ignored.Remove(userId);
                }
                // Save changes
                this._save();
            }
        }

        public IgnoreResult Toggle(string user)
        {
            // Find the character id
            user = Format.UppercaseFirst(user);
            UInt32 userId = this._context.Chat.GetUserID(user);
            if (userId == 0) return IgnoreResult.Error;
            lock (this)
            {
                bool exists = false;
                foreach (IgnoresV1Entry entry in this._data.Entries)
                {
                    if (!this._matchesIgnoreMethod(entry))
                        continue;
                    if (entry.CharacterID != userId && entry.Character != user)
                        continue;
                    exists = true;
                    break;
                }
                if (exists)
                {
                    this.Remove(user);
                    return IgnoreResult.Removed;
                }
                else
                {
                    this.Add(user);
                    return IgnoreResult.Added;
                }
            }
        }

        #region Internal
        private Context _context;
        private Dictionary<UInt32, string> _ignored = new Dictionary<uint, string>();
        private IgnoresV1 _data = null;

        internal Ignores(Context context)
        {
            this._context = context;
            this._context.StateEvent += new Handler<Vha.Chat.Events.StateEventArgs>(_context_StateEvent);
            this._data = new IgnoresV1();
            this._load();
        }

        #region Private methods
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
        }

        private void _load()
        {
            string file = this._context.Configuration.OptionsPath + this._context.Configuration.IgnoresFile;
            Base data = Base.Load(file);
            if (data == null) return;
            if (data.Type != typeof(IgnoresV1))
                throw new ArgumentException("Invalid ignores data type: " + data.Type.ToString() + " when loading file: " + file);
            this._data = (IgnoresV1)data;
            this._rebuild();
        }

        private void _save()
        {
            string file = this._context.Configuration.OptionsPath + this._context.Configuration.IgnoresFile;
            this._data.Save(file);
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
                    return (entry.ID == this._context.CharacterId);
                default:
                    return false;
            }
        }

        void _context_StateEvent(Context context, Vha.Chat.Events.StateEventArgs args)
        {
            // Whenever the state changes, rebuild the cache
            this._rebuild();
        }
        #endregion
        #endregion
    }
}