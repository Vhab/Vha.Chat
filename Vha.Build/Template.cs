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
    public class Template
    {
        public string WorkingDirectory { get; private set; }
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }

        public Template(string workingDirectory, string inputFile, string outputFile)
        {
            this.WorkingDirectory = workingDirectory;
            this.InputFile = inputFile;
            this.OutputFile = outputFile;
        }

        public bool Run()
        {
            Dictionary<string, string> replacements = new Dictionary<string,string>();

            // Revision number
            string numberString = GetOutput("hg.exe", "id -n");
            if (numberString == null) return false;
            numberString = numberString.TrimEnd('+');
            int number = 0;
            if (!int.TryParse(numberString, out number))
            {
                Console.WriteLine("Unable to obtain mercurial revision number");
            }
            replacements.Add("{MERCURIAL_REVISION_NUMBER}", number.ToString());

            // Revision ID
            string revision = GetOutput("hg.exe", "id -i");
            if (revision == null) return false;
            revision = revision.TrimEnd('+');
            if (revision.Length != 12)
            {
                Console.WriteLine("Unable to obtain mercurial revision id");
                revision = "";
            }
            replacements.Add("{MERCURIAL_REVISION}", revision);

            string branch = GetOutput("hg.exe", "id -b");
            // Branch);
            if (branch == null) return false;
            if (branch.Contains(" ") || branch.Contains("\n"))
            {
                Console.WriteLine("Unable to obtain mercurial branch");
                branch = "";
            }
            replacements.Add("{MERCURIAL_BRANCH}", branch);

            // Open input file
            FileStream inputStream = null;
            StreamReader inputReader = null;
            string input = null;
            try
            {
                inputStream = File.OpenRead(this.InputFile);
                inputReader = new StreamReader(inputStream);
                input = inputReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading input file: " + ex.Message);
                return false;
            }
            finally
            {
                if (inputReader != null)
                    inputReader.Close();
                if (inputStream != null)
                    inputStream.Close();
            }

            // Fill out template
            string output = input;
            foreach (KeyValuePair<string, string> replacement in replacements)
            {
                output = output.Replace(replacement.Key, replacement.Value);
            }

            // Write output
            FileStream outputStream = null;
            StreamWriter outputWriter = null;
            try
            {
                outputStream = File.Open(this.OutputFile, FileMode.Create, FileAccess.Write);
                outputWriter = new StreamWriter(outputStream);
                outputWriter.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while writing output file: " + ex.Message);
                return false;
            }
            finally
            {
                if (outputWriter != null)
                    outputWriter.Close();
                if (outputStream != null)
                    outputStream.Close();
            }

            // And we're done
            return true;
        }

        private string GetOutput(string exec, string arguments)
        {
            try
            {
                // Setup
                ProcessStartInfo si = new ProcessStartInfo(exec, arguments);
                si.WorkingDirectory = this.WorkingDirectory;
                si.UseShellExecute = false;
                si.RedirectStandardOutput = true;
                // Create process
                Process process = Process.Start(si);
                process.WaitForExit();
                // Read output
                string output = process.StandardOutput.ReadToEnd();
                return output.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while executing command: " + ex.Message);
                return null;
            }
        }
    }
}
