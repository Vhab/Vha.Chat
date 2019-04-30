namespace Vha.Chat.UI
{
    partial class IgnoresForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IgnoresForm));
            this._characters = new System.Windows.Forms.CheckedListBox();
            this._remove = new System.Windows.Forms.Button();
            this._selectAll = new System.Windows.Forms.Button();
            this._selectNone = new System.Windows.Forms.Button();
            this._character = new System.Windows.Forms.TextBox();
            this._add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _characters
            // 
            this._characters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._characters.CheckOnClick = true;
            this._characters.FormattingEnabled = true;
            this._characters.IntegralHeight = false;
            this._characters.Location = new System.Drawing.Point(12, 67);
            this._characters.Name = "_characters";
            this._characters.Size = new System.Drawing.Size(216, 205);
            this._characters.TabIndex = 0;
            // 
            // _remove
            // 
            this._remove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._remove.Location = new System.Drawing.Point(12, 307);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(216, 23);
            this._remove.TabIndex = 1;
            this._remove.Text = "Remove Selected";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _selectAll
            // 
            this._selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._selectAll.Location = new System.Drawing.Point(12, 278);
            this._selectAll.Name = "_selectAll";
            this._selectAll.Size = new System.Drawing.Size(105, 23);
            this._selectAll.TabIndex = 2;
            this._selectAll.Text = "Select All";
            this._selectAll.UseVisualStyleBackColor = true;
            this._selectAll.Click += new System.EventHandler(this._selectAll_Click);
            // 
            // _selectNone
            // 
            this._selectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._selectNone.Location = new System.Drawing.Point(123, 278);
            this._selectNone.Name = "_selectNone";
            this._selectNone.Size = new System.Drawing.Size(105, 23);
            this._selectNone.TabIndex = 3;
            this._selectNone.Text = "Select None";
            this._selectNone.UseVisualStyleBackColor = true;
            this._selectNone.Click += new System.EventHandler(this._selectNone_Click);
            // 
            // _character
            // 
            this._character.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._character.Location = new System.Drawing.Point(12, 12);
            this._character.MaxLength = 30;
            this._character.Name = "_character";
            this._character.Size = new System.Drawing.Size(216, 20);
            this._character.TabIndex = 4;
            // 
            // _add
            // 
            this._add.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._add.Location = new System.Drawing.Point(12, 38);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(216, 23);
            this._add.TabIndex = 5;
            this._add.Text = "Add to ignore list";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // IgnoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 342);
            this.Controls.Add(this._add);
            this.Controls.Add(this._character);
            this.Controls.Add(this._selectNone);
            this.Controls.Add(this._selectAll);
            this.Controls.Add(this._remove);
            this.Controls.Add(this._characters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(256, 2048);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 300);
            this.Name = "IgnoresForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: Ignore List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox _characters;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.Button _selectAll;
        private System.Windows.Forms.Button _selectNone;
        private System.Windows.Forms.TextBox _character;
        private System.Windows.Forms.Button _add;
    }
}