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
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments supplied");
                Environment.Exit(1);
            }

            // Run Vha.Build
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: " + ex.Message);
                Environment.Exit(1);
            }

        }

        static void Run(string[] args)
        {
            // Parse arguments
            Arguments arguments = new Arguments(args);

            // - workingDirectory
            string workingDirectory = Environment.CurrentDirectory;
            if (arguments["workingDirectory"] != null)
                workingDirectory = arguments["workingDirectory"];

            // Switch between modes
            int returnCode = 0;
            switch (args[0])
            {
                case "help":
                    Console.WriteLine("Vha.Build is a utility for pre and/or post-build events.");
                    Console.WriteLine("Currently it features 2 modes, installer and temple");
                    Console.WriteLine();
                    Console.WriteLine("Available options:");
                    Console.WriteLine("* help");
                    Console.WriteLine("   Displays this output");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("* installer");
                    Console.WriteLine("   Initializes and runs a NSIS script");
                    Console.WriteLine();
                    Console.WriteLine("   Available arguments:");
                    Console.WriteLine();
                    Console.WriteLine("    -nsisScript=\"...\"");
                    Console.WriteLine("      Mandatory. The .nsi script that should be run");
                    Console.WriteLine();
                    Console.WriteLine("    -assembly=\"...\"");
                    Console.WriteLine("      Optional. Gathers information from a specific binary and feeds it to the .nsi script");
                    Console.WriteLine();
                    Console.WriteLine("    -nsisExecutable=\"...\"");
                    Console.WriteLine("      Optional. Defines the NSIS executable that needs to be run (only works if nsisPath is also defined)");
                    Console.WriteLine();
                    Console.WriteLine("    -nsisPath=\"...\"");
                    Console.WriteLine("      Optional. Defines the path NSIS executable resides in");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("* template");
                    Console.WriteLine("   Generates an source file based on a template input file");
                    Console.WriteLine();
                    Console.WriteLine("   Available arguments:");
                    Console.WriteLine();
                    Console.WriteLine("    -inputFile=\"...\"");
                    Console.WriteLine("      Mandatory. The input template file to be parsed");
                    Console.WriteLine();
                    Console.WriteLine("    -outputFile=\"...\"");
                    Console.WriteLine("      Mandatory. The output file to be written.");
                    Console.WriteLine("      Will overwrite any existing file.");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Global arguments:");
                    Console.WriteLine();
                    Console.WriteLine("  -workingDirectory=\"...\"");
                    Console.WriteLine("    Optional. Sets the working directory to start the finding and executing files in");
                    Console.WriteLine();
                    returnCode = 0;
                    break;
                case "installer":
#if DEBUG
                    Console.WriteLine("Running in debug mode, skipping installer generation");
                    returnCode = 0;
#else
                    returnCode = RunInstaller(workingDirectory, arguments);
#endif
                    break;
                case "template":
                    returnCode = RunTemplate(workingDirectory, arguments);
                    break;
            }

            // Exit
            if (returnCode != 0)
            {
                Console.WriteLine("Vha.Build exiting with error code " + returnCode);
            }
            Environment.Exit(returnCode);
        }

        static int RunInstaller(string workingDirectory, Arguments args)
        {
            // - nsisScript
            string nsisScript = "";
            if (args["nsisScript"] != null)
                nsisScript = args["nsisScript"];
            // - assembly
            string assembly = "";
            if (args["assembly"] != null)
                assembly = args["assembly"];

            // Create NSIS tool
            NSIS installer = null;
            if (args["nsisExecutable"] != null && args["nsisPath"] != null)
            {
                installer = new NSIS(args["nsisPath"], args["nsisExecutable"]);
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
            if (!success) return 2;
            return 0;
        }

        static int RunTemplate(string workingDirectory, Arguments args)
        {
            // Input file
            string inputFile = args["inputFile"];
            if (string.IsNullOrEmpty(inputFile))
            {
                Console.WriteLine("Missing 'inputFile' argument");
                return 1;
            }
            string inputFilePath = inputFile;
            if (!Path.IsPathRooted(inputFile))
                inputFilePath = Path.Combine(workingDirectory, inputFile);
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Unable to locate '" + inputFile + "'");
                return 2;
            }

            // Output file
            string outputFile = args["outputFile"];
            if (string.IsNullOrEmpty(outputFile))
            {
                Console.WriteLine("Missing 'outputFile' argument");
                return 1;
            }
            string outputFilePath = outputFile;
            if (!Path.IsPathRooted(outputFile))
                outputFilePath = Path.Combine(workingDirectory, outputFile);

            // Setup
            Template template = new Template(workingDirectory, inputFilePath, outputFilePath);
            bool success = template.Run();
            if (!success) return 4;
            return 0;
        }
    }
}
