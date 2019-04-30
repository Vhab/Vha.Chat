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
using System.IO;
using System.Security;
using System.Security.Permissions;
using Vha.Chat.Data;
using Vha.Chat.UI.Controls;

namespace Vha.Chat
{
    /// <summary>
    /// Read-only instance of Data.ConfigurationV1
    /// </summary>
    public class Configuration
    {
        public bool ReadOnly { get { return this._readOnly; } }
        public string OptionsPath { get { return this._optionsPath; } }
        public string OptionsFile { get { return this._optionsFile; } }
        public string OptionsFilePath { get { return this._optionsPath + Path.DirectorySeparatorChar + this._optionsFile; } }
        public string IgnoresFile { get { return this._ignoresFile; } }
        public string IgnoresFilePath { get { return this._optionsPath + Path.DirectorySeparatorChar + this._ignoresFile; } }
        public OutputControlInitializationMode OutputMode { get { return this._outputMode; } }
        public Dimension[] Dimensions { get { return this._dimensions; } }

        public Configuration(ConfigurationV1 config)
        {
            if (config.OptionsPath == null)
            {
                this._optionsPath = "";
                this._readOnly = true;
            }
            else
            {
                this._optionsPath = config.OptionsPath;
                this._readOnly = false;
            }
            this._optionsFile = config.OptionsFile;
            this._ignoresFile = config.IgnoresFile;
            this._outputMode = config.OutputMode;
            this._dimensions = config.Dimensions.ConvertAll<Dimension>(
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

        private bool _readOnly;
        private string _optionsPath;
        private string _optionsFile;
        private string _ignoresFile;
        private OutputControlInitializationMode _outputMode;
        private Dimension[] _dimensions;

        private Dimension _convertDimension(ConfigurationV1Dimension dimension)
        {
            return new Dimension(dimension.Name, dimension.Address, dimension.Port);
        }
    }
}
