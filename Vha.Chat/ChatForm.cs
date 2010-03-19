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
        protected ChatTreeNode _guests = new ChatTreeNode(ChatInputType.Character, "Guests");

        protected ChatInput _inputUtil;
        protected ChatOutput _outputUtil;
        protected ChatHtml _htmlUtil;
        protected Net.Chat _chat;

        protected List<string> _history = new List<string>();
        protected int _historyIndex = 0;

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
            this._tree.Nodes.Add(this._guests);

            this._inputUtil = new ChatInput(this, this._chat);
            this._htmlUtil = new ChatHtml(this, this._inputUtil);
            this._outputUtil = new ChatOutput(this, this._htmlUtil, this._chat);

            // Disable options button
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
            this._htmlUtil.AppendHtml(this._outputBox.Document, Program.Configuration.TextStyle, html, true);
            // Clean up old messages
            while (this._outputBox.Document.Body.Children.Count > Program.Configuration.MaximumMessages)
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
            TreeNode node = this._channels.GetNode(e.Name);
            if (node != null)
            {
                if (e.Muted) node.ImageKey = node.SelectedImageKey = "ChannelDisabled";
                else node.ImageKey = node.SelectedImageKey = "Channel";
            }
            else
            {
                if (e.Muted) this._channels.AddNode(e.Name, "ChannelDisabled");
                else this._channels.AddNode(e.Name, "Channel");
            }
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
            if (e.Online)
            {
                if (this._online.ContainsNode(e.Character)) return;
                if (this._offline.ContainsNode(e.Character))
                    this._offline.RemoveNode(e.Character);
                this._online.AddNode(e.Character, "CharacterOnline");
                if (this._online.Nodes.Count == 1)
                    this._online.Expand();
            }
            else
            {
                if (this._offline.ContainsNode(e.Character)) return;
                if (this._online.ContainsNode(e.Character))
                    this._online.RemoveNode(e.Character);
                this._offline.AddNode(e.Character, "CharacterOffline");
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
            if (this._online.ContainsNode(e.Character))
                this._online.RemoveNode(e.Character);
            if (this._offline.ContainsNode(e.Character))
                this._offline.RemoveNode(e.Character);
        }

        private void _chat_PrivateChannelStatusEvent(Vha.Net.Chat chat, PrivateChannelStatusEventArgs e)
        {
            // Use invoke if needed
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new PrivateChannelStatusEventHandler(_chat_PrivateChannelStatusEvent), new object[] { chat, e });
                return;
            }
            // Handle our own channel
            if (e.ChannelID == chat.ID)
            {
                // (not sure this ever happens, but better safe than sorry)
                if (e.CharacterID == chat.ID) return;
                // Update list
                if (e.Join)
                {
                    if (this._guests.ContainsNode(e.Character)) return;
                    this._guests.AddNode(e.Character, "Character");
                    if (this._guests.Nodes.Count == 1)
                        this._guests.Expand();
                }
                else
                {
                    if (!this._guests.ContainsNode(e.Character)) return;
                    this._guests.RemoveNode(e.Character);
                }
            }
            // Handle remote private channels
            else
            {
                // Ignore other characters
                if (e.CharacterID != chat.ID) return;
                // Update list
                if (e.Join)
                {
                    if (this._privateChannels.ContainsNode(e.Channel)) return;
                    this._privateChannels.AddNode(e.Channel, "Character");
                }
                else
                {
                    if (!this._privateChannels.ContainsNode(e.Channel)) return;
                    this._privateChannels.RemoveNode(e.Channel);
                }
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
            // Invoke
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new StatusChangeEventHandler(_chat_StatusChangeEvent), new object[] { chat, e });
                return;
            }
            // Update buttons when disconnect
            if (e.State == ChatState.Disconnected)
            {
                this._connect.Enabled = true;
                this._disconnect.Enabled = false;
            }
            // Only really care about being connected for the rest of the code
            else if (e.State == ChatState.Reconnecting)
            {
                // - Clear tree sections
                this._channels.Nodes.Clear();
                this._online.Nodes.Clear();
                this._offline.Nodes.Clear();
                this._guests.Nodes.Clear();
                this._privateChannels.Nodes.Clear();
            }
            else if (e.State == ChatState.Connected)
            {
                // - Add ourselves to private channel
                this._privateChannels.AddNode(this._chat.Character, "Character");
                this._privateChannels.Expand();
                // Update buttons
                this._connect.Enabled = false;
                this._disconnect.Enabled = true;
            }
        }

        private void _inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (this._historyIndex < this._history.Count)
                {
                    this._historyIndex++;
                    this._inputBox.Text = this._history[this._historyIndex - 1];
                    this._inputBox.Select(this._inputBox.Text.Length, 0);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (this._historyIndex > 0)
                {
                    this._historyIndex--;
                    if (this._historyIndex == 0)
                        this._inputBox.Text = "";
                    else
                        this._inputBox.Text = this._history[this._historyIndex - 1];
                    this._inputBox.Select(this._inputBox.Text.Length, 0);
                }
            }
            else
            {
                this._historyIndex = 0;
            }
        }

        private void _inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                // Sensible out
                if (this._inputBox.Text.Trim().Length == 0)
                    return;
                // History
                if (this._history.Count == 0 || this._history[0] != this._inputBox.Text)
                    this._history.Insert(0, this._inputBox.Text);
                while (this._history.Count > Program.Configuration.MaximumHistory)
                    this._history.RemoveAt(this._history.Count - 1);
                this._historyIndex = 0;
                // Handle the input
                if (this._inputBox.Text.StartsWith("/"))
                {
                    this._inputUtil.Command(this._inputBox.Text);
                    this._inputBox.Text = "";
                    return;
                }
                if (this._target.SelectedItem == null)
                {
                    AppendLine("Error", "No channel selected");
                    return;
                }
                ChatTarget target = (ChatTarget)this._target.SelectedItem;
                this._inputUtil.Send(target.Type, target.Target, this._inputBox.Text);
                this._inputBox.Text = "";
            }
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
            this._outputBox.Document.Write(this._htmlUtil.Template);
            this._outputBox.Document.BackColor = this.BackColor;
            if (this._outputBox.Document.Body == null) return;
            string color = this.ForeColor.R.ToString("X") + this.ForeColor.G.ToString("X") + this.ForeColor.B.ToString("X");
            this._outputBox.Document.Body.Style = "color: #" + color + ";";
            // Welcome message
            this.AppendLine("Internal", "The following commands are available:");
            this.AppendLine("Internal", "- /tell [username] [message]");
            this.AppendLine("Internal", "- /leave [private channel]");
            this.AppendLine("Internal", "- /invite [username]");
            this.AppendLine("Internal", "- /kick [username]");
            this.AppendLine("Internal", "- /kickall");
            this.AppendLine("Internal", "- /addbuddy [username]");
            this.AppendLine("Internal", "- /rembuddy [username]");
            this.AppendLine("Internal", "- /o [message]");
            this.AppendLine("Internal", "- /mute [channel]");
            this.AppendLine("Internal", "- /unmute [channel]");
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

        private void _tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Early outs
            if (e.Button != MouseButtons.Right) return;
            if (e.Node == null) return;
            if (e.Node.Parent == null) return;
            // Get the data
            ChatTreeNode branch = (ChatTreeNode)e.Node.Parent;
            string target = e.Node.Text;
            // Set as selected node
            this._tree.SelectedNode = e.Node;
            // Show menu
            if (e.Node.Parent == this._online)
            {
                // Online character menu
                this._characterMenu.Tag = target;
                if (this._guests.ContainsNode(target))
                {
                    this._characterMenu_Invite.Enabled = false;
                }
                else
                {
                    this._characterMenu_Invite.Enabled = true;
                }
                this._characterMenu.Show((Control)sender, e.Location);
            }
            else if (e.Node.Parent == this._offline)
            {
                // Offline character menu
                this._characterMenu.Tag = target;
                this._characterMenu_Invite.Enabled = false;
                this._characterMenu.Show((Control)sender, e.Location);
            }
            else if (e.Node.Parent == this._channels)
            {
                this._channelMenu.Tag = target;
                // Disable irrelevant buttons
                if (e.Node.ImageKey == "ChannelDisabled")
                {
                    this._channelMenu_Mute.Enabled = false;
                    this._channelMenu_Unmute.Enabled = true;
                }
                else
                {
                    this._channelMenu_Mute.Enabled = true;
                    this._channelMenu_Unmute.Enabled = false;
                }
                this._channelMenu.Show((Control)sender, e.Location);
            }
            else if (e.Node.Parent == this._privateChannels)
            {
                this._privateChannelMenu.Tag = target;
                // Can't leave the channel if it's our own channel
                if (target == this._chat.Character)
                {
                    this._privateChannelMenu_Leave.Enabled = false;
                }
                else
                {
                    this._privateChannelMenu_Leave.Enabled = true;
                }
                this._privateChannelMenu.Show((Control)sender, e.Location);
            }
            else if (e.Node.Parent == this._guests)
            {
                // Guest character menu
                this._guestsMenu.Tag = target;
                this._guestsMenu.Show((Control)sender, e.Location);
            }
        }

        private void _about_Click(object sender, EventArgs e)
        {
            this._inputUtil.Command("about");
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
            Form options = new OptionsForm();
            options.ShowDialog();
        }

        private void _channelMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            SetTarget(ChatInputType.Channel, (string)this._channelMenu.Tag);
        }

        private void _channelMenu_Mute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._inputUtil.Command("mute " + (string)this._channelMenu.Tag);
        }

        private void _channelMenu_Unmute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._inputUtil.Command("unmute " + (string)this._channelMenu.Tag);
        }

        private void _privateChannelMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            SetTarget(ChatInputType.PrivateChannel, (string)this._privateChannelMenu.Tag);
        }

        private void _privateChannelMenu_Leave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            this._inputUtil.Command("leave " + (string)this._privateChannelMenu.Tag);
        }

        private void _characterMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            SetTarget(ChatInputType.Character, (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Remove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._inputUtil.Command("rembuddy " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Invite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._inputUtil.Command("invite " + (string)this._characterMenu.Tag);
        }

        private void _guestsMenu_Kick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._guestsMenu.Tag)) return;
            this._inputUtil.Command("kick " + (string)this._guestsMenu.Tag);
        }
    }
}