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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Vha.Build
{
    public class NSIS
    {
        private string _path;
        private string _executable;
        private Dictionary<string, string> _definitions;

        public NSIS()
        {
            this._executable = "NSIS" + Path.DirectorySeparatorChar + "makensis.exe";
            this._path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + Path.DirectorySeparatorChar;
            if (!this.IsInstalled) this._path = "C:/Program Files/";
            if (!this.IsInstalled) this._path = "C:/Program Files (x86)/";
            this._definitions = new Dictionary<string, string>();
        }

        public NSIS(string path, string executable)
        {
            this._path = Path.GetFullPath(path);
            this._executable = executable;
            this._definitions = new Dictionary<string, string>();
        }

        public bool IsInstalled
        {
            get { return File.Exists(this.FullPath); }
        }

        public string FullPath
        {
            get { return Path.Combine(this._path, this._executable); }
        }

        public void Define(string key, string value)
        {
            if (this._definitions.ContainsKey(key))
                this._definitions[key] = value;
            else this._definitions.Add(key, value);
            Console.WriteLine("Defined '{0}' as '{1}'", key, value);
        }

        public bool Run(string script)
        {
            return this.Run(Environment.CurrentDirectory, script);
        }

        public bool Run(string workingDirectory, string script)
        {
            // Check if NSIS is installed
            if (!this.IsInstalled)
            {
                Console.WriteLine("Unable to locate Nullsoft Scriptable Install System");
                return false;
            }
            // Some preperations
            script = Path.GetFullPath(script);
            workingDirectory = Path.GetFullPath(workingDirectory);
            Console.WriteLine("Script: " + script);
            Console.WriteLine("Working directory: " + workingDirectory);
            StringBuilder args = new StringBuilder();
            foreach (KeyValuePair<string, string> d in this._definitions)
            {
                args.Append(string.Format(
                    "\"/D{0}={1}\" ",
                    d.Key,
                    d.Value.Replace("\\", "\\\\").Replace("\"", "\\\"")
                    ));
            }
            args.Append("/V2 ");
            args.Append("\"" + script + "\"");
            Console.WriteLine("Arguments: " + args.ToString());

            // Prepare process
            ProcessStartInfo info = new ProcessStartInfo(this.FullPath, args.ToString());
            info.WorkingDirectory = workingDirectory;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;

            // Start process
            Process process = Process.Start(info);
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Dispose();
            if (exitCode == 0) return true;

            // Report error
            Console.WriteLine("NSIS terminated with error code " + exitCode);
            return false;
        }
    }
}
