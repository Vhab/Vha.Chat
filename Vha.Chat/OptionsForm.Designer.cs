namespace Vha.Chat
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this._seperator = new System.Windows.Forms.Label();
            this._topBackground = new System.Windows.Forms.Panel();
            this._title = new System.Windows.Forms.Label();
            this._cancel = new System.Windows.Forms.Button();
            this._reset = new System.Windows.Forms.Button();
            this._save = new System.Windows.Forms.Button();
            this._chatOptions = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this._accountOptions = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._icon = new System.Windows.Forms.PictureBox();
            this._topBackground.SuspendLayout();
            this._chatOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this._accountOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).BeginInit();
            this.SuspendLayout();
            // 
            // _seperator
            // 
            this._seperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._seperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._seperator.Location = new System.Drawing.Point(-2, 44);
            this._seperator.Name = "_seperator";
            this._seperator.Size = new System.Drawing.Size(421, 2);
            this._seperator.TabIndex = 7;
            // 
            // _topBackground
            // 
            this._topBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._topBackground.BackColor = System.Drawing.SystemColors.Window;
            this._topBackground.Controls.Add(this._icon);
            this._topBackground.Controls.Add(this._title);
            this._topBackground.Location = new System.Drawing.Point(0, 0);
            this._topBackground.Name = "_topBackground";
            this._topBackground.Size = new System.Drawing.Size(417, 44);
            this._topBackground.TabIndex = 8;
            // 
            // _title
            // 
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.SystemColors.Window;
            this._title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._title.Location = new System.Drawing.Point(55, 9);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(86, 25);
            this._title.TabIndex = 0;
            this._title.Text = "Options";
            // 
            // _cancel
            // 
            this._cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel.Location = new System.Drawing.Point(327, 276);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _reset
            // 
            this._reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._reset.Location = new System.Drawing.Point(246, 276);
            this._reset.Name = "_reset";
            this._reset.Size = new System.Drawing.Size(75, 23);
            this._reset.TabIndex = 1;
            this._reset.Text = "Reset";
            this._reset.UseVisualStyleBackColor = true;
            // 
            // _save
            // 
            this._save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._save.Location = new System.Drawing.Point(12, 276);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(75, 23);
            this._save.TabIndex = 0;
            this._save.Text = "Save";
            this._save.UseVisualStyleBackColor = true;
            this._save.Click += new System.EventHandler(this._save_Click);
            // 
            // _chatOptions
            // 
            this._chatOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._chatOptions.Controls.Add(this.label5);
            this._chatOptions.Controls.Add(this.label4);
            this._chatOptions.Controls.Add(this.label3);
            this._chatOptions.Controls.Add(this.label2);
            this._chatOptions.Controls.Add(this.numericUpDown3);
            this._chatOptions.Controls.Add(this.numericUpDown2);
            this._chatOptions.Controls.Add(this.numericUpDown1);
            this._chatOptions.Controls.Add(this.checkBox1);
            this._chatOptions.Location = new System.Drawing.Point(12, 59);
            this._chatOptions.Name = "_chatOptions";
            this._chatOptions.Size = new System.Drawing.Size(390, 125);
            this._chatOptions.TabIndex = 13;
            this._chatOptions.TabStop = false;
            this._chatOptions.Text = "Chat";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Info window history";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Lines of scrollback history";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Invert script and bot colors";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lines of input history";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(233, 97);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown3.TabIndex = 6;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(233, 71);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown2.TabIndex = 5;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(233, 45);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown1.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(233, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // _accountOptions
            // 
            this._accountOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._accountOptions.Controls.Add(this.checkBox3);
            this._accountOptions.Controls.Add(this.checkBox2);
            this._accountOptions.Controls.Add(this.label7);
            this._accountOptions.Controls.Add(this.label6);
            this._accountOptions.Location = new System.Drawing.Point(12, 190);
            this._accountOptions.Name = "_accountOptions";
            this._accountOptions.Size = new System.Drawing.Size(390, 73);
            this._accountOptions.TabIndex = 14;
            this._accountOptions.TabStop = false;
            this._accountOptions.Text = "Account";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(233, 47);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(233, 21);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Remember account";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Remember dimension";
            // 
            // _icon
            // 
            this._icon.BackgroundImage = global::Vha.Chat.Properties.Resources.OptionsBigBitmap;
            this._icon.Location = new System.Drawing.Point(17, 6);
            this._icon.Name = "_icon";
            this._icon.Size = new System.Drawing.Size(32, 32);
            this._icon.TabIndex = 5;
            this._icon.TabStop = false;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 311);
            this.Controls.Add(this._accountOptions);
            this.Controls.Add(this._chatOptions);
            this.Controls.Add(this._save);
            this.Controls.Add(this._reset);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._seperator);
            this.Controls.Add(this._topBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vha.Chat :: Options";
            this._topBackground.ResumeLayout(false);
            this._topBackground.PerformLayout();
            this._chatOptions.ResumeLayout(false);
            this._chatOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this._accountOptions.ResumeLayout(false);
            this._accountOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _seperator;
        private System.Windows.Forms.Panel _topBackground;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _reset;
        private System.Windows.Forms.Button _save;
        private System.Windows.Forms.GroupBox _chatOptions;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox _accountOptions;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox _icon;
    }
}