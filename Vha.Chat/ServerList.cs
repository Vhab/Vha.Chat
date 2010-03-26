/*
* Vha.Common
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Vha.Chat
{
    public class ServerList
    {
        #region members
        public Entries _entries = new Entries();
        public List<Server> Servers
        {
            get
            {
                lock (this._entries.Servers)
                    return this._entries.Servers;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Creates a new server list based on input xml
        /// </summary>
        /// <param name="path">Path to xml file containing server list</param>
        public ServerList(string path)
        {
            this._entries = Common.XML<Entries>.FromFile(path);
            if (this._entries == null)
            {
                this._entries = new Entries();
                this._entries.Servers = new List<Server>(4);
            }
        }

        /// <summary>
        /// Adds a server entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Add(string name, string address, int port)
        {
            Server newentry = new Server(name, address, port);
            lock (this._entries)
                if (!this.Contains(newentry))
                    this._entries.Servers.Add(newentry);
                else
                    return false;
            return true;
        }

        /// <summary>
        /// Removes a server entry
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool Remove(string Name)
        {
            lock (this._entries)
            {
                foreach (Server s in this._entries.Servers)
                {
                    if (s.Name == Name)
                    {
                        this._entries.Servers.Remove(s);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Removes a server entry
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public bool Remove(string Address, int Port)
        {
            lock (this._entries)
            {
                foreach (Server s in this._entries.Servers)
                {
                    if ((s.Address == Address && s.Port == Port))
                    {
                        this._entries.Servers.Remove(s);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Retrieves a server entry
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Server Get(string Name)
        {
            lock (this._entries)
            {
                foreach (Server s in this._entries.Servers)
                {
                    if (s.Name == Name)
                    {
                        return s;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Retrieves a server entry
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public Server Get(string Address, int Port)
        {
            lock (this._entries)
            {
                foreach (Server s in this._entries.Servers)
                {
                    if ((s.Address == Address && s.Port == Port))
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        public bool Contains(Server server)
        {
            lock (this._entries)
            {
                foreach (Server s in this._entries.Servers)
                {
                    if ((s.Address == server.Address && s.Port == server.Port)
                        || s.Name == server.Name)
                        return true;
                }
            }
            return false;
        }
        #endregion

        #region classes

        [XmlRoot("Servers")]
        public class Entries
        {
            [XmlElement("Server")]
            public List<Server> Servers;
        }
        #endregion
    }
}