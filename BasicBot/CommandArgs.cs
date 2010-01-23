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
using System.Text;
using Vha.Common;
using Vha.Net;

namespace BasicBot
{
    public class CommandArgs
    {
        private Chat _chat;
        public readonly bool Tell;
        public readonly uint CharacterID;
        public readonly string Character;
        public readonly string Message;
        public readonly string Command;
        public readonly string[] Args;

        public CommandArgs(Chat chat, bool tell, uint characterID, string character, string message)
        {
            this._chat = chat;
            this.Tell = tell;
            this.CharacterID = characterID;
            this.Character = character;
            this.Message = message;
            this.Command = message.Trim();
            int index = this.Command.IndexOf(' ');
            if (index > 0)
            {
                string args = this.Command.Substring(index).Trim();
                this.Command = this.Command.Substring(0, index);
                Args = args.Split(' ');
            }
            else
            {
                Args = new string[0];
            }
            this.Command = this.Command.ToLower();
        }

        public void Reply(string message)
        {
            if (this.Tell)
            {
                _chat.SendPrivateMessage(this.CharacterID, message);
            }
            else
            {
                _chat.SendChannelMessage(_chat.OrganizationID, message);
            }
        }
    }
}
