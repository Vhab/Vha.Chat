using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Vha.Chat
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void _save_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}