using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VhaBot.Chat
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            // Set the version
            this.Version.Text = "Version " + Application.ProductVersion;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            // Focus the form instead of the License textbox
            this.License.Select(0, 0);
        }
    }
}