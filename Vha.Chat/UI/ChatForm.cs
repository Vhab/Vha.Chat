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
using Vha.Chat.Commands;
using Vha.Chat.Events;
using Vha.Chat.UI.Controls;

namespace Vha.Chat.UI
{
    public partial class ChatForm : BaseForm
    {
        protected ChatTreeNode _onlineFriends = new ChatTreeNode(MessageType.Character, "Online");
        protected ChatTreeNode _offlineFriends = new ChatTreeNode(MessageType.Character, "Offline");
        protected ChatTreeNode _recentFriends = new ChatTreeNode(MessageType.Character, "Recent");
        protected ChatTreeNode _channels = new ChatTreeNode(MessageType.Channel, "Channels");
        protected ChatTreeNode _privateChannels = new ChatTreeNode(MessageType.PrivateChannel, "Private Channels");
        protected ChatTreeNode _guests = new ChatTreeNode(MessageType.Character, "Guests");

        protected Context _context;
        protected ChatContextMenu _contextMenu;
        protected bool _initialized = false;

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
            
            this._tree.Nodes.Add(this._onlineFriends);
            this._tree.Nodes.Add(this._offlineFriends);
            this._tree.Nodes.Add(this._recentFriends);
            this._tree.Nodes.Add(this._channels);
            this._tree.Nodes.Add(this._privateChannels);
            this._tree.Nodes.Add(this._guests);

            this._contextMenu = new ChatContextMenu(this, context, this);
            this._outputBox.ContextMenu = this._contextMenu;
            this._outputBox.Context = context;
            this._outputBox.BackgroundColor = this.BackColor;
            this._outputBox.ForegroundColor = this.ForeColor;
            this._outputBox.ClickedEvent += new OutputControlHandler<OutputControlClickedEventArgs>(_outputBox_ClickedEvent);
            this._outputBox.Initialize(context.Configuration.OutputMode);

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

            // A gentle welcome message
            this._context.Write(MessageClass.Internal, "Type /help to view all available commands");

            // Enable commands
            this._context.Input.RegisterCommand(new UI.Commands.OpenCommand(this));
            this._context.Input.RegisterCommand(new UI.Commands.StartCommand());

