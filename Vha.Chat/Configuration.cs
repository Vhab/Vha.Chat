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
using System.Xml.Serialization;
using Vha.Chat.Data;

namespace Vha.Chat
{
    /// <summary>
    /// Read-only instance of Data.ConfigurationV1
    /// </summary>
    public class Configuration
    {
        public readonly string OptionsPath;
        public readonly string OptionsFile;
        public readonly string IgnoresPath;
        public readonly Dimension[] Dimensions;

        public Configuration(Base data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Type != typeof(ConfigurationV1))
                throw new ArgumentException("Invalid config data type: " + data.Type.ToString());
            ConfigurationV1 config = (ConfigurationV1)data;
            this.OptionsPath = config.OptionsPath;
            this.OptionsFile = config.OptionsFile;
            this.IgnoresPath = config.IgnoresPath;
            this.Dimensions = config.Dimensions.ConvertAll<Dimension>(
                new Converter<ConfigurationV1Dimension,Dimension>(this._convertDimension))
                .ToArray();
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
