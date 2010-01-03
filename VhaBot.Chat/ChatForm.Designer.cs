namespace VhaBot.Chat
{
    partial class ChatForm
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
            this.Tree = new System.Windows.Forms.TreeView();
            this.Output = new System.Windows.Forms.WebBrowser();
            this.Input = new System.Windows.Forms.TextBox();
            this.Channels = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Tree
            // 
            this.Tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Tree.Location = new System.Drawing.Point(571, 6);
            this.Tree.Name = "Tree";
            this.Tree.ShowPlusMinus = false;
            this.Tree.ShowRootLines = false;
            this.Tree.Size = new System.Drawing.Size(157, 452);
            this.Tree.TabIndex = 0;
            this.Tree.DoubleClick += new System.EventHandler(this.Tree_DoubleClick);
            // 
            // Output
            // 
            this.Output.AllowWebBrowserDrop = false;
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.IsWebBrowserContextMenuEnabled = false;
            this.Output.Location = new System.Drawing.Point(6, 6);
            this.Output.MinimumSize = new System.Drawing.Size(20, 20);
            this.Output.Name = "Output";
            this.Output.ScriptErrorsSuppressed = true;
            this.Output.Size = new System.Drawing.Size(559, 425);
            this.Output.TabIndex = 1;
            this.Output.Url = new System.Uri("", System.UriKind.Relative);
            this.Output.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Output_Navigating);
            this.Output.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Output_DocumentCompleted);
            // 
            // Input
            // 
            this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Input.Location = new System.Drawing.Point(154, 437);
            this.Input.Multiline = true;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(411, 21);
            this.Input.TabIndex = 2;
            this.Input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Input_KeyPress);
            // 
            // Channels
            // 
            this.Channels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Channels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Channels.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Channels.FormattingEnabled = true;
            this.Channels.Location = new System.Drawing.Point(6, 437);
            this.Channels.Name = "Channels";
            this.Channels.Size = new System.Drawing.Size(142, 21);
            this.Channels.TabIndex = 4;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 464);
            this.Controls.Add(this.Channels);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Tree);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VhaBot.Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView Tree;
        private System.Windows.Forms.WebBrowser Output;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.ComboBox Channels;

    }
}