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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Vha.Common;
using Vha.Net;
using Vha.Net.Events;

namespace BasicBot
{
    internal static class Program
    {
        private static string _owner;
        public static string Owner { get { return _owner; } }

        // Application entry
        static void Main(string[] args)
        {
            // Read configuration
            Configuration configuration = Configuration.Read("Config.xml");
            if (configuration == null)
            {
                Console.WriteLine("Unable to read configuration");
                Console.ReadLine();
                return;
            }
            _owner = configuration.Owner;
            // Output debug from Vha.Net
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            // Initialize chat connection
            Chat chat = new Chat(configuration.Server, configuration.Port, configuration.Username, configuration.Password, configuration.Character);
            chat.PrivateMessageEvent += new PrivateMessageEventHandler(OnPrivateMessageEvent);
            chat.ChannelMessageEvent += new ChannelMessageEventHandler(OnChannelMessageEvent);
            chat.Connect();
            while (true) Console.ReadLine();
        }

        // Handle messages from organizationchat
        static void OnChannelMessageEvent(Chat chat, ChannelMessageEventArgs e)
        {
            if (chat.ID == e.CharacterID || e.Type != ChannelType.Organization) return;
            CommandArgs args = new CommandArgs(chat, false, e.CharacterID, e.Character, e.Message);
            Commands.OnCommand(chat, args);
        }

        // Handle private messages
        static void OnPrivateMessageEvent(Chat chat, PrivateMessageEventArgs e)
        {
            if (chat.ID == e.CharacterID || e.Outgoing) return;
            CommandArgs args = new CommandArgs(chat, true, e.CharacterID, e.Character, e.Message);
            Commands.OnCommand(chat, args);
        }
    }
}
