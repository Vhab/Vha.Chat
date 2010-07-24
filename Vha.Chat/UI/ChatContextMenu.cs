using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Vha.Chat.Properties;
using Vha.Common;
using Vha.Net;

namespace Vha.Chat.UI
{
    public class ChatContextMenu
    {
        public ChatContextMenu(Control parent, Context context) : this(parent, context, null) { }

        public ChatContextMenu(Control parent, Context context, ChatForm form)
        {
            if (parent == null || context == null)
                throw new ArgumentNullException();
            this._parent = parent;
            this._context = context;
            this._form = form;

            // Create character menu
            this._characterMenu_TalkTo = new ToolStripMenuItem(
                "Talk To...", Resources.TalkBitmap, new EventHandler(_characterMenu_TalkTo_Click));
            this._characterMenu_Open = new ToolStripMenuItem(
                "Open", Resources.PopupBitmap, new EventHandler(_characterMenu_Open_Click));
            this._characterMenu_Kick = new ToolStripMenuItem(
                "Kick", Resources.LeaveBitmap, new EventHandler(_characterMenu_Kick_Click));
            this._characterMenu_Invite = new ToolStripMenuItem(
                "Invite", Resources.EnterBitmap, new EventHandler(_characterMenu_Invite_Click));
            this._characterMenu_Add = new ToolStripMenuItem(
                "Add", Resources.CharacterAddBitmap, new EventHandler(_characterMenu_Add_Click));
            this._characterMenu_Remove = new ToolStripMenuItem(
                "Remove", Resources.CharacterDeleteBitmap, new EventHandler(_characterMenu_Remove_Click));
            this._characterMenu_Whois = new ToolStripMenuItem(
                "Whois", Resources.CharacterInformationBitmap, new EventHandler(_characterMenu_Whois_Click));
            this._characterMenu_Ignore = new ToolStripMenuItem(
                "Ignore", Resources.CharacterDisabledBitmap, new EventHandler(_characterMenu_Ignore_Click));
            this._characterMenu_Unignore = new ToolStripMenuItem(
                "Unignore", Resources.CharacterBitmap, new EventHandler(_characterMenu_Unignore_Click));
            this._characterMenu = new ContextMenuStrip();
            this._characterMenu.Items.AddRange(new ToolStripItem[] {
                this._characterMenu_TalkTo,
                this._characterMenu_Open,
                new ToolStripSeparator(),
                this._characterMenu_Whois,
                this._characterMenu_Kick,
                this._characterMenu_Invite,
                this._characterMenu_Add,
                this._characterMenu_Remove,
                new ToolStripSeparator(),
                this._characterMenu_Ignore,
                this._characterMenu_Unignore
            });

            // Create channel menu
            this._channelMenu_TalkTo = new ToolStripMenuItem(
                "Talk To...", Resources.TalkBitmap, new EventHandler(_channelMenu_TalkTo_Click));
            this._channelMenu_Open = new ToolStripMenuItem(
                "Open", Resources.PopupBitmap, new EventHandler(_channelMenu_Open_Click));
            this._channelMenu_Mute = new ToolStripMenuItem(
                "Mute", Resources.ChannelBitmap, new EventHandler(_channelMenu_Mute_Click));
            this._channelMenu_Unmute = new ToolStripMenuItem(
                "Unmute", Resources.ChannelDisabledBitmap, new EventHandler(_channelMenu_Unmute_Click));
            this._channelMenu = new ContextMenuStrip();
            this._channelMenu.Items.AddRange(new ToolStripItem[] {
                this._channelMenu_TalkTo,
                this._channelMenu_Open,
                new ToolStripSeparator(),
                this._channelMenu_Mute,
                this._channelMenu_Unmute,
            });

            // Create private channel menu
            this._privateChannelMenu_TalkTo = new ToolStripMenuItem(
                "Talk To...", Resources.TalkBitmap, new EventHandler(_privateChannelMenu_TalkTo_Click));
            this._privateChannelMenu_Open = new ToolStripMenuItem(
                "Open", Resources.PopupBitmap, new EventHandler(_privateChannelMenu_Open_Click));
            this._privateChannelMenu_Leave = new ToolStripMenuItem(
                "Leave", Resources.LeaveBitmap, new EventHandler(_privateChannelMenu_Leave_Click));
            this._privateChannelMenu_KickAll = new ToolStripMenuItem(
                "Kick Everyone", Resources.LeaveBitmap, new EventHandler(_privateChannelMenu_KickAll_Click));
            this._privateChannelMenu = new ContextMenuStrip();
            this._privateChannelMenu.Items.AddRange(new ToolStripItem[] {
                this._privateChannelMenu_TalkTo,
                this._privateChannelMenu_Open,
                new ToolStripSeparator(),
                this._privateChannelMenu_Leave,
                this._privateChannelMenu_KickAll,
            });
        }

        public void Show(MessageTarget target) { this.Show(target, Cursor.Position); }
        private delegate void ShowDelegate(MessageTarget target, Point location);
        public void Show(MessageTarget target, Point location)
        {
            // Error handling
            if (target == null || location == null)
                throw new ArgumentNullException();
            // No context when not connected
            if (this._context.State != ContextState.Connected)
                return;
            // Thread safety
            if (this._parent.InvokeRequired)
            {
                this._parent.Invoke(
                    new ShowDelegate(Show),
                    new object[] { target, location });
                return;
            }
            // Determine type
            switch (target.Type)
            {
                case MessageType.Character:
                    this._showCharacter(target.Target, location);
                    break;
                case MessageType.Channel:
                    this._showChannel(target.Target, location);
                    break;
                case MessageType.PrivateChannel:
                    this._showPrivateChannel(target.Target, location);
                    break;
            }
        }

