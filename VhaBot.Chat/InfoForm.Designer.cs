namespace VhaBot.Chat
{
    partial class InfoForm
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
            this.Info = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // Info
            // 
            this.Info.AllowWebBrowserDrop = false;
            this.Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Info.IsWebBrowserContextMenuEnabled = false;
            this.Info.Location = new System.Drawing.Point(0, 0);
            this.Info.MinimumSize = new System.Drawing.Size(20, 20);
            this.Info.Name = "Info";
            this.Info.ScriptErrorsSuppressed = true;
            this.Info.Size = new System.Drawing.Size(333, 321);
            this.Info.TabIndex = 0;
            this.Info.Url = new System.Uri("", System.UriKind.Relative);
            this.Info.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Info_Navigating);
            this.Info.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Info_DocumentCompleted);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(333, 321);
            this.Controls.Add(this.Info);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VhaBot.Chat :: Popup";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser Info;
    }
}