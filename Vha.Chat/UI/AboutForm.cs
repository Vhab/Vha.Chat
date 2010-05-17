using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Vha.Chat;

namespace Vha.Chat.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            // Set the version
            this._version.Text = "Version " + Application.ProductVersion;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this._license.Select(0, 0);
        }
    }
}