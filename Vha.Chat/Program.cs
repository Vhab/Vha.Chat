/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; version 2 of the License only.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
* USA
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Vha.Chat;
using Vha.Chat.UI;
using Vha.Common;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace Vha.Chat
{
    public static class Program
    {
        /// <summary>
        /// Returns the main ApplicationContext to control the main form
        /// </summary>
        public static ApplicationContext ApplicationContext;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check security
            try
            {
                SecurityPermission permissions = new SecurityPermission(PermissionState.Unrestricted);
                permissions.Demand();
            }
            catch (SecurityException)
            {
                MessageBox.Show(
                    "Vha.Chat was unable to obtain the required execution permissions.\n" +
                    "This might be because you're attempting to run this application from a network drive or due to specific restrictions setup on your machine.\n\n" +
                    "This application will now close.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Fix working directory
            string path = Assembly.GetExecutingAssembly().Location;
            if (path.LastIndexOf(Path.DirectorySeparatorChar) > 0)
            {
                path = Path.GetDirectoryName(path);
                Environment.CurrentDirectory = path;
            }
            // Create context
            Context context = new Context();
            // Check for readonly mode
            if (context.Configuration.ReadOnly)
            {
                MessageBox.Show(
                    "Vha.Chat was unable to obtain the required filesystem permissions.\n" +
                    "All preferences will be lost when you close this application.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // Initialize Windows.Forms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            context.ExceptionEvent += new Handler<Exception>(UnhandledException);
#endif
            // Start application
            ApplicationContext = new ApplicationContext();
            ApplicationContext.MainForm = new AuthenticationForm(context);
            Application.Run(ApplicationContext);
        }

#if !DEBUG
        // Exception handling
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException((Exception)e.ExceptionObject);
        }

        private static void UnhandledException(Context context, Exception args)
        {
            UnhandledException(args);
        }

        private static void UnhandledException(Exception ex)
        {
            LogException(ex, 0);
            MessageBox.Show("An exception has occurred. This application will now close.\n" +
                "The full error message has been written to error.log.\n" +
                "Please assist us in fixing this bug and report this error at http://forums.vhabot.net/.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        private static void LogException(Exception ex, int depth)
        {
            try
            {
                FileStream stream = File.Open("error.log", FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                if (depth == 0)
                {
                    writer.WriteLine("-------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToLongTimeString());
                    writer.WriteLine("-------------------------------------------------------------------------");
                }
                writer.WriteLine(ex.ToString());
                writer.WriteLine();
                writer.Close();
            }
            catch {}
            if (ex.InnerException != null)
                LogException(ex.InnerException, depth + 1);
        }
#endif
    }
}