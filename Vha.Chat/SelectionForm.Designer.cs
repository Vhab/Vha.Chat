namespace Vha.Chat
{
    partial class SelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionForm));
            this._characters = new System.Windows.Forms.ComboBox();
            this.lblCharacter = new System.Windows.Forms.Label();
            this._cancel = new System.Windows.Forms.Button();
            this._select = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _characters
            // 
            this._characters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._characters.FormattingEnabled = true;
            this._characters.Location = new System.Drawing.Point(71, 13);
            this._characters.Name = "_characters";
            this._characters.Size = new System.Drawing.Size(201, 21);
            this._characters.TabIndex = 0;
            // 
            // lblCharacter
            // 
            this.lblCharacter.AutoSize = true;
            this.lblCharacter.Location = new System.Drawing.Point(12, 16);
            this.lblCharacter.Name = "lblCharacter";
            this.lblCharacter.Size = new System.Drawing.Size(53, 13);
            this.lblCharacter.TabIndex = 1;
            this.lblCharacter.Text = "Character";
            // 
            // _cancel
            // 
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(145, 40);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(127, 23);
            this._cancel.TabIndex = 9;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _select
            // 
            this._select.Location = new System.Drawing.Point(12, 40);
            this._select.Name = "_select";
            this._select.Size = new System.Drawing.Size(127, 23);
            this._select.TabIndex = 8;
            this._select.Text = "Select";
            this._select.UseVisualStyleBackColor = true;
            this._select.Click += new System.EventHandler(this._select_Click);
            // 
            // SelectionForm
            // 
            this.AcceptButton = this._select;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(284, 75);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._select);
            this.Controls.Add(this.lblCharacter);
            this.Controls.Add(this._characters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: Character Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _characters;
        private System.Windows.Forms.Label lblCharacter;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _select;
    }
}