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
using Vha.Chat;
using Vha.Chat.Events;

namespace Vha.Chat.UI
{
    public partial class ChatForm : BaseForm
    {
        protected ChatTreeNode _online = new ChatTreeNode(MessageType.Character, "Online");
        protected ChatTreeNode _offline = new ChatTreeNode(MessageType.Character, "Offline");
        protected ChatTreeNode _channels = new ChatTreeNode(MessageType.Channel, "Channels");
        protected ChatTreeNode _privateChannels = new ChatTreeNode(MessageType.PrivateChannel, "Private Channels");
        protected ChatTreeNode _guests = new ChatTreeNode(MessageType.Character, "Guests");

        protected ChatHtml _htmlUtil;
        protected Context _context;

        protected List<string> _history = new List<string>();
        protected int _historyIndex = 0;

        public ChatForm(Context context)
            : base(context, "Chat")
        {
            InitializeComponent();
            base.Initialize();

            this._context = context;
            this._context.Options.SavedEvent += new Handler<Options>(_context_SavedEvent);
            this._context.StateEvent += new Handler<StateEventArgs>(_context_StateEvent);
            this._context.MessageEvent += new Handler<MessageEventArgs>(_context_MessageEvent);
            this._context.ChannelJoinEvent += new Handler<ChannelEventArgs>(_context_ChannelJoinEvent);
            this._context.ChannelUpdatedEvent += new Handler<ChannelEventArgs>(_context_ChannelUpdatedEvent);
            this._context.PrivateChannelInviteEvent += new Handler<PrivateChannelInviteEventArgs>(_context_PrivateChannelInviteEvent);
            this._context.PrivateChannelJoinEvent += new Handler<PrivateChannelEventArgs>(_context_PrivateChannelJoinEvent);
            this._context.PrivateChannelLeaveEvent += new Handler<PrivateChannelEventArgs>(_context_PrivateChannelLeaveEvent);
            this._context.CharacterJoinEvent += new Handler<PrivateChannelEventArgs>(_context_CharacterJoinEvent);
            this._context.CharacterLeaveEvent += new Handler<PrivateChannelEventArgs>(_context_CharacterLeaveEvent);
            this._context.FriendAddedEvent += new Handler<FriendEventArgs>(_context_FriendAddedEvent);
            this._context.FriendRemovedEvent += new Handler<FriendEventArgs>(_context_FriendRemovedEvent);
            this._context.FriendUpdatedEvent += new Handler<FriendEventArgs>(_context_FriendUpdatedEvent);
            
            this._tree.Nodes.Add(this._online);
            this._tree.Nodes.Add(this._offline);
            this._tree.Nodes.Add(this._channels);
            this._tree.Nodes.Add(this._privateChannels);
            this._tree.Nodes.Add(this._guests);

            this._htmlUtil = new ChatHtml(this._context, this);

            this._outputBox.BackgroundColor = this.BackColor;
            this._outputBox.ForegroundColor = this.ForeColor;
            this._outputBox.ClickedEvent += new AomlHandler<AomlClickedEventArgs>(_outputBox_ClickedEvent);

            // Update buttons to reflect the state of chat.
            switch (this._context.State)
            {
                case ContextState.Disconnected:
                    this._connect.Visible = this._connect.Enabled = true;
                    this._disconnect.Visible = this._disconnect.Enabled = false;
                    break;
                default:
                    this._connect.Visible = this._connect.Enabled = false;
                    this._disconnect.Visible = this._disconnect.Enabled = true;
                    break;
            }

            // Force options update manually
            this._context_SavedEvent(this._context, this._context.Options);

            // A gentle welcome message
            this._context.Write(MessageClass.Internal, "Type /help to view all available commands");
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._context.Options.SavedEvent -= new Handler<Options>(_context_SavedEvent);
            this._context.StateEvent -= new Handler<StateEventArgs>(_context_StateEvent);
            this._context.MessageEvent -= new Handler<MessageEventArgs>(_context_MessageEvent);
            this._context.ChannelJoinEvent -= new Handler<ChannelEventArgs>(_context_ChannelJoinEvent);
            this._context.ChannelUpdatedEvent -= new Handler<ChannelEventArgs>(_context_ChannelUpdatedEvent);
            this._context.PrivateChannelInviteEvent -= new Handler<PrivateChannelInviteEventArgs>(_context_PrivateChannelInviteEvent);
            this._context.PrivateChannelJoinEvent -= new Handler<PrivateChannelEventArgs>(_context_PrivateChannelJoinEvent);
            this._context.PrivateChannelLeaveEvent -= new Handler<PrivateChannelEventArgs>(_context_PrivateChannelLeaveEvent);
            this._context.CharacterJoinEvent -= new Handler<PrivateChannelEventArgs>(_context_CharacterJoinEvent);
            this._context.CharacterLeaveEvent -= new Handler<PrivateChannelEventArgs>(_context_CharacterLeaveEvent);
            this._context.FriendAddedEvent -= new Handler<FriendEventArgs>(_context_FriendAddedEvent);
            this._context.FriendRemovedEvent -= new Handler<FriendEventArgs>(_context_FriendRemovedEvent);
            this._context.FriendUpdatedEvent -= new Handler<FriendEventArgs>(_context_FriendUpdatedEvent);
            // If we're still the main form at this stage, let's just call it quits
            if (Program.ApplicationContext.MainForm == this)
            {
                this._context.Disconnect();
                Application.Exit();
            }
        }

