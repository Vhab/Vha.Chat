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
        public int MessageBuffer
        {
            get { return this._options.MessageBuffer; }
            set { this._options.MessageBuffer = value; this.Touch(); }
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
        public HorizontalPosition PanelPosition
        {
            get { return this._options.PanelPosition; }
            set { this._options.PanelPosition = value; this.Touch(); }
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

        public OptionsSize GetSize(string name, string element) { return this.GetSize(name, element, false); }
        public OptionsSize GetSize(string name, string element, bool create)
        {
            lock (this)
            {
                foreach (OptionsV1Size size in this._options.Sizes)
                    if (size.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) &&
                        size.Element.Equals(element, StringComparison.CurrentCultureIgnoreCase))
                        return new OptionsSize(this, size);
                if (create)
                {
                    this.Touch();
                    OptionsV1Size size = new OptionsV1Size();
                    size.Name = name;
                    size.Element = element;
                    this._options.Sizes.Add(size);
                    return new OptionsSize(this, size);
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

        public void Reset()
        {
            lock (this)
            {
                this._watcher.Reset(new OptionsV1());
                this._modified = false;
            }
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
        public ProxyType Type
        {
            get { return this._data.Type; }
            set { this._data.Type = value; this._parent.Touch(); }
        }
        public string Address
        {
            get { return this._data.Address; }
            set { this._data.Address = value; this._parent.Touch(); }
        }
        public int Port
        {
            get { return this._data.Port; }
            set { this._data.Port = value; this._parent.Touch(); }
        }
        public string Username
        {
            get { return this._data.Username; }
            set { this._data.Username = value; this._parent.Touch(); }
        }
        public string Password
        {
            get { return this._data.Password; }
            set { this._data.Password = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsProxy(Options parent, OptionsV1Proxy proxy)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (proxy == null) throw new ArgumentNullException("proxy");
            this._parent = parent;
            this._data = proxy;
        }

        private Options _parent;
        private OptionsV1Proxy _data;
        #endregion
    }

    public class OptionsAccount
    {
        public string Dimension
        {
            get { return this._data.Dimension; }
            set { this._data.Dimension = value; this._parent.Touch(); }
        }
        public string Name
        {
            get { return this._data.Name; }
            set { this._data.Name = value; this._parent.Touch(); }
        }
        public string Character
        {
            get { return this._data.Character; }
            set { this._data.Character = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsAccount(Options parent, OptionsV1Account account)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (account == null) throw new ArgumentNullException("account");
            this._parent = parent;
            this._data = account;
        }

        private Options _parent;
        private OptionsV1Account _data;
        #endregion
    }

    public class OptionsWindow
    {
        public string Name
        {
            get { return this._data.Name; }
            set { this._data.Name = value; this._parent.Touch(); }
        }
        public int X
        {
            get { return this._data.X; }
            set { this._data.X = value; this._parent.Touch(); }
        }
        public int Y
        {
            get { return this._data.Y; }
            set { this._data.Y = value; this._parent.Touch(); }
        }
        public int Width
        {
            get { return this._data.Width; }
            set { this._data.Width = value; this._parent.Touch(); }
        }
        public int Height
        {
            get { return this._data.Height; }
            set { this._data.Height = value; this._parent.Touch(); }
        }
        public bool Maximized
        {
            get { return this._data.Maximized; }
            set { this._data.Maximized = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsWindow(Options parent, OptionsV1Window window)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (window == null) throw new ArgumentNullException("window");
            this._parent = parent;
            this._data = window;
        }

        private Options _parent;
        private OptionsV1Window _data;
        #endregion
    }

    public class OptionsSize
    {
        public string Name
        {
            get { return this._data.Name; }
            set { this._data.Name = value; this._parent.Touch(); }
        }
        public string Element
        {
            get { return this._data.Element; }
            set { this._data.Element = value; this._parent.Touch(); }
        }
        public int Size
        {
            get { return this._data.Size; }
            set { this._data.Size = value; this._parent.Touch(); }
        }

        #region Internal
        internal OptionsSize(Options parent, OptionsV1Size size)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (size == null) throw new ArgumentNullException("size");
            this._parent = parent;
            this._data = size;
        }

        private Options _parent;
        private OptionsV1Size _data;
        #endregion
    }
}