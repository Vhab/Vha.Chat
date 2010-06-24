/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
            // Create context
            Context context = new Context();
            // Initialize Windows.Forms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
#endif
            // Start application
            ApplicationContext = new ApplicationContext();
            ApplicationContext.MainForm = new AuthenticationForm(context);
            Application.Run(ApplicationContext);
        }

#if !DEBUG
        // Exception handling
        public static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException((Exception)e.ExceptionObject);
        }

        public static void UnhandledException(Vha.Net.Chat chat, Exception ex)
        {
            UnhandledException(ex);
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