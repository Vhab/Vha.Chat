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
using System.Xml.Serialization;
using Vha.Net;
using Vha.Chat.Data;

namespace Vha.Chat
{
    /// <summary>
    /// A wrapper around Data.OptionsV1 with modification tracking
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Triggered when a setting is modified.
        /// Be advised this event can be triggered quite often and multiple times after each other.
        /// Only use this when you absolutely need to.
        /// </summary>
        public event Handler<Options> ModifiedEvent;
        /// <summary>
        /// Triggered when the current set of options are saved to disk.
        /// Also triggered when the options are changed (thus saved) from an outside application.
        /// </summary>
        public event Handler<Options> SavedEvent;

        public int MaximumMessages
        {
            get { return this._options.MaximumMessages; }
            set { this._options.MaximumMessages = value; this.Touch(); }
        }
        public int MaximumTexts
        {
            get { return this._options.MaximumTexts; }
            set { this._options.MaximumTexts = value; this.Touch(); }
        }
        public int MaximumHistory
        {
            get { return this._options.MaximumHistory; }
            set { this._options.MaximumHistory = value; this.Touch(); }
        }
        public TextStyle TextStyle
        {
            get { return this._options.TextStyle; }
            set { this._options.TextStyle = value; this.Touch(); }
        }
        public IgnoreMethod IgnoreMethod
        {
            get { return this._options.IgnoreMethod; }
            set { this._options.IgnoreMethod = value; this.Touch(); }
        }
        public OptionsProxy Proxy
        {
            get
            {
                if (this._options.Proxy == null)
                    this._options.Proxy = new OptionsV1Proxy();
                return new OptionsProxy(this, this._options.Proxy);
            }
        }
        public string LastDimension
        {
            get { return this._options.LastDimension; }
            set { this._options.LastDimension = value; this.Touch(); }
        }
        public string LastAccount
        {
            get { return this._options.LastAccount; }
            set { this._options.LastAccount = value; this.Touch(); }
        }

        public OptionsWindow[] Windows
        {
            get
            {
                List<OptionsWindow> windows = new List<OptionsWindow>();
                lock (this)
                {
                    foreach (OptionsV1Window window in this._options.Windows)
                        windows.Add(new OptionsWindow(this, window));
                }
                return windows.ToArray();
            }
        }

        public OptionsWindow GetWindow(string name) { return this.GetWindow(name, false); }
        public OptionsWindow GetWindow(string name, bool create)
        {
            lock (this)
            {
                foreach (OptionsV1Window window in this._options.Windows)
                    if (window.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return new OptionsWindow(this, window);
                if (create)
                {
                    this.Touch();
                    OptionsV1Window window = new OptionsV1Window();
                    window.Name = name;
                    this._options.Windows.Add(window);
                    return new OptionsWindow(this, window);
                }
            }
            return null;
        }

        public OptionsAccount[] Accounts
        {
            get
            {
                List<OptionsAccount> accounts = new List<OptionsAccount>();
                lock (this)
                {
                    foreach (OptionsV1Account account in this._options.Accounts)
                        accounts.Add(new OptionsAccount(this, account));
                }
                return accounts.ToArray();
            }
        }

        public OptionsAccount GetAccount(string name) { return this.GetAccount(name, false); }
        public OptionsAccount GetAccount(string name, bool create)
        {
            lock (this)
            {
                foreach (OptionsV1Account account in this._options.Accounts)
                    if (account.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return new OptionsAccount(this, account);
                if (create)
                {
                    this.Touch();
                    OptionsV1Account account = new OptionsV1Account();
                    account.Name = name;
                    this._options.Accounts.Add(account);
                    return new OptionsAccount(this, account);
                }
            }
            return null;
        }

        public bool Modified { get { return this._modified; } }

        /// <summary>
        /// Marks this object as 'modified'
        /// </summary>
        public void Touch()
        {
            this._modified = true;
            if (this.ModifiedEvent != null)
                this.ModifiedEvent(this._context, this);
        }

        public void Save()
        {
            lock (this)
            {
                this._watcher.Save();
                this._modified = false;
            }
            if (this.SavedEvent != null)
                this.SavedEvent(this._context, this);
        }

        #region Internal
        internal Options(Context context)
        {
            // Load data
            string path = context.Configuration.OptionsPath + context.Configuration.OptionsFile;
            this._watcher = new Watcher(new OptionsV1(), path);
            this._watcher.LoadedEvent += new WatcherHandler(_watcher_LoadedEvent);
            this._watcher.Load();
            // And the rest...
            this._options = (OptionsV1)this._watcher.Data;
            this._modified = false;
            this._context = context;
        }

        private Watcher _watcher;
        private OptionsV1 _options;
        private bool _modified;
        private Context _context;

        private void _watcher_LoadedEvent(Watcher watcher)
        {
            this._options = (OptionsV1)watcher.Data;
            this._modified = false;
            // The data was modified from the outside
            if (this.ModifiedEvent != null)
                this.ModifiedEvent(this._context, this);
            // The data was saved (just not by this process)
            if (this.SavedEvent != null)
                this.SavedEvent(this._context, this);
        }
        #endregion
    }

    public class OptionsProxy
    {
        public string Type
        {
            get { return this._proxy.Type; }
            set { this._proxy.Type = value; this._parent.Touch(); }
        }
        public string Address
        {
            get { return this._proxy.Address; }
            set { this._proxy.Address = value; this._parent.Touch(); }
        }
        public int Port
        {
            get { return this._proxy.Port; }
            set { this._proxy.Port = value; this._parent.Touch(); }
        }
        public string Username
        {
            get { return this._proxy.Username; }
            set { this._proxy.Username = value; this._parent.Touch(); }
        }
        public string Password
        {
            get { return this._proxy.Password; }
            set { this._proxy.Password = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsProxy(Options parent, OptionsV1Proxy proxy)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (proxy == null) throw new ArgumentNullException("proxy");
            this._parent = parent;
            this._proxy = proxy;
        }

        private Options _parent;
        private OptionsV1Proxy _proxy;
        #endregion
    }

    public class OptionsAccount
    {
        public string Dimension
        {
            get { return this._account.Dimension; }
            set { this._account.Dimension = value; this._parent.Touch(); }
        }
        public string Name
        {
            get { return this._account.Name; }
            set { this._account.Name = value; this._parent.Touch(); }
        }
        public string Character
        {
            get { return this._account.Character; }
            set { this._account.Character = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsAccount(Options parent, OptionsV1Account account)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (account == null) throw new ArgumentNullException("account");
            this._parent = parent;
            this._account = account;
        }

        private Options _parent;
        private OptionsV1Account _account;
        #endregion
    }

    public class OptionsWindow
    {
        public string Name
        {
            get { return this._window.Name; }
            set { this._window.Name = value; this._parent.Touch(); }
        }
        public int X
        {
            get { return this._window.X; }
            set { this._window.X = value; this._parent.Touch(); }
        }
        public int Y
        {
            get { return this._window.Y; }
            set { this._window.Y = value; this._parent.Touch(); }
        }
        public int Width
        {
            get { return this._window.Width; }
            set { this._window.Width = value; this._parent.Touch(); }
        }
        public int Height
        {
            get { return this._window.Height; }
            set { this._window.Height = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsWindow(Options parent, OptionsV1Window window)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (window == null) throw new ArgumentNullException("window");
            this._parent = parent;
            this._window = window;
        }

        private Options _parent;
        private OptionsV1Window _window;
        #endregion
    }
}