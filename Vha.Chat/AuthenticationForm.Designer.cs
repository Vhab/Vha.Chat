namespace Vha.Chat
{
    partial class AuthenticationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthenticationForm));
            this._serverLabel = new System.Windows.Forms.Label();
            this._server = new System.Windows.Forms.ComboBox();
            this._accountLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._password = new System.Windows.Forms.TextBox();
            this._login = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this._account = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _serverLabel
            // 
            this._serverLabel.AutoSize = true;
            this._serverLabel.Location = new System.Drawing.Point(12, 15);
            this._serverLabel.Name = "_serverLabel";
            this._serverLabel.Size = new System.Drawing.Size(38, 13);
            this._serverLabel.TabIndex = 0;
            this._serverLabel.Text = "Server";
            // 
            // _server
            // 
            this._server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._server.FormattingEnabled = true;
            this._server.Location = new System.Drawing.Point(74, 12);
            this._server.Name = "_server";
            this._server.Size = new System.Drawing.Size(198, 21);
            this._server.TabIndex = 1;
            // 
            // _accountLabel
            // 
            this._accountLabel.AutoSize = true;
            this._accountLabel.Location = new System.Drawing.Point(12, 42);
            this._accountLabel.Name = "_accountLabel";
            this._accountLabel.Size = new System.Drawing.Size(47, 13);
            this._accountLabel.TabIndex = 3;
            this._accountLabel.Text = "Account";
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(12, 69);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(53, 13);
            this._passwordLabel.TabIndex = 4;
            this._passwordLabel.Text = "Password";
            // 
            // _password
            // 
            this._password.Location = new System.Drawing.Point(74, 66);
            this._password.Name = "_password";
            this._password.PasswordChar = '*';
            this._password.Size = new System.Drawing.Size(198, 20);
            this._password.TabIndex = 3;
            // 
            // _login
            // 
            this._login.Location = new System.Drawing.Point(12, 92);
            this._login.Name = "_login";
            this._login.Size = new System.Drawing.Size(127, 23);
            this._login.TabIndex = 4;
            this._login.Text = "Login";
            this._login.UseVisualStyleBackColor = true;
            this._login.Click += new System.EventHandler(this._login_Click);
            // 
            // _cancel
            // 
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(145, 92);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(127, 23);
            this._cancel.TabIndex = 5;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _account
            // 
            this._account.FormattingEnabled = true;
            this._account.Location = new System.Drawing.Point(74, 40);
            this._account.Name = "_account";
            this._account.Size = new System.Drawing.Size(198, 21);
            this._account.TabIndex = 2;
            // 
            // AuthenticationForm
            // 
            this.AcceptButton = this._login;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(284, 127);
            this.Controls.Add(this._account);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._login);
            this.Controls.Add(this._password);
            this.Controls.Add(this._passwordLabel);
            this.Controls.Add(this._accountLabel);
            this.Controls.Add(this._server);
            this.Controls.Add(this._serverLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthenticationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vha.Chat :: Authentication";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AuthenticationForm_FormClosed);
            this.Load += new System.EventHandler(this.AuthenticationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _serverLabel;
        private System.Windows.Forms.ComboBox _server;
        private System.Windows.Forms.Label _accountLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.TextBox _password;
        private System.Windows.Forms.Button _login;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.ComboBox _account;
    }
}

