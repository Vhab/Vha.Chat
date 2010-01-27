namespace Vha.Chat
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this._title = new System.Windows.Forms.Label();
            this._version = new System.Windows.Forms.Label();
            this._copyright = new System.Windows.Forms.Label();
            this._license = new System.Windows.Forms.TextBox();
            this._seperator = new System.Windows.Forms.Label();
            this._topBackground = new System.Windows.Forms.Panel();
            this._icon = new System.Windows.Forms.PictureBox();
            this._topBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).BeginInit();
            this.SuspendLayout();
            // 
            // _title
            // 
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.SystemColors.Window;
            this._title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._title.Location = new System.Drawing.Point(55, 14);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(101, 25);
            this._title.TabIndex = 0;
            this._title.Text = "Vha.Chat";
            // 
            // _version
            // 
            this._version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._version.BackColor = System.Drawing.SystemColors.Window;
            this._version.Location = new System.Drawing.Point(263, 39);
            this._version.Name = "_version";
            this._version.Size = new System.Drawing.Size(122, 13);
            this._version.TabIndex = 1;
            this._version.Text = "Version X.X.X";
            this._version.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _copyright
            // 
            this._copyright.AutoSize = true;
            this._copyright.BackColor = System.Drawing.SystemColors.Window;
            this._copyright.Location = new System.Drawing.Point(57, 39);
            this._copyright.Name = "_copyright";
            this._copyright.Size = new System.Drawing.Size(230, 13);
            this._copyright.TabIndex = 3;
            this._copyright.Text = "Copyright © 2009-2010 Remco van Oosterhout";
            // 
            // _license
            // 
            this._license.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._license.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._license.Location = new System.Drawing.Point(12, 74);
            this._license.Multiline = true;
            this._license.Name = "_license";
            this._license.ReadOnly = true;
            this._license.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._license.Size = new System.Drawing.Size(368, 126);
            this._license.TabIndex = 4;
            this._license.Text = resources.GetString("_license.Text");
            // 
            // _seperator
            // 
            this._seperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._seperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._seperator.Location = new System.Drawing.Point(-5, 58);
            this._seperator.Name = "_seperator";
            this._seperator.Size = new System.Drawing.Size(408, 2);
            this._seperator.TabIndex = 5;
            // 
            // _topBackground
            // 
            this._topBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._topBackground.BackColor = System.Drawing.SystemColors.Window;
            this._topBackground.Controls.Add(this._icon);
            this._topBackground.Controls.Add(this._copyright);
            this._topBackground.Controls.Add(this._title);
            this._topBackground.Controls.Add(this._version);
            this._topBackground.Location = new System.Drawing.Point(-5, -5);
            this._topBackground.Name = "_topBackground";
            this._topBackground.Size = new System.Drawing.Size(408, 63);
            this._topBackground.TabIndex = 6;
            // 
            // _icon
            // 
            this._icon.BackgroundImage = global::Vha.Chat.Properties.Resources.ChatBigBitmap;
            this._icon.Location = new System.Drawing.Point(17, 19);
            this._icon.Name = "_icon";
            this._icon.Size = new System.Drawing.Size(32, 32);
            this._icon.TabIndex = 4;
            this._icon.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 212);
            this.Controls.Add(this._seperator);
            this.Controls.Add(this._license);
            this.Controls.Add(this._topBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: About";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this._topBackground.ResumeLayout(false);
            this._topBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _version;
        private System.Windows.Forms.Label _copyright;
        private System.Windows.Forms.TextBox _license;
        private System.Windows.Forms.Label _seperator;
        private System.Windows.Forms.Panel _topBackground;
        private System.Windows.Forms.PictureBox _icon;

    }
}