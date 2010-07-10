namespace Vha.Chat.UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this._info = new Vha.Chat.UI.AomlBox();
            this.SuspendLayout();
            // 
            // _info
            // 
            this._info.AllowWebBrowserDrop = false;
            this._info.BackgroundColor = System.Drawing.Color.White;
            this._info.Dock = System.Windows.Forms.DockStyle.Fill;
            this._info.ForegroundColor = System.Drawing.Color.Black;
            this._info.IsWebBrowserContextMenuEnabled = false;
            this._info.Location = new System.Drawing.Point(0, 0);
            this._info.MinimumSize = new System.Drawing.Size(20, 20);
            this._info.Name = "_info";
            this._info.ScriptErrorsSuppressed = true;
            this._info.Size = new System.Drawing.Size(374, 374);
            this._info.TabIndex = 0;
            this._info.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(374, 374);
            this.Controls.Add(this._info);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(170, 200);
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: Info";
            this.ResumeLayout(false);

        }

        #endregion

        private Vha.Chat.UI.AomlBox _info;
    }
}