        public delegate void SetTargetDelegate(MessageTarget target);
        public void SetTarget(MessageTarget target)
        {
            if (this._target.InvokeRequired)
            {
                this._target.BeginInvoke(new SetTargetDelegate(SetTarget), new object[] { target });
                return;
            }
            // Focus input
            this._inputBox.Focus();
            this._inputBox.Select(this._inputBox.Text.Length, 0);
            // Check if the target already exists in our list
            foreach (object t in this._target.Items)
            {
                MessageTarget ct = (MessageTarget)t;
                if (!ct.Equals(target)) continue;
                this._target.SelectedItem = t;
                return;
            }
            // Add new target
            int index = 0;
            foreach (MessageTarget t in this._target.Items)
            {
                if (t.Target.CompareTo(target.Target) > 0)
                {
                    this._target.Items.Insert(index, target);
                    this._target.SelectedIndex = index;
                    return;
                }
                index++;
            }
            index = this._target.Items.Add(target);
            this._target.SelectedIndex = index;
        }

        #region Context callbacks
        void _context_SavedEvent(Context context, Options args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<Options>(_context_SavedEvent),
                    new object[] { context, args });
                return;
            }
            // Move panel to the left
            if (args.PanelPosition == HorizontalPosition.Left &&
                this._container.RightToLeft != RightToLeft.Yes)
            {
                this._container.RightToLeft = RightToLeft.Yes;
                this._inputBox.RightToLeft = RightToLeft.No;
                this._target.RightToLeft = RightToLeft.No;
            }
            // Move panel to the right
            if (args.PanelPosition == HorizontalPosition.Right &&
                this._container.RightToLeft != RightToLeft.No)
            {
                this._container.RightToLeft = RightToLeft.No;
            }
            // TODO: update size thingy

