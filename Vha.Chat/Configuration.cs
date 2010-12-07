/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using System.IO;
using System.Security;
using System.Security.Permissions;
using Vha.Chat.Data;

namespace Vha.Chat
{
    /// <summary>
    /// Read-only instance of Data.ConfigurationV1
    /// </summary>
    public class Configuration
    {
        public readonly bool ReadOnly;
        public readonly string OptionsPath;
        public readonly string OptionsFile;
        public readonly string IgnoresFile;
        public readonly Dimension[] Dimensions;

        public Configuration(Base data)
        {
            // A little bit of sanity
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Type != typeof(ConfigurationV1))
                throw new ArgumentException("Invalid config data type: " + data.Type.ToString());
            // Store values
            ConfigurationV1 config = (ConfigurationV1)data;
            this.OptionsFile = config.OptionsFile;
            this.IgnoresFile = config.IgnoresFile;
            this.Dimensions = config.Dimensions.ConvertAll<Dimension>(
                new Converter<ConfigurationV1Dimension,Dimension>(this._convertDimension))
                .ToArray();
            // Test options path before storing
            try
            {
                string path = Path.GetFullPath(config.OptionsPath);
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, path);
                permission.Demand();
                // Permission granted
                this.OptionsPath = path;
                this.ReadOnly = false;
                return;
            }
            catch (SecurityException)
            {
                // Fail if path is absolute
                if (Path.IsPathRooted(config.OptionsPath))
                {
                    // Path is absolute, fail
                    this.OptionsPath = "";
                    this.ReadOnly = true;
                    return;
                }
            }
            // Try AppData path
            try
            {
                string path = string.Format("{1}{0}{2}{0}{3}",
                    Path.DirectorySeparatorChar,
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Vha.Chat",
                    config.OptionsPath);
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, path);
                permission.Demand();
                // Permission granted
                this.OptionsPath = path;
                this.ReadOnly = false;
            }
            catch (SecurityException)
            {
                this.OptionsPath = "";
                this.ReadOnly = true;
            }
        }

        public Dimension GetDimension(string name)
        {
            foreach (Dimension dim in this.Dimensions)
                if (dim.Name == name)
                    return dim;
            return null;
        }

        private Dimension _convertDimension(ConfigurationV1Dimension dimension)
        {
            return new Dimension(dimension.Name, dimension.Address, dimension.Port);
        }
    }
}
