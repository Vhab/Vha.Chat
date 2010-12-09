using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using Vha.Chat.Data;

namespace Vha.Chat.UI
{
    public partial class DirectorySelectionForm : Form
    {
        public DirectorySelectionForm(ConfigurationV1 configuration, string localPath, string appDataPath)
        {
            InitializeComponent();

            this._configuration = configuration;
            this._localPath = localPath;
            this._appDataPath = appDataPath;

            this._configuration.OptionsPath = null;
            this._defaultDirectory.Enabled = this._testDirectory(localPath);
            this._appData.Enabled = this._testDirectory(appDataPath);
        }

        private ConfigurationV1 _configuration;
        private string _localPath;
        private string _appDataPath;

        private void _defaultDirectory_Click(object sender, EventArgs e)
        {
            this._configuration.OptionsPath = this._localPath;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void _appData_Click(object sender, EventArgs e)
        {
            this._configuration.OptionsPath = this._appDataPath;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void _readOnly_Click(object sender, EventArgs e)
        {
            this._configuration.OptionsPath = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool _testDirectory(string path)
        {
            // Early out
            if (path == null) return false;
            // Test permissions
            try
            {
                FileIOPermission permission = new FileIOPermission(
                    FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, path);
                permission.Demand();
            }
            catch (SecurityException)
            {
                // Permission denied
                return false;
            }
            // Try to create the directory
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    // Failed to create directory
                    return false;
                }
            }
            // Test directory by actually writing to it
            try
            {
                string file = path + Path.DirectorySeparatorChar + "Vha.Chat.tmp";
                FileStream stream = File.Create(file);
                stream.Close();
                File.Delete(file);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
