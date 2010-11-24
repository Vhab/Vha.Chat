namespace Vha.Chat.UI
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
            this._outputBox = new Vha.Chat.UI.ChatOutputBox();
            this._inputBox = new System.Windows.Forms.TextBox();
            this._target = new System.Windows.Forms.ComboBox();
            this._connect = new System.Windows.Forms.ToolStripButton();
            this._disconnect = new System.Windows.Forms.ToolStripButton();
            this._options = new System.Windows.Forms.ToolStripButton();
            this._about = new System.Windows.Forms.ToolStripButton();
            this._buttons = new System.Windows.Forms.ToolStrip();
            this._ignores = new System.Windows.Forms.ToolStripButton();
            this._container = new System.Windows.Forms.SplitContainer();
            this._buttons.SuspendLayout();
            this._container.Panel1.SuspendLayout();
            this._container.Panel2.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tree.ImageIndex = 0;
            this._tree.ImageList = this._icons;
            this._tree.Location = new System.Drawing.Point(2, 2);
            this._tree.Name = "_tree";
            this._tree.SelectedImageIndex = 0;
            this._tree.Size = new System.Drawing.Size(174, 518);
            this._tree.TabIndex = 3;
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
            this._outputBox.BackgroundColor = System.Drawing.Color.White;
            this._outputBox.Context = null;
            this._outputBox.ForegroundColor = System.Drawing.Color.Black;
            this._outputBox.IsWebBrowserContextMenuEnabled = false;
            this._outputBox.Location = new System.Drawing.Point(2, 2);
            this._outputBox.MaximumLines = 0;
            this._outputBox.MaximumTexts = 50;
            this._outputBox.MinimumSize = new System.Drawing.Size(20, 20);
            this._outputBox.Name = "_outputBox";
            this._outputBox.ScriptErrorsSuppressed = true;
            this._outputBox.Size = new System.Drawing.Size(589, 491);
            this._outputBox.TabIndex = 4;
            this._outputBox.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // _inputBox
            // 
            this._inputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._inputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._inputBox.Location = new System.Drawing.Point(150, 499);
            this._inputBox.Multiline = true;
            this._inputBox.Name = "_inputBox";
            this._inputBox.Size = new System.Drawing.Size(441, 21);
            this._inputBox.TabIndex = 0;
            this._inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._inputBox_KeyDown);
            this._inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._inputBox_KeyPress);
            // 
            // _target
            // 
            this._target.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._target.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._target.FormattingEnabled = true;
            this._target.Location = new System.Drawing.Point(2, 499);
            this._target.Name = "_target";
            this._target.Size = new System.Drawing.Size(142, 21);
            this._target.TabIndex = 1;
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
            this._ignores,
            this._options,
            this._about});
            this._buttons.Location = new System.Drawing.Point(0, 0);
            this._buttons.Name = "_buttons";
            this._buttons.Padding = new System.Windows.Forms.Padding(6);
            this._buttons.Size = new System.Drawing.Size(782, 32);
            this._buttons.TabIndex = 2;
            // 
            // _ignores
            // 
            this._ignores.Image = global::Vha.Chat.Properties.Resources.IgnoresBitmap;
            this._ignores.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._ignores.Margin = new System.Windows.Forms.Padding(0);
            this._ignores.Name = "_ignores";
            this._ignores.Size = new System.Drawing.Size(82, 20);
            this._ignores.Text = "Ignore List";
            this._ignores.Click += new System.EventHandler(this._ignores_Click);
            // 
            // _container
            // 
            this._container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._container.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._container.Location = new System.Drawing.Point(5, 37);
            this._container.Margin = new System.Windows.Forms.Padding(5);
            this._container.Name = "_container";
            // 
            // _container.Panel1
            // 
            this._container.Panel1.Controls.Add(this._target);
            this._container.Panel1.Controls.Add(this._inputBox);
            this._container.Panel1.Controls.Add(this._outputBox);
            this._container.Panel1.Padding = new System.Windows.Forms.Padding(2);
            // 
            // _container.Panel2
            // 
            this._container.Panel2.Controls.Add(this._tree);
            this._container.Panel2.Padding = new System.Windows.Forms.Padding(2);
            this._container.Size = new System.Drawing.Size(774, 522);
            this._container.SplitterDistance = 593;
            this._container.SplitterWidth = 3;
            this._container.TabIndex = 5;
            this._container.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this._container_SplitterMoved);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this._container);
            this.Controls.Add(this._buttons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 270);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
            this._buttons.ResumeLayout(false);
            this._buttons.PerformLayout();
            this._container.Panel1.ResumeLayout(false);
            this._container.Panel1.PerformLayout();
            this._container.Panel2.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private Vha.Chat.UI.ChatOutputBox _outputBox;
        private System.Windows.Forms.TextBox _inputBox;
        private System.Windows.Forms.ComboBox _target;
        private System.Windows.Forms.ToolStripButton _disconnect;
        private System.Windows.Forms.ToolStripButton _options;
        private System.Windows.Forms.ToolStripButton _about;
        private System.Windows.Forms.ToolStripButton _connect;
        private System.Windows.Forms.ToolStrip _buttons;
        private System.Windows.Forms.ImageList _icons;
        private System.Windows.Forms.SplitContainer _container;
        private System.Windows.Forms.ToolStripButton _ignores;

    }
}