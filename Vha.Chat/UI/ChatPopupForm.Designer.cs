namespace Vha.Chat.UI
{
    partial class ChatPopupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatPopupForm));
            this._inputBox = new System.Windows.Forms.TextBox();
            this._outputBox = new Vha.Chat.UI.ChatOutputBox();
            this.SuspendLayout();
            // 
            // _inputBox
            // 
            this._inputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._inputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._inputBox.Location = new System.Drawing.Point(5, 369);
            this._inputBox.Name = "_inputBox";
            this._inputBox.Size = new System.Drawing.Size(454, 20);
            this._inputBox.TabIndex = 0;
            this._inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._inputBox_KeyDown);
            this._inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._inputBox_KeyPress);
            // 
            // _outputBox
            // 
            this._outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._outputBox.BackgroundColor = System.Drawing.Color.White;
            this._outputBox.ForegroundColor = System.Drawing.Color.Black;
            this._outputBox.Location = new System.Drawing.Point(5, 5);
            this._outputBox.MaximumLines = 0;
            this._outputBox.MaximumTexts = 50;
            this._outputBox.MinimumSize = new System.Drawing.Size(20, 20);
            this._outputBox.Name = "_outputBox";
            this._outputBox.Size = new System.Drawing.Size(454, 358);
            this._outputBox.TabIndex = 1;
            this._outputBox.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // ChatPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 394);
            this.Controls.Add(this._outputBox);
            this.Controls.Add(this._inputBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 250);
            this.Name = "ChatPopupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatPopupForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _inputBox;
        private Vha.Chat.UI.ChatOutputBox _outputBox;
    }
}