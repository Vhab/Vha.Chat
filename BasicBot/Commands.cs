/*
* BasicBot - An VhaBot.Net example
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
using System.Text;
using VhaBot.Common;
using VhaBot.Net;

namespace BasicBot
{
    public static class Commands
    {
        // Handles all commands
        public static void OnCommand(Chat chat, CommandArgs e)
        {
            switch (e.Command)
            {
                case "!random":
                    OnRandom(chat, e);
                    break;
                case "!cybor":
                    OnCybor(chat, e);
                    break;
                case "!quit":
                    OnQuit(chat, e);
                    break;
                default:
                    if (!e.Tell) return;
                    e.Reply("Unknown command");
                    break;
            }
        }

        // Handle random command
        public static void OnRandom(Chat chat, CommandArgs e)
        {
            if (e.Args.Length < 2)
            {
                e.Reply("Correct usage: !random [minimum] [maximum]");
                return;
            }
            int min = 0;
            int max = 0;
            if (!int.TryParse(e.Args[0], out min) || !int.TryParse(e.Args[1], out max))
            {
                e.Reply("Invalid values");
            }
            Random rand = new Random();
            int value = rand.Next(max - min + 1) + min;
            e.Reply("From " + min + " to " + max + ", I rolled " + value);
        }

        // Handle cybor command
        public static void OnCybor(Chat chat, CommandArgs e)
        {
            if (!e.Tell)
            {
                e.Reply("I don't want everyone to see my 1's and 0's");
                return;
            }
            e.Reply("You can see me naked at http://svn.vhabion.net/VhaBot/");
        }

        // Handle quit command
        public static void OnQuit(Chat chat, CommandArgs e)
        {
            if (e.Character != Program.Owner)
            {
                e.Reply("Sorry, this command is only available for the owner");
                return;
            }
            e.Reply("Shutting down...");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
