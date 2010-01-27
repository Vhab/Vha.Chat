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

namespace Vha.Chat
{
    public static class Program
    {
        public static bool MonoMode = false;
        public static int MaximumMessages = 250;
        public static int MaximumTexts = 50;
        public static int MaximumHistory = 25;
        public static ApplicationContext Context;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for mono
            if (Type.GetType("Mono.Runtime") != null)
            {
                MonoMode = true;
            }
            // Start application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);

            Context = new ApplicationContext();
            Context.MainForm = new AuthenticationForm();
            Application.Run(Context);
        }

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
            DisplayException(ex);
            MessageBox.Show("An exception has occurred. This application will now close.\n\nException:" + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        private static void DisplayException(Exception ex)
        {
            Console.WriteLine("Exception: " + ex.ToString());
            if (ex.InnerException != null)
                DisplayException(ex.InnerException);
        }

    }
}