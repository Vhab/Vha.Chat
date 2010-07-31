/*
* Vha.AOML
* Copyright (C) 2010 Remco van Oosterhout
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

using System.Text.RegularExpressions;

namespace Vha.AOML
{
    public class Tokenizer
    {
        /// <summary>
        /// Loads and tokenizes a string.
        /// Calling this method will clear the state of this object and make any previous returned values invalid.
        /// </summary>
        /// <param name="text">The string to be tokenized</param>
        public TokenCollection Parse(string aoml)
        {
            // Replace 'other' newline characters
            aoml = aoml.Replace("\r\n", "\n");
            aoml = aoml.Replace("\n\r", "\n");
            // Tokenize
            TokenCollection tokens = new TokenCollection();
            int offset = 0;
            while (offset < aoml.Length)
            {
                // Check for identifiers
                Match identifier = this._identifierRegex.Match(aoml, offset);
                if (identifier != null)
                {
                    tokens.Add(TokenType.Identifier, identifier.Value, ref offset);
                    continue;
                }
                // Check for characters
                switch (aoml[offset])
                {
                    case '<':
                        tokens.Add(TokenType.TagBegin, "<", ref offset);
                        break;
                    case '>':
                        tokens.Add(TokenType.TagEnd, ">", ref offset);
                        break;
                    case ' ':
                        tokens.Add(TokenType.WhiteSpace, " ", ref offset);
                        break;
                    case '\t':
                        tokens.Add(TokenType.WhiteSpace, "\t", ref offset);
                        break;
                    case '\n':
                        tokens.Add(TokenType.LineBreak, "\n", ref offset);
                        break;
                    case '\'':
                        tokens.Add(TokenType.SingleQuote, "'", ref offset);
                        break;
                    case '"':
                        tokens.Add(TokenType.DoubleQuote, "\"", ref offset);
                        break;
                    case '=':
                        tokens.Add(TokenType.Equal, "=", ref offset);
                        break;
                    case '/':
                        tokens.Add(TokenType.Slash, "/", ref offset);
                        break;
                    default:
                        tokens.Add(TokenType.Text, aoml[offset].ToString(), ref offset);
                        break;
                }
            }
            return tokens;
        }

        #region Internal
        private Regex _identifierRegex = new Regex("^[a-z][a-z0-9_\\-]*", RegexOptions.IgnoreCase);
        #endregion
    }
}
