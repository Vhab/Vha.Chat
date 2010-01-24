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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Vha.Net;
using Vha.Net.Events;
using Vha.Common;

namespace Vha.Chat
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
            this._chat.FriendStatusEvent += new FriendStatusEventHandler(_chat_FriendStatusEvent);
            this._chat.FriendRemovedEvent += new FriendRemovedEventHandler(_chat_FriendRemovedEvent);
            this._chat.ChannelJoinEvent += new ChannelJoinEventHandler(_chat_ChannelJoinEvent);
            this._chat.PrivateChannelStatusEvent += new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent);
            this._chat.PrivateChannelRequestEvent += new PrivateChannelRequestEventHandler(_chat_PrivateChannelRequestEvent);
            this._chat.StatusChangeEvent += new StatusChangeEventHandler(_chat_StatusChangeEvent);

            this._tree.Nodes.Add(this._online);
            this._tree.Nodes.Add(this._offline);
            this._tree.Nodes.Add(this._channels);
            this._tree.Nodes.Add(this._privateChannels);

            this._input = new ChatInput(this, this._chat);
            this._output = new ChatOutput(this, this._chat);
            this._links = new ChatHtml(this, this._input, this._chat);

            // Disable unsupported buttons
            this._options.Visible = false;
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
            if (this._outputBox.Document == null || this._outputBox.Document.Body == null)
            {
                this._lines.Enqueue(html);
                return;
            }
            this._links.AppendHtml(this._outputBox.Document, html, true);
            // Clean up old messages
            while (this._outputBox.Document.Body.Children.Count > Program.MaximumMessages)
            {
                this._outputBox.Document.Body.FirstChild.OuterHtml = "";
            }
            // Scroll to bottom
            this._outputBox.Document.InvokeScript("scrollToBottom");
        }

        public delegate void SetTargetDelegate(ChatInputType type, string target);
        public void SetTarget(ChatInputType type, string target)
        {
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new SetTargetDelegate(SetTarget), new object[] { type, target });
                return;
            }
            // Focus input
            this._inputBox.Focus();
            this._inputBox.Select(this._inputBox.Text.Length, 0);
            // Create target
            ChatTarget chatTarget = new ChatTarget(type, target);
            // Check if the target already exists in our list
            foreach (object t in this._target.Items)
            {
                ChatTarget ct = (ChatTarget)t;
                if (!ct.Equals(chatTarget)) continue;
                this._target.SelectedItem = t;
                return;
            }
            // Add new target
            int index = this._target.Items.Add(chatTarget);
            this._target.SelectedIndex = index;
        }

        private void _chat_ChannelJoinEvent(Vha.Net.Chat chat, ChannelJoinEventArgs e)
        {
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new ChannelJoinEventHandler(_chat_ChannelJoinEvent), new object[] { chat, e });
                return;
            }
            TreeNode node = new TreeNode(e.Name.Trim('~'));
            node.ForeColor = Color.DarkBlue;
            this._channels.Nodes.Add(node);
            if (this._channels.Nodes.Count == 1)
                this._channels.Expand();
        }

        private void _chat_FriendStatusEvent(Vha.Net.Chat chat, FriendStatusEventArgs e)
        {
            // Ignore temporary buddies
            if (e.Temporary) return;
            // Use invoke if needed
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new FriendStatusEventHandler(_chat_FriendStatusEvent), new object[] { chat, e });
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

        private void _chat_FriendRemovedEvent(Vha.Net.Chat chat, CharacterIDEventArgs e)
        {
            // Use invoke if needed
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new FriendRemovedEventHandler(_chat_FriendRemovedEvent), new object[] { chat, e });
                return;
            }
            // Remove friend
            if (this._online.ContainsText(e.Character))
                this._online.RemoveText(e.Character);
            if (this._offline.ContainsText(e.Character))
                this._offline.RemoveText(e.Character);
        }

        private void _chat_PrivateChannelStatusEvent(Vha.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            // Use invoke if needed
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent), new object[] { chat, e });
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

        void _chat_PrivateChannelRequestEvent(Vha.Net.Chat chat, PrivateChannelRequestEventArgs e)
        {
            // Invoke on form thread
            if (this.InvokeRequired)
            {
                this.Invoke(new PrivateChannelRequestEventHandler(_chat_PrivateChannelRequestEvent), new object[] { chat, e });
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

        private void _chat_StatusChangeEvent(Vha.Net.Chat chat, StatusChangeEventArgs e)
        {
            // Update buttons when disconnect
            if (e.State == ChatState.Disconnected)
            {
                this._connect.Enabled = true;
                this._disconnect.Enabled = false;
            }
            // Only really care about being connected for the rest of the code
            if (e.State != ChatState.Connected) return;
            // Invoke
            if (this.InvokeRequired)
            {
                this.Invoke(new StatusChangeEventHandler(_chat_StatusChangeEvent), new object[] { chat, e });
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
            // Update buttons
            this._connect.Enabled = false;
            this._disconnect.Enabled = true;
        }

        private void _inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r') return;
            e.Handled = true;
            if (this._inputBox.Text.Trim().Length == 0)
                return;
            if (this._inputBox.Text.StartsWith("/"))
            {
                this._input.Command(this._inputBox.Text);
                this._inputBox.Text = "";
                return;
            }
            if (this._target.SelectedItem == null)
            {
                AppendLine("Error", "No channel selected");
                return;
            }
            ChatTarget target = (ChatTarget)this._target.SelectedItem;
            this._input.Send(target.Type, target.Target, this._inputBox.Text);
            this._inputBox.Text = ""; ;
        }

        private void _outputBox_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // We can navigate to local files
            if (e.Url.Scheme == "file") return;
            // Cancel all other actions
            e.Cancel = true;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._chat == null) return;
            if (this._chat.State != ChatState.Disconnected)
            {
                this._chat.Disconnect();
            }
            this._chat = null;
        }

        private void _outputBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this._outputBox.Document.Write(this._links.Template);
            this._outputBox.Document.BackColor = this.BackColor;
            if (this._outputBox.Document.Body == null) return;
            string color = this.ForeColor.R.ToString("X") + this.ForeColor.G.ToString("X") + this.ForeColor.B.ToString("X");
            this._outputBox.Document.Body.Style = "color: #" + color + ";";
            // Welcome message
            this.AppendLine("Internal", "The following commands are available:");
            this.AppendLine("Internal", "- /tell <i>[username] [message]</i>");
            this.AppendLine("Internal", "- /leave <i>[channel]</i>");
            this.AppendLine("Internal", "- /invite <i>[username]</i>");
            this.AppendLine("Internal", "- /kick <i>[username]</i>");
            this.AppendLine("Internal", "- /kickall");
            this.AppendLine("Internal", "- /addbuddy <i>[username]</i>");
            this.AppendLine("Internal", "- /rembuddy <i>[username]</i>");
            this.AppendLine("Internal", "- /o <i>[message]</i>");
            this.AppendLine("Internal", "- /about");
            // Clear queue
            while (this._lines.Count > 0)
            {
                string html = this._lines.Dequeue();
                this.AppendLine(html);
            }
        }

        private void _tree_DoubleClick(object sender, EventArgs e)
        {
            // Check if something sensible is selected
            if (this._tree.SelectedNode == null) return;
            if (this._tree.SelectedNode.Parent == null) return;
            // Get the data
            ChatTreeNode branch = (ChatTreeNode)this._tree.SelectedNode.Parent;
            string target = this._tree.SelectedNode.Text;
            // Set the target
            this.SetTarget(branch.Type, target);
        }

        private void _about_Click(object sender, EventArgs e)
        {
            this._input.Command("about");
        }

        private void _connect_Click(object sender, EventArgs e)
        {
            // Return to authorization window
            Program.Context.MainForm = new AuthenticationForm();
            // Close this window before showing auth window
            this.Close();
            Program.Context.MainForm.Show();
        }

        private void _disconnect_Click(object sender, EventArgs e)
        {
            this._chat.Disconnect();
            // Disable form
            this._disconnect.Enabled = false;
            this._connect.Enabled = true;
        }

        private void _options_Click(object sender, EventArgs e)
        {
            this.AppendLine("Internal", "Coming soon: Options™");
        }
    }
}