/*
* BasicBot - An Vha.Net example
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
using System.IO;

namespace BasicBot
{
    [XmlRoot("Configuration"), Serializable]
    public class Configuration
    {
        public string Server;
        public int Port;
        public string Username;
        public string Password;
        public string Character;
        public string Owner;

        public static Configuration Read(string file)
        {
            Configuration configuration = null;
            FileStream stream = null;
            try
            {
                stream = File.Open(file, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                configuration = (Configuration)serializer.Deserialize(stream);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return configuration;
        }
    }
}
