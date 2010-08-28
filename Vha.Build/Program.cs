/*
* Vha.Build
* Copyright (C) 2010 Remco van Oosterhout
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace Vha.Build
{
    class Program
    {
        static void Main(string[] args)
        {
            // Exit on DEBUG builds
#if DEBUG
            Console.WriteLine("Vha.Build is not available on debug builds");
            Environment.Exit(0);
#endif

            // Run Vha.Build
            try
            {
                // Parse arguments
                Arguments arguments = new Arguments(args);

                // Display help
                if (arguments["help"] != null)
                {
                    Console.WriteLine("Vha.Build is a utility for post-build events.");
                    Console.WriteLine("Currently its only feature is executing a NSIS script.");
                    Console.WriteLine();
                    Console.WriteLine("Available arguments:");
                    Console.WriteLine("  -help");
                    Console.WriteLine("    Optional. Displays this output");
                    Console.WriteLine();
                    Console.WriteLine("  -workingDirectory=\"...\"");
                    Console.WriteLine("    Optional. Sets the working directory to start the installer script in");
                    Console.WriteLine();
                    Console.WriteLine("  -nsisScript=\"...\"");
                    Console.WriteLine("    Mandatory. The .nsi script that should be run");
                    Console.WriteLine();
                    Console.WriteLine("  -assembly=\"...\"");
                    Console.WriteLine("    Optional. Gathers information from a specific binary and feeds it to the .nsi script");
                    Console.WriteLine();
                    Console.WriteLine("  -nsisExecutable=\"...\"");
                    Console.WriteLine("    Optional. Defines the NSIS executable that needs to be run (only works if nsisPath is also defined)");
                    Console.WriteLine();
                    Console.WriteLine("  -nsisPath=\"...\"");
                    Console.WriteLine("    Optional. Defines the path NSIS executable resides in");
                    Environment.Exit(0);
                }

                // - workingDirectory
                string workingDirectory = Environment.CurrentDirectory;
                if (arguments["workingDirectory"] != null)
                    workingDirectory = arguments["workingDirectory"];
                // - nsisScript
                string nsisScript = "";
                if (arguments["nsisScript"] != null)
                    nsisScript = arguments["nsisScript"];
                // - assembly
                string assembly = "";
                if (arguments["assembly"] != null)
                    assembly = arguments["assembly"];

                // Create NSIS tool
                NSIS installer = null;
                if (arguments["nsisExecutable"] != null && arguments["nsisPath"] != null)
                {
                    installer = new NSIS(arguments["nsisPath"], arguments["nsisExecutable"]);
                }
                else
                {
                    installer = new NSIS();
                }

                // Check if NSIS is installed
                if (!installer.IsInstalled)
                {
                    Console.WriteLine("NSIS is not installed, will not create an installer!");
                    Environment.Exit(0);
                }

                // Check if a script is defined
                if (string.IsNullOrEmpty(nsisScript))
                {
                    Console.WriteLine("Missing mandatory argument 'nsisScript'");
                    Environment.Exit(4);
                }

                // Gather assembly information
                if (!string.IsNullOrEmpty(assembly))
                {
                    if (!Path.IsPathRooted(assembly))
                        assembly = Path.Combine(workingDirectory, assembly);
                    Assembly ass = Assembly.LoadFrom(assembly);
                    installer.Define("ASSEMBLY_TITLE",
                        ((AssemblyTitleAttribute)(ass.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0])).Title);
                    installer.Define("ASSEMBLY_DESCRIPTION",
                        ((AssemblyDescriptionAttribute)(ass.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0])).Description);
                    installer.Define("ASSEMBLY_COMPANY",
                        ((AssemblyCompanyAttribute)(ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0])).Company);
                    installer.Define("ASSEMBLY_COPYRIGHT",
                        ((AssemblyCopyrightAttribute)(ass.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])).Copyright);
                    installer.Define("ASSEMBLY_VERSION", ass.GetName().Version.ToString());
                }

                // Run installer script
                bool success = installer.Run(workingDirectory, nsisScript);
                if (!success) Environment.Exit(2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: " + ex.Message);
                Environment.Exit(1);
            }

        }
    }
}