            // Wait for load event
            this.Load += new EventHandler(ChatForm_Load);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            // Force options update manually
            this._context_SavedEvent(this._context, this._context.Options);
            // Ready!
            this._initialized = true;
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
            // Disable open command
            this._context.Input.UnregisterCommandByTrigger("open");
            this._context.Input.UnregisterCommandByTrigger("start");
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
            // Update splitter size
            OptionsSize splitter = this._context.Options.GetSize("ChatForm", "Splitter", false);
            if (splitter != null)
            {
                this._container.SplitterDistance = splitter.Size;
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
            // Update AomlBox settings
            this._outputBox.MaximumTexts = args.MaximumTexts;
            this._outputBox.MaximumLines = args.MaximumMessages;
            this._outputBox.TextSize = args.ChatTextSize;
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
                this._onlineFriends.Nodes.Clear();
                this._offlineFriends.Nodes.Clear();
                this._recentFriends.Nodes.Clear();
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
            string prefix = "";
            // - Append channel
            switch (args.Source.Type)
            {
                case MessageType.Channel:
                    if (!string.IsNullOrEmpty(args.Source.Channel))
                        prefix = string.Format("[<a href=\"channel://{0}\" class=\"Link\">{0}</a>] ", args.Source.Channel);
                    break;
                case MessageType.Character:
                    prefix = string.Format(
                        "{1}[<a href=\"character://{0}\" class=\"Link\">{0}</a>]: ",
                        args.Source.Character, args.Source.Outgoing ? "To " : "");
                    break;
                case MessageType.PrivateChannel:
                    prefix = string.Format("[<a href=\"privchan://{0}\" class=\"Link\">{0}</a>] ", args.Source.Channel);
                    break;
            }
            // - Append character
            switch (args.Source.Type)
            {
                case MessageType.Channel:
                case MessageType.PrivateChannel:
                    if (!string.IsNullOrEmpty(args.Source.Character))
                        prefix += string.Format("<a href=\"character://{0}\" class=\"Link\">{0}</a>: ", args.Source.Character);
                    break;
            }
            // Format message into aoml
            string template = string.Format(
                "<div class=\"Line\"><span class=\"Time\">[{0:00}:{1:00}:{2:00}]</span> <span class=\"{3}\">{4}{{0}}</span></div>",
                args.Time.Hour, args.Time.Minute, args.Time.Second,
                args.Class.ToString(), prefix);
            this._outputBox.Write(template, args.Message, context.Options.TextStyle, true);
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
            if ((args.Channel.Flags & ChannelFlags.Muted) != 0) this._channels.AddNode(args.Channel.Name, "ChannelDisabled");
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
            // Check if the channel was renamed
            if (args.PreviousChannel != null && args.PreviousChannel.Name != args.Channel.Name)
            {
                // Let's pretend we joined a new channel
                this._channels.RemoveNode(args.PreviousChannel.Name);
                this._context_ChannelJoinEvent(context, args);
                return;
            }
            // Update the channel
            TreeNode node = this._channels.GetNode(args.Channel.Name);
            if ((args.Channel.Flags & ChannelFlags.Muted) != 0) node.ImageKey = node.SelectedImageKey = "ChannelDisabled";
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
            string icon = args.Friend.Online ? "CharacterOnline" : "CharacterOffline";
            if (args.Friend.Temporary)
            {
                this._recentFriends.AddNode(args.Friend.Name, icon);
            }
            else if (args.Friend.Online)
            {
                this._onlineFriends.AddNode(args.Friend.Name, icon);
                if (this._onlineFriends.Nodes.Count == 1)
                    this._onlineFriends.Expand();
            }
            else
            {
                this._offlineFriends.AddNode(args.Friend.Name, icon);
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
            this._offlineFriends.RemoveNode(args.Friend.Name);
            this._onlineFriends.RemoveNode(args.Friend.Name);
            this._recentFriends.RemoveNode(args.Friend.Name);
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
            // Remove from all lists
            this._onlineFriends.RemoveNode(args.Friend.Name);
            this._offlineFriends.RemoveNode(args.Friend.Name);
            this._recentFriends.RemoveNode(args.Friend.Name);
            // Determine icon
            string icon = args.Friend.Online ? "CharacterOnline" : "CharacterOffline";
            // Add to appropriate list
            if (args.Friend.Temporary)
            {
                this._recentFriends.AddNode(args.Friend.Name, icon);
            }
            else if (args.Friend.Online)
            {
                this._onlineFriends.AddNode(args.Friend.Name, icon);
                if (this._onlineFriends.Nodes.Count == 1)
                    this._onlineFriends.Expand();
            }
            else
            {
                this._offlineFriends.AddNode(args.Friend.Name, icon);
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
                    this._context.Input.Command(Web.EscapeHtml(this._inputBox.Text));
                    this._inputBox.Text = "";
                    return;
                }
                if (this._target.SelectedItem == null)
                {
                    this._context.Write(MessageClass.Error, "No channel selected");
                    return;
                }
                MessageTarget target = (MessageTarget)this._target.SelectedItem;
                this._context.Input.Send(target, Web.EscapeHtml(this._inputBox.Text), false);
                this._inputBox.Text = "";
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void _outputBox_ClickedEvent(OutputControl sender, OutputControlClickedEventArgs e)
        {
            // Handle only left click, except for when shift is pressed
            if (e.ButtonsPressed != MouseButtons.Left || e.ShiftPressed)
                return;
            // Determine target
            MessageTarget target = null;
            switch (e.Type)
            {
                case "character":
                    target = new MessageTarget(MessageType.Character, e.Argument);
                    break;
                case "channel":
                    target = new MessageTarget(MessageType.Channel, e.Argument);
                    break;
                case "privchan":
                    target = new MessageTarget(MessageType.PrivateChannel, e.Argument);
                    break;
                default:
                    return;
            }
            // We'll be handling this event
            e.Handled = true;
            // Ignore local character
            if (target.Type == MessageType.Character &&
                target.Target.ToLower() == this._context.Character.ToLower())
                return;
            // Switch target
            this.SetTarget(target);
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
            if (e.Node.Parent == this._onlineFriends ||
                e.Node.Parent == this._offlineFriends ||
                e.Node.Parent == this._recentFriends)
            {
                // Character menu
                this._contextMenu.Show(new MessageTarget(MessageType.Character, target));
            }
            else if (e.Node.Parent == this._channels)
            {
                // Channel menu
                this._contextMenu.Show(new MessageTarget(MessageType.Channel, target));
            }
            else if (e.Node.Parent == this._privateChannels)
            {
                // Private channel menu
                this._contextMenu.Show(new MessageTarget(MessageType.PrivateChannel, target));
            }
            else if (e.Node.Parent == this._guests)
            {
                // Character menu
                this._contextMenu.Show(new MessageTarget(MessageType.Character, target));
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

        private void _ignores_Click(object sender, EventArgs e)
        {
            Form ignores = new IgnoresForm(this._context);
            ignores.ShowDialog();
        }

        private void _container_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this._context == null || !this._initialized) return;
            OptionsSize size = this._context.Options.GetSize("ChatForm", "Splitter", true);
            size.Size = this._container.SplitterDistance;
        }
        #endregion

        
    }
}