            // Update AomlBox settings
            this._outputBox.MaximumTexts = args.MaximumTexts;
            this._outputBox.MaximumLines = args.MaximumMessages;
        }

        void _context_StateEvent(Context context, StateEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<StateEventArgs>(_context_StateEvent),
                    new object[] { context, args });
                return;
            }
            // Update buttons when disconnect
            if (args.State == ContextState.Disconnected)
            {
                this._connect.Enabled = this._connect.Visible = true;
                this._disconnect.Enabled = this._disconnect.Visible = false;
            }
            // Could use some cleaning up when reconnecting
            else if (args.State == ContextState.Reconnecting)
            {
                // - Clear tree sections
                this._channels.Nodes.Clear();
                this._online.Nodes.Clear();
                this._offline.Nodes.Clear();
                this._guests.Nodes.Clear();
                this._privateChannels.Nodes.Clear();
                // Update buttons
                this._connect.Enabled = this._connect.Visible = false;
                this._disconnect.Enabled = this._disconnect.Visible = true;
            }
            // And need this when connected
            else if (args.State == ContextState.Connected)
            {
                // - Add ourselves to private channel
                this._privateChannels.AddNode(context.Character, "Character");
                this._privateChannels.Expand();
                // Update buttons
                this._connect.Enabled = this._connect.Visible = false;
                this._disconnect.Enabled = this._disconnect.Visible = true;
            }
            // Notify state change
            context.Write(MessageClass.Internal, "State changed to: " + args.State.ToString());
        }

        void _context_MessageEvent(Context context, MessageEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<MessageEventArgs>(_context_MessageEvent),
                    new object[] { context, args });
                return;
            }
            // Create message
            string line = "";
            // - Append channel
            switch (args.Source.Type)
            {
                case MessageType.Channel:
                    if (!string.IsNullOrEmpty(args.Source.Channel))
                        line = string.Format("[<a href=\"channel://{0}\" class=\"Link\">{0}</a>] ", args.Source.Channel);
                    break;
                case MessageType.Character:
                    line = string.Format(
                        "{1}[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: ",
                        args.Source.Character, args.Source.Outgoing ? "To " : "");
                    break;
                case MessageType.PrivateChannel:
                    line = string.Format("[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] ", args.Source.Channel);
                    break;
            }
            // - Append character
            switch (args.Source.Type)
            {
                case MessageType.Channel:
                case MessageType.PrivateChannel:
                    if (!string.IsNullOrEmpty(args.Source.Character))
                        line += string.Format("<a href=\"character://{0}\" class=\"Link\">{0}</a>: ", args.Source.Character);
                    break;
            }
            // - Append message
            line += args.Message;
            // Format message into html
            string aoml = string.Format(
                "<div class=\"Line\"><span class=\"Time\">[{0:00}:{1:00}:{2:00}]</span> <span class=\"{3}\">{4}</span></div>",
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                args.Class.ToString(), line);
            this._outputBox.Write(aoml, context.Options.TextStyle, true);
        }

        void _context_CharacterLeaveEvent(Context context, PrivateChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<PrivateChannelEventArgs>(_context_CharacterLeaveEvent),
                    new object[] { context, args });
                return;
            }
            // (not sure this ever happens, but better safe than sorry)
            if (args.Character == context.Character) return;
            // Update list
            this._guests.RemoveNode(args.Character);
        }

        void _context_CharacterJoinEvent(Context context, PrivateChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<PrivateChannelEventArgs>(_context_CharacterJoinEvent),
                    new object[] { context, args });
                return;
            }
            // (not sure this ever happens, but better safe than sorry)
            if (args.Character == context.Character) return;
            // Update list
            this._guests.AddNode(args.Character, "Character");
            if (this._guests.Nodes.Count == 1)
                this._guests.Expand();
        }

        void _context_PrivateChannelLeaveEvent(Context context, PrivateChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<PrivateChannelEventArgs>(_context_PrivateChannelLeaveEvent),
                    new object[] { context, args });
                return;
            }
            // Ignore other characters leaving
            if (args.Character != context.Character) return;
            // Update list
            this._privateChannels.RemoveNode(args.Channel.Name);
        }

        void _context_PrivateChannelJoinEvent(Context context, PrivateChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<PrivateChannelEventArgs>(_context_PrivateChannelJoinEvent),
                    new object[] { context, args });
                return;
            }

            // Ignore other characters joining
            if (args.Character != context.Character) return;
            // Update list
            this._privateChannels.AddNode(args.Channel.Name, "Character");
            if (this._privateChannels.Nodes.Count == 1)
                this._privateChannels.Expand();
        }

        void _context_PrivateChannelInviteEvent(Context context, PrivateChannelInviteEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<PrivateChannelInviteEventArgs>(_context_PrivateChannelInviteEvent),
                    new object[] { context, args });
                return;
            }
            // Show dialog
            DialogResult result = MessageBox.Show(
                "You have been invited to " + args.Channel.Name + "'s private channel. Do you wish to join?",
                "Private Channel Invite",
                MessageBoxButtons.YesNo);
            // Join channel if accepted
            if (result == DialogResult.Yes)
                args.Accept();
            else
                args.Decline();
        }

        void _context_ChannelJoinEvent(Context context, ChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<ChannelEventArgs>(_context_ChannelJoinEvent),
                    new object[] { context, args });
                return;
            }
            if (args.Channel.Muted) this._channels.AddNode(args.Channel.Name, "ChannelDisabled");
            else this._channels.AddNode(args.Channel.Name, "Channel");
            if (this._channels.Nodes.Count == 1)
                this._channels.Expand();
        }

        void _context_ChannelUpdatedEvent(Context context, ChannelEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<ChannelEventArgs>(_context_ChannelUpdatedEvent),
                    new object[] { context, args });
                return;
            }
            TreeNode node = this._channels.GetNode(args.Channel.Name);
            if (args.Channel.Muted) node.ImageKey = node.SelectedImageKey = "ChannelDisabled";
            else node.ImageKey = node.SelectedImageKey = "Channel";
        }

        void _context_FriendAddedEvent(Context context, FriendEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<FriendEventArgs>(_context_FriendAddedEvent),
                    new object[] { context, args });
                return;
            }
            if (args.Friend.Online)
            {
                this._online.AddNode(args.Friend.Name, "CharacterOnline");
                if (this._online.Nodes.Count == 1)
                    this._online.Expand();
            }
            else
            {
                this._offline.AddNode(args.Friend.Name, "CharacterOffline");
            }
        }

        void _context_FriendRemovedEvent(Context context, FriendEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<FriendEventArgs>(_context_FriendRemovedEvent),
                    new object[] { context, args });
                return;
            }
            this._offline.RemoveNode(args.Friend.Name);
            this._online.RemoveNode(args.Friend.Name);
        }


        void _context_FriendUpdatedEvent(Context context, FriendEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Handler<FriendEventArgs>(_context_FriendUpdatedEvent),
                    new object[] { context, args });
                return;
            }
            if (args.Friend.Online)
            {
                this._offline.RemoveNode(args.Friend.Name);
                this._online.AddNode(args.Friend.Name, "CharacterOnline");
                if (this._online.Nodes.Count == 1)
                    this._online.Expand();
            }
            else
            {
                this._online.RemoveNode(args.Friend.Name);
                this._offline.AddNode(args.Friend.Name, "CharacterOffline");
            }
        }
        #endregion

        #region Form callbacks
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
                while (this._history.Count > this._context.Options.MaximumHistory)
                    this._history.RemoveAt(this._history.Count - 1);
                this._historyIndex = 0;
                // Handle the input
                if (this._inputBox.Text.StartsWith(this._context.Input.Prefix))
                {
                    this._context.Input.Command(HTML.EscapeString(this._inputBox.Text));
                    this._inputBox.Text = "";
                    return;
                }
                if (this._target.SelectedItem == null)
                {
                    this._context.Write(MessageClass.Error, "No channel selected");
                    return;
                }
                MessageTarget target = (MessageTarget)this._target.SelectedItem;
                this._context.Input.Send(target, HTML.EscapeString(this._inputBox.Text), false);
                this._inputBox.Text = "";
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void _outputBox_ClickedEvent(AomlBox sender, AomlClickedEventArgs e)
        {
            this._htmlUtil.Link(e.Type, e.Argument);
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
            this.SetTarget(new MessageTarget(branch.Type, target));
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
                if (this._context.HasGuest(target))
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
                if (this._context.GetChannel(target).Muted)
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
                if (target == this._context.Character)
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
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void _connect_Click(object sender, EventArgs e)
        {
            // Return to authorization window
            Program.ApplicationContext.MainForm = new AuthenticationForm(this._context);
            // Close this window before showing auth window
            this.Close();
            Program.ApplicationContext.MainForm.Show();
        }

        private void _disconnect_Click(object sender, EventArgs e)
        {
            this._context.Disconnect();
            // Disable form
            this._disconnect.Enabled = false;
            this._connect.Enabled = true;
        }

        private void _options_Click(object sender, EventArgs e)
        {
            Form options = new OptionsForm(this._context);
            options.ShowDialog();
        }

        private void _channelMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            SetTarget(new MessageTarget(MessageType.Channel, (string)this._channelMenu.Tag));
        }

        private void _channelMenu_Mute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._context.Input.Command("mute " + (string)this._channelMenu.Tag);
        }

        private void _channelMenu_Unmute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._context.Input.Command("unmute " + (string)this._channelMenu.Tag);
        }

        private void _privateChannelMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            SetTarget(new MessageTarget(MessageType.PrivateChannel, (string)this._privateChannelMenu.Tag));
        }

        private void _privateChannelMenu_Leave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            this._context.Input.Command("leave " + (string)this._privateChannelMenu.Tag);
        }

        private void _characterMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            SetTarget(new MessageTarget(MessageType.Character, (string)this._characterMenu.Tag));
        }

        private void _characterMenu_Remove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("friend remove " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Invite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("invite " + (string)this._characterMenu.Tag);
        }

        private void _guestsMenu_Kick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._guestsMenu.Tag)) return;
            this._context.Input.Command("kick " + (string)this._guestsMenu.Tag);
        }
        #endregion
    }
}