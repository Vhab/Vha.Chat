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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Vha.Chat.Commands;
using Vha.Common;

namespace Vha.Chat.UI.Commands
{
    public class OpenCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            if (!context.Input.CheckArguments(trigger, args.Length, 2, true)) return false;
            // Determine target
            MessageTarget target = null;
            switch (args[0].ToLower())
            {
                case "character":
                    if (!context.Input.CheckCharacter(args[1], true)) return false;
                    if (args[1].ToLower() == context.Character.ToLower())
                    {
                        context.Write(MessageClass.Error, "You cannot open a popup window to talk to yourself");
                        return false;
                    }
                    target = new MessageTarget(MessageType.Character, Format.UppercaseFirst(args[1]));
                    break;
                case "channel":
                    string channel = string.Join(" ", args, 1, args.Length - 1);
                    if (!context.Input.CheckChannel(channel, true)) return false;
                    target = new MessageTarget(MessageType.Channel, context.GetChannel(channel).Name);
                    break;
                case "privatechannel":
                    if (!context.Input.CheckPrivateChannel(args[1], true)) return false;
                    target = new MessageTarget(MessageType.PrivateChannel, Format.UppercaseFirst(args[1]));
                    break;
                default:
                    context.Write(MessageClass.Error, "Expecting either 'character', 'channel' or 'privatechannel' as first argument for this command");
                    return false;
            }
            // Show popup window
            lock (this._forms)
            {
                // Check if a window already exists
                if (this._forms.ContainsKey(target))
                {
                    this._forms[target].Focus();
                    return true;
                }
                // Create new window
                Form form = new ChatPopupForm(context, target);
                form.FormClosed += new FormClosedEventHandler(_formClosed);
                this._forms.Add(target, form);
                Utils.InvokeShow(Program.ApplicationContext.MainForm, form);
            }
            return true;
        }

        public OpenCommand(Form form)
            : base(
                "Open window", // Name
                new string[] { "open" }, // Triggers
                new string[] { "open [type] [target]" }, // Usage
                new string[] {
                    "open character Vhab",
                    "open channel Clan OOC",
                    "open privatechannel Helpbot"}, // Examples
                // Description
                "The open command allows you to open a popup window to chat to a specific channel or character.\n" +
                "This command can also be activated by holding shift while click on a channel or character link."
            )
        {
            this._form = form;
        }

        private void _formClosed(object sender, FormClosedEventArgs e)
        {
            Form form = (Form)sender;
            lock (this._forms)
            {
                foreach (KeyValuePair<MessageTarget, Form> kvp in this._forms)
                {
                    if (kvp.Value != form) continue;
                    this._forms.Remove(kvp.Key);
                    break;
                }
            }
        }

        private Form _form = null;
        private Dictionary<MessageTarget, Form> _forms = new Dictionary<MessageTarget,Form>(); 
    }
}
