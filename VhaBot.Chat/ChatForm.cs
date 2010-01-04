/*
* VhaBot.Chat
* Copyright (C) 2009 Remco van Oosterhout
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VhaBot.Net;
using VhaBot.Net.Events;
using VhaBot.Common;

namespace VhaBot.Chat
{
    public partial class ChatForm : Form
    {
        protected ChatTreeNode _online = new ChatTreeNode(ChatInputType.Character, "Online");
        protected ChatTreeNode _offline = new ChatTreeNode(ChatInputType.Character, "Offline");
        protected ChatTreeNode _channels = new ChatTreeNode(ChatInputType.Channel, "Channels");
        protected ChatTreeNode _privateChannels = new ChatTreeNode(ChatInputType.PrivateChannel, "Private Channels");

        protected ChatInput _input;
        protected ChatOutput _output;
        protected ChatHtml _links;
        protected Net.Chat _chat;

        protected Queue<string> _lines = new Queue<string>();

        public ChatForm(Net.Chat chat)
        {
            InitializeComponent();

            this._chat = chat;
            this._chat.FriendStatusEvent += new FriendStatusEventHandler(Chat_FriendStatusEvent);
            this._chat.FriendRemovedEvent += new FriendRemovedEventHandler(Chat_FriendRemovedEvent);
            this._chat.ChannelJoinEvent += new ChannelJoinEventHandler(Chat_ChannelJoinEvent);
            this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(Chat_PrivateChannelStatusEvent);
            this._chat.PrivateChannelRequestEvent += new PrivateChannelRequestEventHandler(Chat_PrivateChannelRequestEvent);
            this._chat.StatusChangeEvent += new StatusChangeEventHandler(Chat_StatusChangeEvent);

            this.Tree.Nodes.Add(this._online);
            this.Tree.Nodes.Add(this._offline);
            this.Tree.Nodes.Add(this._channels);
            this.Tree.Nodes.Add(this._privateChannels);

            this._input = new ChatInput(this, this._chat);
            this._output = new ChatOutput(this, this._chat);
            this._links = new ChatHtml(this, this._input, this._chat);

            this.Output.DocumentText = this._links.Template;
        }

        public void AppendLine(string type, string line)
        {
            string html = string.Format(
                "<div class=\"Line\"><span class=\"Time\">[{0:00}:{1:00}:{2:00}]</span> <span class=\"{3}\">{4}</span></div>",
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, type, line);
            AppendLine(html);
        }
        public delegate void AppendLineDelegate(string html);
        private void AppendLine(string html)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AppendLineDelegate(AppendLine), new object[] { html });
                return;
            }
            // Queue messages if the browser isn't ready yet
            if (this.Output.Document == null || this.Output.Document.Body == null)
            {
                this._lines.Enqueue(html);
                return;
            }
            this._links.AppendHtml(this.Output.Document, html, true);
            // Clean up old messages
            while (this.Output.Document.Body.Children.Count > Program.MaximumMessages)
            {
                this.Output.Document.Body.FirstChild.OuterHtml = "";
            }
            // Scroll to bottom
            this.Output.Document.InvokeScript("scrollToBottom");
        }

        public delegate void SetTargetDelegate(ChatInputType type, string target);
        public void SetTarget(ChatInputType type, string target)
        {
            if (this.Channels.InvokeRequired)
            {
                this.Channels.BeginInvoke(new SetTargetDelegate(SetTarget), new object[] { type, target });
                return;
            }
            // Focus input
            this.Input.Focus();
            this.Input.Select(this.Input.Text.Length, 0);
            // Create target
            ChatTarget chatTarget = new ChatTarget(type, target);
            // Check if the target already exists in our list
            foreach (object t in this.Channels.Items)
            {
                ChatTarget ct = (ChatTarget)t;
                if (!ct.Equals(chatTarget)) continue;
                this.Channels.SelectedItem = t;
                return;
            }
            // Add new target
            int index = this.Channels.Items.Add(chatTarget);
            this.Channels.SelectedIndex = index;
        }

        private void Chat_ChannelJoinEvent(VhaBot.Net.Chat chat, ChannelJoinEventArgs e)
        {
            if (this.Channels.InvokeRequired)
            {
                this.Channels.BeginInvoke(new ChannelJoinEventHandler(Chat_ChannelJoinEvent), new object[] { chat, e });
                return;
            }
            TreeNode node = new TreeNode(e.Name.Trim('~'));
            node.ForeColor = Color.DarkBlue;
            this._channels.Nodes.Add(node);
            if (this._channels.Nodes.Count == 1)
                this._channels.Expand();
        }

        private void Chat_FriendStatusEvent(VhaBot.Net.Chat chat, FriendStatusEventArgs e)
        {
            // Ignore temporary buddies
            if (e.Temporary) return;
            // Use invoke if needed
            if (this.Channels.InvokeRequired)
            {
                this.Channels.BeginInvoke(new FriendStatusEventHandler(Chat_FriendStatusEvent), new object[] { chat, e });
                return;
            }
            // Add buddy to list
            TreeNode node = new TreeNode(e.Character);
            if (e.Online)
            {
                if (this._online.ContainsText(e.Character)) return;
                if (this._offline.ContainsText(e.Character))
                    this._offline.RemoveText(e.Character);
                node.ForeColor = Color.DarkGreen;
                this._online.Nodes.Add(node);
                if (this._online.Nodes.Count == 1)
                    this._online.Expand();
            }
            else
            {
                if (this._offline.ContainsText(e.Character)) return;
                if (this._online.ContainsText(e.Character))
                    this._online.RemoveText(e.Character);
                node.ForeColor = Color.DarkRed;
                this._offline.Nodes.Add(node);
            }
        }

        private void Chat_FriendRemovedEvent(VhaBot.Net.Chat chat, CharacterIDEventArgs e)
        {
            // Use invoke if needed
            if (this.Channels.InvokeRequired)
            {
                this.Channels.BeginInvoke(new FriendRemovedEventHandler(Chat_FriendRemovedEvent), new object[] { chat, e });
                return;
            }
            // Remove friend
            if (this._online.ContainsText(e.Character))
                this._online.RemoveText(e.Character);
            if (this._offline.ContainsText(e.Character))
                this._offline.RemoveText(e.Character);
        }

        private void Chat_PrivateChannelStatusEvent(VhaBot.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            // Use invoke if needed
            if (this.Channels.InvokeRequired)
            {
                this.Channels.BeginInvoke(new PrivateChannelStatusEventHandler(Chat_PrivateChannelStatusEvent), new object[] { chat, e });
                return;
            }
            // Ignore our own channel
            if (e.ChannelID == chat.ID) return;
            // Ignore other characters
            if (e.CharacterID != chat.ID) return;
            // Update list
            if (e.Join)
            {
                if (this._privateChannels.ContainsText(e.Channel)) return;
                TreeNode node = new TreeNode(e.Channel);
                node.ForeColor = Color.DimGray;
                this._privateChannels.Nodes.Add(node);
            }
            else
            {
                if (!this._privateChannels.ContainsText(e.Channel)) return;
                this._privateChannels.RemoveText(e.Channel);
            }
        }

        void Chat_PrivateChannelRequestEvent(VhaBot.Net.Chat chat, PrivateChannelRequestEventArgs e)
        {
            // Invoke on form thread
            if (this.InvokeRequired)
            {
                this.Invoke(new PrivateChannelRequestEventHandler(Chat_PrivateChannelRequestEvent), new object[] { chat, e });
                return;
            }
            // Show dialog
            DialogResult result = MessageBox.Show(
                "You have been invited to " + e.Character + "'s private channel. Do you wish to join?",
                "Private Channel Invite",
                MessageBoxButtons.YesNo);
            // Join channel if accepted
            if (result == DialogResult.Yes)
            {
                e.Join = true;
            }
        }

        private void Chat_StatusChangeEvent(VhaBot.Net.Chat chat, StatusChangeEventArgs e)
        {
            // Only really care about being connected
            if (e.State != ChatState.Connected) return;
            // Invoke
            if (this.InvokeRequired)
            {
                this.Invoke(new StatusChangeEventHandler(Chat_StatusChangeEvent), new object[] { chat, e });
                return;
            }
            // - Add ourselves to private channel
            TreeNode node = new TreeNode(this._chat.Character);
            node.ForeColor = Color.DimGray;
            this._privateChannels.Nodes.Clear();
            this._privateChannels.Nodes.Add(node);
            this._privateChannels.Expand();
            // - Clear other tree sections
            this._channels.Nodes.Clear();
            this._online.Nodes.Clear();
            this._offline.Nodes.Clear();
        }

        private void Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            e.Handled = true;
            if (this.Input.Text.Trim().Length == 0)
                return;
            if (this.Input.Text.StartsWith("/"))
            {
                this._input.Command(this.Input.Text);
                this.Input.Text = "";
                return;
            }
            if (this.Channels.SelectedItem == null)
            {
                AppendLine("Error", "No channel selected");
                return;
            }
            ChatTarget target = (ChatTarget)this.Channels.SelectedItem;
            this._input.Send(target.Type, target.Target, this.Input.Text);
            this.Input.Text = ""; ;
        }

        private void Output_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // We can navigate to local files
            if (e.Url.Scheme == "file") return;
            // Cancel all other actions
            e.Cancel = true;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._chat.Disconnect();
            this._chat = null;
            // Return to authorization window
            Program.Context.MainForm = new AuthenticationForm();
            Program.Context.MainForm.Show();
        }

        private void Output_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Output.Document.BackColor = this.BackColor;
            if (this.Output.Document.Body == null) return;
            string color = this.ForeColor.R.ToString("X") + this.ForeColor.G.ToString("X") + this.ForeColor.B.ToString("X");
            this.Output.Document.Body.Style = "color: #" + color + ";";
            // Welcome message
            this.AppendLine("Internal", "Thank you for using <a href='chatcmd:///about' class='Link'>VhaBot.Chat</a>");
            this.AppendLine("Internal", "The following commands are available:");
            this.AppendLine("Internal", "- /tell [username] [message]");
            this.AppendLine("Internal", "- /leave [channel]");
            this.AppendLine("Internal", "- /invite [username]");
            this.AppendLine("Internal", "- /kick [username]");
            this.AppendLine("Internal", "- /kickall");
            this.AppendLine("Internal", "- /addbuddy [username]");
            this.AppendLine("Internal", "- /rembuddy [username]");
            this.AppendLine("Internal", "- /about");
            // Clear queue
            while (this._lines.Count > 0)
            {
                string html = this._lines.Dequeue();
                this.AppendLine(html);
            }
        }

        private void Tree_DoubleClick(object sender, EventArgs e)
        {
            // Check if something sensible is selected
            if (this.Tree.SelectedNode == null) return;
            if (this.Tree.SelectedNode.Parent == null) return;
            // Get the data
            ChatTreeNode branch = (ChatTreeNode)this.Tree.SelectedNode.Parent;
            string target = this.Tree.SelectedNode.Text;
            // Set the target
            this.SetTarget(branch.Type, target);
        }
    }
}