namespace Vha.Chat.UI
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
            this._icon = new System.Windows.Forms.PictureBox();
            this._title = new System.Windows.Forms.Label();
            this._cancel = new System.Windows.Forms.Button();
            this._reset = new System.Windows.Forms.Button();
            this._save = new System.Windows.Forms.Button();
            this._chatOptions = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this._ignoreMethod = new System.Windows.Forms.ComboBox();
            this._panelPosition = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._textStyle = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._maximumTexts = new System.Windows.Forms.NumericUpDown();
            this._maximumMessages = new System.Windows.Forms.NumericUpDown();
            this._maximumHistory = new System.Windows.Forms.NumericUpDown();
            this._proxyOptions = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._proxyPassword = new System.Windows.Forms.TextBox();
            this._proxyPort = new System.Windows.Forms.NumericUpDown();
            this._proxyUsername = new System.Windows.Forms.TextBox();
            this._proxyAddress = new System.Windows.Forms.TextBox();
            this._proxyType = new System.Windows.Forms.ComboBox();
            this._miscOptions = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this._donateVisible = new System.Windows.Forms.CheckBox();
            this._topBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).BeginInit();
            this._chatOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._maximumTexts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumHistory)).BeginInit();
            this._proxyOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._proxyPort)).BeginInit();
            this._miscOptions.SuspendLayout();
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
            // _icon
            // 
            this._icon.BackgroundImage = global::Vha.Chat.Properties.Resources.OptionsBigBitmap;
            this._icon.Location = new System.Drawing.Point(17, 6);
            this._icon.Name = "_icon";
            this._icon.Size = new System.Drawing.Size(32, 32);
            this._icon.TabIndex = 5;
            this._icon.TabStop = false;
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
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(246, 447);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 12;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _reset
            // 
            this._reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._reset.Location = new System.Drawing.Point(12, 447);
            this._reset.Name = "_reset";
            this._reset.Size = new System.Drawing.Size(75, 23);
            this._reset.TabIndex = 14;
            this._reset.Text = "Reset";
            this._reset.UseVisualStyleBackColor = true;
            this._reset.Click += new System.EventHandler(this._reset_Click);
            // 
            // _save
            // 
            this._save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._save.Location = new System.Drawing.Point(327, 447);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(75, 23);
            this._save.TabIndex = 13;
            this._save.Text = "Save";
            this._save.UseVisualStyleBackColor = true;
            this._save.Click += new System.EventHandler(this._save_Click);
            // 
            // _chatOptions
            // 
            this._chatOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._chatOptions.Controls.Add(this.label6);
            this._chatOptions.Controls.Add(this._ignoreMethod);
            this._chatOptions.Controls.Add(this._panelPosition);
            this._chatOptions.Controls.Add(this.label1);
            this._chatOptions.Controls.Add(this._textStyle);
            this._chatOptions.Controls.Add(this.label5);
            this._chatOptions.Controls.Add(this.label4);
            this._chatOptions.Controls.Add(this.label3);
            this._chatOptions.Controls.Add(this.label2);
            this._chatOptions.Controls.Add(this._maximumTexts);
            this._chatOptions.Controls.Add(this._maximumMessages);
            this._chatOptions.Controls.Add(this._maximumHistory);
            this._chatOptions.Location = new System.Drawing.Point(12, 59);
            this._chatOptions.Name = "_chatOptions";
            this._chatOptions.Size = new System.Drawing.Size(390, 183);
            this._chatOptions.TabIndex = 13;
            this._chatOptions.TabStop = false;
            this._chatOptions.Text = "Chat";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Ignore list method";
            // 
            // _ignoreMethod
            // 
            this._ignoreMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ignoreMethod.FormattingEnabled = true;
            this._ignoreMethod.Location = new System.Drawing.Point(189, 152);
            this._ignoreMethod.Name = "_ignoreMethod";
            this._ignoreMethod.Size = new System.Drawing.Size(192, 21);
            this._ignoreMethod.TabIndex = 6;
            // 
            // _panelPosition
            // 
            this._panelPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._panelPosition.FormattingEnabled = true;
            this._panelPosition.Location = new System.Drawing.Point(189, 125);
            this._panelPosition.Name = "_panelPosition";
            this._panelPosition.Size = new System.Drawing.Size(192, 21);
            this._panelPosition.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Side panel position";
            // 
            // _textStyle
            // 
            this._textStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._textStyle.FormattingEnabled = true;
            this._textStyle.Location = new System.Drawing.Point(189, 20);
            this._textStyle.Name = "_textStyle";
            this._textStyle.Size = new System.Drawing.Size(192, 21);
            this._textStyle.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Info window history";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Lines of scrollback history";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Script and bot colors";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lines of input history";
            // 
            // _maximumTexts
            // 
            this._maximumTexts.Location = new System.Drawing.Point(189, 99);
            this._maximumTexts.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this._maximumTexts.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._maximumTexts.Name = "_maximumTexts";
            this._maximumTexts.Size = new System.Drawing.Size(62, 20);
            this._maximumTexts.TabIndex = 4;
            this._maximumTexts.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // _maximumMessages
            // 
            this._maximumMessages.Location = new System.Drawing.Point(189, 73);
            this._maximumMessages.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._maximumMessages.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._maximumMessages.Name = "_maximumMessages";
            this._maximumMessages.Size = new System.Drawing.Size(62, 20);
            this._maximumMessages.TabIndex = 3;
            this._maximumMessages.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // _maximumHistory
            // 
            this._maximumHistory.Location = new System.Drawing.Point(189, 47);
            this._maximumHistory.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this._maximumHistory.Name = "_maximumHistory";
            this._maximumHistory.Size = new System.Drawing.Size(62, 20);
            this._maximumHistory.TabIndex = 2;
            // 
            // _proxyOptions
            // 
            this._proxyOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._proxyOptions.Controls.Add(this.label10);
            this._proxyOptions.Controls.Add(this.label9);
            this._proxyOptions.Controls.Add(this.label8);
            this._proxyOptions.Controls.Add(this.label7);
            this._proxyOptions.Controls.Add(this._proxyPassword);
            this._proxyOptions.Controls.Add(this._proxyPort);
            this._proxyOptions.Controls.Add(this._proxyUsername);
            this._proxyOptions.Controls.Add(this._proxyAddress);
            this._proxyOptions.Controls.Add(this._proxyType);
            this._proxyOptions.Location = new System.Drawing.Point(12, 248);
            this._proxyOptions.Name = "_proxyOptions";
            this._proxyOptions.Size = new System.Drawing.Size(390, 128);
            this._proxyOptions.TabIndex = 14;
            this._proxyOptions.TabStop = false;
            this._proxyOptions.Text = "Proxy";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Password";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Username";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Mode";
            // 
            // _proxyPassword
            // 
            this._proxyPassword.Location = new System.Drawing.Point(189, 99);
            this._proxyPassword.Name = "_proxyPassword";
            this._proxyPassword.PasswordChar = '*';
            this._proxyPassword.Size = new System.Drawing.Size(192, 20);
            this._proxyPassword.TabIndex = 11;
            // 
            // _proxyPort
            // 
            this._proxyPort.Location = new System.Drawing.Point(319, 47);
            this._proxyPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._proxyPort.Name = "_proxyPort";
            this._proxyPort.Size = new System.Drawing.Size(62, 20);
            this._proxyPort.TabIndex = 9;
            this._proxyPort.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // _proxyUsername
            // 
            this._proxyUsername.Location = new System.Drawing.Point(189, 73);
            this._proxyUsername.Name = "_proxyUsername";
            this._proxyUsername.Size = new System.Drawing.Size(192, 20);
            this._proxyUsername.TabIndex = 10;
            // 
            // _proxyAddress
            // 
            this._proxyAddress.Location = new System.Drawing.Point(189, 47);
            this._proxyAddress.Name = "_proxyAddress";
            this._proxyAddress.Size = new System.Drawing.Size(124, 20);
            this._proxyAddress.TabIndex = 8;
            // 
            // _proxyType
            // 
            this._proxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._proxyType.FormattingEnabled = true;
            this._proxyType.Location = new System.Drawing.Point(189, 19);
            this._proxyType.Name = "_proxyType";
            this._proxyType.Size = new System.Drawing.Size(192, 21);
            this._proxyType.TabIndex = 7;
            this._proxyType.SelectedIndexChanged += new System.EventHandler(this._proxyType_SelectedIndexChanged);
            // 
            // _miscOptions
            // 
            this._miscOptions.Controls.Add(this._donateVisible);
            this._miscOptions.Controls.Add(this.label11);
            this._miscOptions.Location = new System.Drawing.Point(13, 383);
            this._miscOptions.Name = "_miscOptions";
            this._miscOptions.Size = new System.Drawing.Size(389, 45);
            this._miscOptions.TabIndex = 15;
            this._miscOptions.TabStop = false;
            this._miscOptions.Text = "Misc";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Show donate button";
            // 
            // _donateVisible
            // 
            this._donateVisible.AutoSize = true;
            this._donateVisible.Location = new System.Drawing.Point(188, 19);
            this._donateVisible.Name = "_donateVisible";
            this._donateVisible.Size = new System.Drawing.Size(15, 14);
            this._donateVisible.TabIndex = 1;
            this._donateVisible.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 482);
            this.Controls.Add(this._miscOptions);
            this.Controls.Add(this._proxyOptions);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionsForm_FormClosed);
            this._topBackground.ResumeLayout(false);
            this._topBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).EndInit();
            this._chatOptions.ResumeLayout(false);
            this._chatOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._maximumTexts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._maximumHistory)).EndInit();
            this._proxyOptions.ResumeLayout(false);
            this._proxyOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._proxyPort)).EndInit();
            this._miscOptions.ResumeLayout(false);
            this._miscOptions.PerformLayout();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _maximumTexts;
        private System.Windows.Forms.NumericUpDown _maximumMessages;
        private System.Windows.Forms.NumericUpDown _maximumHistory;
        private System.Windows.Forms.GroupBox _proxyOptions;
        private System.Windows.Forms.PictureBox _icon;
        private System.Windows.Forms.ComboBox _panelPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _textStyle;
        private System.Windows.Forms.ComboBox _ignoreMethod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _proxyPassword;
        private System.Windows.Forms.NumericUpDown _proxyPort;
        private System.Windows.Forms.TextBox _proxyUsername;
        private System.Windows.Forms.TextBox _proxyAddress;
        private System.Windows.Forms.ComboBox _proxyType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox _miscOptions;
        private System.Windows.Forms.CheckBox _donateVisible;
        private System.Windows.Forms.Label label11;
    }
}