        private void _showCharacter(string character, Point location)
        {
            // Some sanity
            character = Format.UppercaseFirst(character);
            if (character == this._context.Character) return;
            if (!this._context.Input.CheckCharacter(character, false)) return;
            Friend f = this._context.GetFriend(character);
            // Show
            this._characterMenu.Tag = character;
            this._characterMenu.Show(location);
            // Setup
            this._characterMenu_TalkTo.Visible = this._form != null;
            this._characterMenu_Add.Visible = f == null;
            this._characterMenu_Remove.Visible = !this._characterMenu_Add.Visible;
            if (f != null && !f.Online)
            {
                // We can't kick or invite offline characters
                this._characterMenu_Invite.Visible = false;
                this._characterMenu_Kick.Visible = false;
            }
            else
            {
                this._characterMenu_Invite.Visible = !this._context.HasGuest(character);
                this._characterMenu_Kick.Visible = !this._characterMenu_Invite.Visible;
            }
            this._characterMenu_Ignore.Visible = !this._context.Ignores.Contains(character);
            this._characterMenu_Unignore.Visible = !this._characterMenu_Ignore.Visible;
        }

        private void _showChannel(string channel, Point location)
        {
            // Some sanity
            Channel c = this._context.GetChannel(channel);
            if (c == null) return;
            // Show
            this._channelMenu.Tag = c.Name;
            this._channelMenu.Show(location);
            // Setup
            this._channelMenu_TalkTo.Visible = this._form != null;
            this._channelMenu_Mute.Visible = !c.Muted;
            this._channelMenu_Unmute.Visible = !this._channelMenu_Mute.Visible;
        }

        private void _showPrivateChannel(string character, Point location)
        {
            // Some sanity
            character = Format.UppercaseFirst(character);
            if (!this._context.Input.CheckCharacter(character, false)) return;
            // Show
            this._privateChannelMenu.Tag = character;
            this._privateChannelMenu.Show(location);
            // Setup
            this._privateChannelMenu_TalkTo.Visible = this._form != null;
            this._privateChannelMenu_Leave.Visible = character != this._context.Character;
            this._privateChannelMenu_KickAll.Visible = !this._privateChannelMenu_Leave.Visible;
        }

        #region Members
        private Control _parent = null;
        private Context _context = null;
        private ChatForm _form = null;

        private ContextMenuStrip _characterMenu = null;
        private ToolStripMenuItem _characterMenu_TalkTo = null;
        private ToolStripMenuItem _characterMenu_Open = null;
        private ToolStripMenuItem _characterMenu_Kick = null;
        private ToolStripMenuItem _characterMenu_Invite = null;
        private ToolStripMenuItem _characterMenu_Add = null;
        private ToolStripMenuItem _characterMenu_Remove = null;
        private ToolStripMenuItem _characterMenu_Whois = null;
        private ToolStripMenuItem _characterMenu_Ignore = null;
        private ToolStripMenuItem _characterMenu_Unignore = null;

        private ContextMenuStrip _channelMenu = null;
        private ToolStripMenuItem _channelMenu_TalkTo = null;
        private ToolStripMenuItem _channelMenu_Open = null;
        private ToolStripMenuItem _channelMenu_Mute = null;
        private ToolStripMenuItem _channelMenu_Unmute = null;
        
        private ContextMenuStrip _privateChannelMenu = null;
        private ToolStripMenuItem _privateChannelMenu_TalkTo = null;
        private ToolStripMenuItem _privateChannelMenu_Open = null;
        private ToolStripMenuItem _privateChannelMenu_Leave = null;
        private ToolStripMenuItem _privateChannelMenu_KickAll = null;
        #endregion

        #region Callbacks
        private void _characterMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (this._context.State != ContextState.Connected) return;
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._form.SetTarget(new MessageTarget(MessageType.Character, (string)this._characterMenu.Tag));
        }

        private void _characterMenu_Open_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("open character " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("friend add " + (string)this._characterMenu.Tag);
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

        private void _characterMenu_Kick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("kick " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Whois_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("whois " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Ignore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("ignore " + (string)this._characterMenu.Tag);
        }

        private void _characterMenu_Unignore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._characterMenu.Tag)) return;
            this._context.Input.Command("unignore " + (string)this._characterMenu.Tag);
        }

        private void _channelMenu_TalkTo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._form.SetTarget(new MessageTarget(MessageType.Channel, (string)this._channelMenu.Tag));
        }

        private void _channelMenu_Open_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._channelMenu.Tag)) return;
            this._context.Input.Command("open channel " + (string)this._channelMenu.Tag);
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
            this._form.SetTarget(new MessageTarget(MessageType.PrivateChannel, (string)this._privateChannelMenu.Tag));
        }

        private void _privateChannelMenu_Open_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            this._context.Input.Command("open privatechannel " + (string)this._privateChannelMenu.Tag);
        }

        private void _privateChannelMenu_Leave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)this._privateChannelMenu.Tag)) return;
            this._context.Input.Command("leave " + (string)this._privateChannelMenu.Tag);
        }

        private void _privateChannelMenu_KickAll_Click(object sender, EventArgs e)
        {
            this._context.Input.Command("kickall");
        }
        #endregion
    }
}
