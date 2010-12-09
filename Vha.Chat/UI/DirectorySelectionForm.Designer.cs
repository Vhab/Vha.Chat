namespace Vha.Chat.UI
{
    partial class DirectorySelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectorySelectionForm));
            this.label1 = new System.Windows.Forms.Label();
            this._defaultDirectory = new System.Windows.Forms.Button();
            this._appData = new System.Windows.Forms.Button();
            this._readOnly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thank you for choosing Vha.Chat.\r\nWhere would you like to store your preferences?" +
                "";
            // 
            // _defaultDirectory
            // 
            this._defaultDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._defaultDirectory.Location = new System.Drawing.Point(13, 53);
            this._defaultDirectory.Name = "_defaultDirectory";
            this._defaultDirectory.Size = new System.Drawing.Size(234, 23);
            this._defaultDirectory.TabIndex = 1;
            this._defaultDirectory.Text = "Default directory";
            this._defaultDirectory.UseVisualStyleBackColor = true;
            this._defaultDirectory.Click += new System.EventHandler(this._defaultDirectory_Click);
            // 
            // _appData
            // 
            this._appData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._appData.Location = new System.Drawing.Point(13, 83);
            this._appData.Name = "_appData";
            this._appData.Size = new System.Drawing.Size(234, 23);
            this._appData.TabIndex = 2;
            this._appData.Text = "Application data directory";
            this._appData.UseVisualStyleBackColor = true;
            this._appData.Click += new System.EventHandler(this._appData_Click);
            // 
            // _readOnly
            // 
            this._readOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._readOnly.Location = new System.Drawing.Point(13, 112);
            this._readOnly.Name = "_readOnly";
            this._readOnly.Size = new System.Drawing.Size(234, 23);
            this._readOnly.TabIndex = 3;
            this._readOnly.Text = "Nowhere";
            this._readOnly.UseVisualStyleBackColor = true;
            this._readOnly.Click += new System.EventHandler(this._readOnly_Click);
            // 
            // DirectorySelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 147);
            this.Controls.Add(this._readOnly);
            this.Controls.Add(this._appData);
            this.Controls.Add(this._defaultDirectory);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DirectorySelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vha.Chat :: Welcome";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _defaultDirectory;
        private System.Windows.Forms.Button _appData;
        private System.Windows.Forms.Button _readOnly;
    }
}