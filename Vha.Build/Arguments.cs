/*
* Vha.Build
* Copyright (C) 2010 Remco van Oosterhout
* 
* CommandLine.Utility.Arguments
* Copyright (C) 2002 Richard Lopes
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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vha.Build
{
    public class Arguments
    {
        private Dictionary<string, string> _parameters;

        public Arguments(string[] args)
        {
            this._parameters = new Dictionary<string, string>();
            Regex spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;
            string[] parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples:
            // -param1 value1 --param2 /param3:"Test-:-work"
            //   /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                parts = spliter.Split(txt, 3);
                switch (parts.Length)
                {
                    // Found a value (for the last parameter
                    // found (space separator))
                    case 1:
                        if (parameter != null)
                        {
                            if (!this._parameters.ContainsKey(parameter))
                            {
                                parts[0] =
                                    remover.Replace(parts[0], "$1");

                                this._parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;
                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting.
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!this._parameters.ContainsKey(parameter))
                                this._parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;
                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting.
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!this._parameters.ContainsKey(parameter))
                                this._parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        // Remove possible enclosing characters (",')
                        if (!this._parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            this._parameters.Add(parameter, parts[2]);
                        }
                        parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!this._parameters.ContainsKey(parameter))
                    this._parameters.Add(parameter, "true");
            }
        }

        // Retrieve a parameter value if it exists
        // (overriding C# indexer property)
        public string this[string param]
        {
            get
            {
                if (!this._parameters.ContainsKey(param)) return null;
                return this._parameters[param];
            }
        }
    }
}