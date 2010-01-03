namespace VhaBot.Chat
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
            this.Title = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Copyright = new System.Windows.Forms.Label();
            this.License = new System.Windows.Forms.TextBox();
            this.Seperator = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.BackColor = System.Drawing.SystemColors.Window;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(12, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(133, 25);
            this.Title.TabIndex = 0;
            this.Title.Text = "VhaBot.Chat";
            // 
            // Version
            // 
            this.Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Version.BackColor = System.Drawing.SystemColors.Window;
            this.Version.Location = new System.Drawing.Point(245, 37);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(122, 13);
            this.Version.TabIndex = 1;
            this.Version.Text = "Version X.X.X";
            this.Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Copyright
            // 
            this.Copyright.AutoSize = true;
            this.Copyright.BackColor = System.Drawing.SystemColors.Window;
            this.Copyright.Location = new System.Drawing.Point(12, 37);
            this.Copyright.Name = "Copyright";
            this.Copyright.Size = new System.Drawing.Size(203, 13);
            this.Copyright.TabIndex = 3;
            this.Copyright.Text = "Copyright © 2009 Remco van Oosterhout";
            // 
            // License
            // 
            this.License.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.License.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.License.Location = new System.Drawing.Point(12, 74);
            this.License.Multiline = true;
            this.License.Name = "License";
            this.License.ReadOnly = true;
            this.License.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.License.Size = new System.Drawing.Size(355, 126);
            this.License.TabIndex = 4;
            this.License.Text = resources.GetString("License.Text");
            // 
            // Seperator
            // 
            this.Seperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Seperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Seperator.Location = new System.Drawing.Point(-5, 60);
            this.Seperator.Name = "Seperator";
            this.Seperator.Size = new System.Drawing.Size(395, 2);
            this.Seperator.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Location = new System.Drawing.Point(-5, -5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 65);
            this.panel1.TabIndex = 6;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 212);
            this.Controls.Add(this.Seperator);
            this.Controls.Add(this.License);
            this.Controls.Add(this.Copyright);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VhaBot.Chat :: About";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label Copyright;
        private System.Windows.Forms.TextBox License;
        private System.Windows.Forms.Label Seperator;
        private System.Windows.Forms.Panel panel1;

    }
}