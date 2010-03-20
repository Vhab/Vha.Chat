namespace Vha.Chat
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            this._tree = new System.Windows.Forms.TreeView();
            this._icons = new System.Windows.Forms.ImageList(this.components);
            this._outputBox = new System.Windows.Forms.WebBrowser();
            this._inputBox = new System.Windows.Forms.TextBox();
            this._target = new System.Windows.Forms.ComboBox();
            this._connect = new System.Windows.Forms.ToolStripButton();
            this._disconnect = new System.Windows.Forms.ToolStripButton();
            this._options = new System.Windows.Forms.ToolStripButton();
            this._about = new System.Windows.Forms.ToolStripButton();
            this._buttons = new System.Windows.Forms.ToolStrip();
            this._channelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._channelMenu_TalkTo = new System.Windows.Forms.ToolStripMenuItem();
            this._channelMenu_Seperator = new System.Windows.Forms.ToolStripSeparator();
            this._channelMenu_Mute = new System.Windows.Forms.ToolStripMenuItem();
            this._channelMenu_Unmute = new System.Windows.Forms.ToolStripMenuItem();
            this._privateChannelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._privateChannelMenu_TalkTo = new System.Windows.Forms.ToolStripMenuItem();
            this._privateChannelMenu_Seperator = new System.Windows.Forms.ToolStripSeparator();
            this._privateChannelMenu_Leave = new System.Windows.Forms.ToolStripMenuItem();
            this._characterMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._characterMenu_TalkTo = new System.Windows.Forms.ToolStripMenuItem();
            this._characterMenu_Seperator = new System.Windows.Forms.ToolStripSeparator();
            this._characterMenu_Invite = new System.Windows.Forms.ToolStripMenuItem();
            this._characterMenu_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this._guestsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._guestsMenu_Kick = new System.Windows.Forms.ToolStripMenuItem();
            this._buttons.SuspendLayout();
            this._channelMenu.SuspendLayout();
            this._privateChannelMenu.SuspendLayout();
            this._characterMenu.SuspendLayout();
            this._guestsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tree.ImageIndex = 0;
            this._tree.ImageList = this._icons;
            this._tree.Location = new System.Drawing.Point(611, 35);
            this._tree.Name = "_tree";
            this._tree.SelectedImageIndex = 0;
            this._tree.Size = new System.Drawing.Size(182, 488);
            this._tree.TabIndex = 0;
            this._tree.DoubleClick += new System.EventHandler(this._tree_DoubleClick);
            this._tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._tree_NodeMouseClick);
            // 
            // _icons
            // 
            this._icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_icons.ImageStream")));
            this._icons.TransparentColor = System.Drawing.Color.Transparent;
            this._icons.Images.SetKeyName(0, "Characters");
            this._icons.Images.SetKeyName(1, "Channel");
            this._icons.Images.SetKeyName(2, "ChannelDisabled");
            this._icons.Images.SetKeyName(3, "Character");
            this._icons.Images.SetKeyName(4, "CharacterOffline");
            this._icons.Images.SetKeyName(5, "CharacterOnline");
            // 
            // _outputBox
            // 
            this._outputBox.AllowWebBrowserDrop = false;
            this._outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._outputBox.IsWebBrowserContextMenuEnabled = false;
            this._outputBox.Location = new System.Drawing.Point(6, 35);
            this._outputBox.MinimumSize = new System.Drawing.Size(20, 20);
            this._outputBox.Name = "_outputBox";
            this._outputBox.ScriptErrorsSuppressed = true;
            this._outputBox.Size = new System.Drawing.Size(599, 461);
            this._outputBox.TabIndex = 1;
            this._outputBox.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this._outputBox.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this._outputBox_Navigating);
            this._outputBox.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this._outputBox_DocumentCompleted);
            // 
            // _inputBox
            // 
            this._inputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._inputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._inputBox.Location = new System.Drawing.Point(154, 502);
            this._inputBox.Multiline = true;
            this._inputBox.Name = "_inputBox";
            this._inputBox.Size = new System.Drawing.Size(451, 21);
            this._inputBox.TabIndex = 2;
            this._inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._inputBox_KeyDown);
            this._inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._inputBox_KeyPress);
            // 
            // _target
            // 
            this._target.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._target.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._target.FormattingEnabled = true;
            this._target.Location = new System.Drawing.Point(6, 502);
            this._target.Name = "_target";
            this._target.Size = new System.Drawing.Size(142, 21);
            this._target.TabIndex = 4;
            // 
            // _connect
            // 
            this._connect.Enabled = false;
            this._connect.Image = global::Vha.Chat.Properties.Resources.ConnectBitmap;
            this._connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._connect.Margin = new System.Windows.Forms.Padding(0);
            this._connect.Name = "_connect";
            this._connect.Size = new System.Drawing.Size(72, 20);
            this._connect.Text = "Connect";
            this._connect.Click += new System.EventHandler(this._connect_Click);
            // 
            // _disconnect
            // 
            this._disconnect.Image = global::Vha.Chat.Properties.Resources.DisconnectBitmap;
            this._disconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._disconnect.Margin = new System.Windows.Forms.Padding(0);
            this._disconnect.Name = "_disconnect";
            this._disconnect.Size = new System.Drawing.Size(86, 20);
            this._disconnect.Text = "Disconnect";
            this._disconnect.Click += new System.EventHandler(this._disconnect_Click);
            // 
            // _options
            // 
            this._options.Image = global::Vha.Chat.Properties.Resources.OptionsBitmap;
            this._options.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._options.Margin = new System.Windows.Forms.Padding(0);
            this._options.Name = "_options";
            this._options.Size = new System.Drawing.Size(69, 20);
            this._options.Text = "Options";
            this._options.Click += new System.EventHandler(this._options_Click);
            // 
            // _about
            // 
            this._about.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._about.Image = global::Vha.Chat.Properties.Resources.AboutBitmap;
            this._about.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._about.Margin = new System.Windows.Forms.Padding(0);
            this._about.Name = "_about";
            this._about.Size = new System.Drawing.Size(60, 20);
            this._about.Text = "About";
            this._about.Click += new System.EventHandler(this._about_Click);
            // 
            // _buttons
            // 
            this._buttons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._buttons.AutoSize = false;
            this._buttons.Dock = System.Windows.Forms.DockStyle.None;
            this._buttons.GripMargin = new System.Windows.Forms.Padding(0);
            this._buttons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._buttons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._connect,
            this._disconnect,
            this._options,
            this._about});
            this._buttons.Location = new System.Drawing.Point(0, 0);
            this._buttons.Name = "_buttons";
            this._buttons.Padding = new System.Windows.Forms.Padding(6);
            this._buttons.Size = new System.Drawing.Size(799, 32);
            this._buttons.TabIndex = 5;
            // 
            // _channelMenu
            // 
            this._channelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._channelMenu_TalkTo,
            this._channelMenu_Seperator,
            this._channelMenu_Mute,
            this._channelMenu_Unmute});
            this._channelMenu.Name = "_channelMenu";
            this._channelMenu.Size = new System.Drawing.Size(123, 76);
            // 
            // _channelMenu_TalkTo
            // 
            this._channelMenu_TalkTo.Image = global::Vha.Chat.Properties.Resources.TalkBitmap;
            this._channelMenu_TalkTo.Name = "_channelMenu_TalkTo";
            this._channelMenu_TalkTo.Size = new System.Drawing.Size(122, 22);
            this._channelMenu_TalkTo.Text = "Talk To...";
            this._channelMenu_TalkTo.Click += new System.EventHandler(this._channelMenu_TalkTo_Click);
            // 
            // _channelMenu_Seperator
            // 
            this._channelMenu_Seperator.Name = "_channelMenu_Seperator";
            this._channelMenu_Seperator.Size = new System.Drawing.Size(119, 6);
            // 
            // _channelMenu_Mute
            // 
            this._channelMenu_Mute.Image = global::Vha.Chat.Properties.Resources.ChannelDisabledBitmap;
            this._channelMenu_Mute.Name = "_channelMenu_Mute";
            this._channelMenu_Mute.Size = new System.Drawing.Size(122, 22);
            this._channelMenu_Mute.Text = "Mute";
            this._channelMenu_Mute.Click += new System.EventHandler(this._channelMenu_Mute_Click);
            // 
            // _channelMenu_Unmute
            // 
            this._channelMenu_Unmute.Image = global::Vha.Chat.Properties.Resources.ChannelBitmap;
            this._channelMenu_Unmute.Name = "_channelMenu_Unmute";
            this._channelMenu_Unmute.Size = new System.Drawing.Size(122, 22);
            this._channelMenu_Unmute.Text = "Unmute";
            this._channelMenu_Unmute.Click += new System.EventHandler(this._channelMenu_Unmute_Click);
            // 
            // _privateChannelMenu
            // 
            this._privateChannelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._privateChannelMenu_TalkTo,
            this._privateChannelMenu_Seperator,
            this._privateChannelMenu_Leave});
            this._privateChannelMenu.Name = "_privateChannelMenu";
            this._privateChannelMenu.Size = new System.Drawing.Size(123, 54);
            // 
            // _privateChannelMenu_TalkTo
            // 
            this._privateChannelMenu_TalkTo.Image = global::Vha.Chat.Properties.Resources.TalkBitmap;
            this._privateChannelMenu_TalkTo.Name = "_privateChannelMenu_TalkTo";
            this._privateChannelMenu_TalkTo.Size = new System.Drawing.Size(122, 22);
            this._privateChannelMenu_TalkTo.Text = "Talk To...";
            this._privateChannelMenu_TalkTo.Click += new System.EventHandler(this._privateChannelMenu_TalkTo_Click);
            // 
            // _privateChannelMenu_Seperator
            // 
            this._privateChannelMenu_Seperator.Name = "_privateChannelMenu_Seperator";
            this._privateChannelMenu_Seperator.Size = new System.Drawing.Size(119, 6);
            // 
            // _privateChannelMenu_Leave
            // 
            this._privateChannelMenu_Leave.Image = global::Vha.Chat.Properties.Resources.LeaveBitmap;
            this._privateChannelMenu_Leave.Name = "_privateChannelMenu_Leave";
            this._privateChannelMenu_Leave.Size = new System.Drawing.Size(122, 22);
            this._privateChannelMenu_Leave.Text = "Leave";
            this._privateChannelMenu_Leave.Click += new System.EventHandler(this._privateChannelMenu_Leave_Click);
            // 
            // _characterMenu
            // 
            this._characterMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._characterMenu_TalkTo,
            this._characterMenu_Seperator,
            this._characterMenu_Invite,
            this._characterMenu_Remove});
            this._characterMenu.Name = "_characterMenu";
            this._characterMenu.Size = new System.Drawing.Size(123, 76);
            // 
            // _characterMenu_TalkTo
            // 
            this._characterMenu_TalkTo.Image = global::Vha.Chat.Properties.Resources.TalkBitmap;
            this._characterMenu_TalkTo.Name = "_characterMenu_TalkTo";
            this._characterMenu_TalkTo.Size = new System.Drawing.Size(122, 22);
            this._characterMenu_TalkTo.Text = "Talk To...";
            this._characterMenu_TalkTo.Click += new System.EventHandler(this._characterMenu_TalkTo_Click);
            // 
            // _characterMenu_Seperator
            // 
            this._characterMenu_Seperator.Name = "_characterMenu_Seperator";
            this._characterMenu_Seperator.Size = new System.Drawing.Size(119, 6);
            // 
            // _characterMenu_Invite
            // 
            this._characterMenu_Invite.Image = global::Vha.Chat.Properties.Resources.EnterBitmap;
            this._characterMenu_Invite.Name = "_characterMenu_Invite";
            this._characterMenu_Invite.Size = new System.Drawing.Size(122, 22);
            this._characterMenu_Invite.Text = "Invite";
            this._characterMenu_Invite.Click += new System.EventHandler(this._characterMenu_Invite_Click);
            // 
            // _characterMenu_Remove
            // 
            this._characterMenu_Remove.Image = global::Vha.Chat.Properties.Resources.DeleteBitmap;
            this._characterMenu_Remove.Name = "_characterMenu_Remove";
            this._characterMenu_Remove.Size = new System.Drawing.Size(122, 22);
            this._characterMenu_Remove.Text = "Remove";
            this._characterMenu_Remove.Click += new System.EventHandler(this._characterMenu_Remove_Click);
            // 
            // _guestsMenu
            // 
            this._guestsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._guestsMenu_Kick});
            this._guestsMenu.Name = "_guestsMenu";
            this._guestsMenu.Size = new System.Drawing.Size(97, 26);
            // 
            // _guestsMenu_Kick
            // 
            this._guestsMenu_Kick.Image = global::Vha.Chat.Properties.Resources.LeaveBitmap;
            this._guestsMenu_Kick.Name = "_guestsMenu_Kick";
            this._guestsMenu_Kick.Size = new System.Drawing.Size(96, 22);
            this._guestsMenu_Kick.Text = "Kick";
            this._guestsMenu_Kick.Click += new System.EventHandler(this._guestsMenu_Kick_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 529);
            this.Controls.Add(this._buttons);
            this.Controls.Add(this._target);
            this.Controls.Add(this._inputBox);
            this.Controls.Add(this._outputBox);
            this.Controls.Add(this._tree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this._buttons.ResumeLayout(false);
            this._buttons.PerformLayout();
            this._channelMenu.ResumeLayout(false);
            this._privateChannelMenu.ResumeLayout(false);
            this._characterMenu.ResumeLayout(false);
            this._guestsMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.WebBrowser _outputBox;
        private System.Windows.Forms.TextBox _inputBox;
        private System.Windows.Forms.ComboBox _target;
        private System.Windows.Forms.ToolStripButton _disconnect;
        private System.Windows.Forms.ToolStripButton _options;
        private System.Windows.Forms.ToolStripButton _about;
        private System.Windows.Forms.ToolStripButton _connect;
        private System.Windows.Forms.ToolStrip _buttons;
        private System.Windows.Forms.ImageList _icons;
        private System.Windows.Forms.ContextMenuStrip _channelMenu;
        private System.Windows.Forms.ToolStripMenuItem _channelMenu_Mute;
        private System.Windows.Forms.ToolStripMenuItem _channelMenu_Unmute;
        private System.Windows.Forms.ToolStripMenuItem _channelMenu_TalkTo;
        private System.Windows.Forms.ToolStripSeparator _channelMenu_Seperator;
        private System.Windows.Forms.ContextMenuStrip _privateChannelMenu;
        private System.Windows.Forms.ToolStripMenuItem _privateChannelMenu_TalkTo;
        private System.Windows.Forms.ToolStripSeparator _privateChannelMenu_Seperator;
        private System.Windows.Forms.ToolStripMenuItem _privateChannelMenu_Leave;
        private System.Windows.Forms.ContextMenuStrip _characterMenu;
        private System.Windows.Forms.ToolStripMenuItem _characterMenu_TalkTo;
        private System.Windows.Forms.ToolStripSeparator _characterMenu_Seperator;
        private System.Windows.Forms.ToolStripMenuItem _characterMenu_Remove;
        private System.Windows.Forms.ToolStripMenuItem _characterMenu_Invite;
        private System.Windows.Forms.ContextMenuStrip _guestsMenu;
        private System.Windows.Forms.ToolStripMenuItem _guestsMenu_Kick;

    }
}