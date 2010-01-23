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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            this._tree = new System.Windows.Forms.TreeView();
            this._outputBox = new System.Windows.Forms.WebBrowser();
            this._inputBox = new System.Windows.Forms.TextBox();
            this._target = new System.Windows.Forms.ComboBox();
            this._connect = new System.Windows.Forms.ToolStripButton();
            this._disconnect = new System.Windows.Forms.ToolStripButton();
            this._options = new System.Windows.Forms.ToolStripButton();
            this._about = new System.Windows.Forms.ToolStripButton();
            this._buttons = new System.Windows.Forms.ToolStrip();
            this._buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tree.Location = new System.Drawing.Point(571, 35);
            this._tree.Name = "_tree";
            this._tree.Size = new System.Drawing.Size(157, 423);
            this._tree.TabIndex = 0;
            this._tree.DoubleClick += new System.EventHandler(this._tree_DoubleClick);
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
            this._outputBox.Size = new System.Drawing.Size(559, 396);
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
            this._inputBox.Location = new System.Drawing.Point(154, 437);
            this._inputBox.Multiline = true;
            this._inputBox.Name = "_inputBox";
            this._inputBox.Size = new System.Drawing.Size(411, 21);
            this._inputBox.TabIndex = 2;
            this._inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._inputBox_KeyPress);
            // 
            // _target
            // 
            this._target.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._target.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._target.FormattingEnabled = true;
            this._target.Location = new System.Drawing.Point(6, 437);
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
            this._buttons.Size = new System.Drawing.Size(734, 32);
            this._buttons.TabIndex = 5;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 464);
            this.Controls.Add(this._buttons);
            this.Controls.Add(this._target);
            this.Controls.Add(this._inputBox);
            this.Controls.Add(this._outputBox);
            this.Controls.Add(this._tree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this._buttons.ResumeLayout(false);
            this._buttons.PerformLayout();
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

    }
}