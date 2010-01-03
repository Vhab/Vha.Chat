namespace VhaBot.Chat
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
            this.CharactersList = new System.Windows.Forms.ComboBox();
            this.CharacterLabel = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.Select = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CharactersList
            // 
            this.CharactersList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CharactersList.FormattingEnabled = true;
            this.CharactersList.Location = new System.Drawing.Point(71, 13);
            this.CharactersList.Name = "CharactersList";
            this.CharactersList.Size = new System.Drawing.Size(201, 21);
            this.CharactersList.TabIndex = 0;
            // 
            // CharacterLabel
            // 
            this.CharacterLabel.AutoSize = true;
            this.CharacterLabel.Location = new System.Drawing.Point(12, 16);
            this.CharacterLabel.Name = "CharacterLabel";
            this.CharacterLabel.Size = new System.Drawing.Size(53, 13);
            this.CharacterLabel.TabIndex = 1;
            this.CharacterLabel.Text = "Character";
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(145, 40);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(127, 23);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Select
            // 
            this.Select.Location = new System.Drawing.Point(12, 40);
            this.Select.Name = "Select";
            this.Select.Size = new System.Drawing.Size(127, 23);
            this.Select.TabIndex = 8;
            this.Select.Text = "Select";
            this.Select.UseVisualStyleBackColor = true;
            this.Select.Click += new System.EventHandler(this.Select_Click);
            // 
            // SelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 75);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Select);
            this.Controls.Add(this.CharacterLabel);
            this.Controls.Add(this.CharactersList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VhaBot.Chat :: Character Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CharactersList;
        private System.Windows.Forms.Label CharacterLabel;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Select;
    }
}