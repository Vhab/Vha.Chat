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

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Vha.AOML
{
    public class Tokenizer
    {
        /// <summary>
        /// Returns the amount of tokens currently contained within this object
        /// </summary>
        public int Count
        {
            get
            {
                if (this._tokens == null) return 0;
                return this._tokens.Count;
            }
        }

        /// <summary>
        /// Returns the current position of the tokenizer.
        /// This value is always 0 or greater and less or equal to Count.
        /// </summary>
        public int Current { get { return this._current; } }

        /// <summary>
        /// Loads and tokenizes a string.
        /// Calling this method will clear the state of this object and make any previous returned values invalid.
        /// </summary>
        /// <param name="text">The string to be tokenized</param>
        public void Load(string text)
        {
            // Replace 'other' newline characters
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\n\r", "\n");
            // Clean up
            Clear();
            // Tokenize
            int offset = 0;
            while (offset < text.Length)
            {
                // Check for identifiers
                Match identifier = this._identifierRegex.Match(text, offset);
                if (identifier != null)
                {
                    this.PushToken(TokenType.Identifier, identifier.Value, ref offset);
                    continue;
                }
                // Check for characters
                switch (text[offset])
                {
                    case '<':
                        this.PushToken(TokenType.TagBegin, "<", ref offset);
                        break;
                    case '>':
                        this.PushToken(TokenType.TagEnd, ">", ref offset);
                        break;
                    case ' ':
                        this.PushToken(TokenType.WhiteSpace, " ", ref offset);
                        break;
                    case '\t':
                        this.PushToken(TokenType.WhiteSpace, "\t", ref offset);
                        break;
                    case '\n':
                        this.PushToken(TokenType.LineBreak, "\n", ref offset);
                        break;
                    case '\'':
                        this.PushToken(TokenType.SingleQuote, "'", ref offset);
                        break;
                    case '"':
                        this.PushToken(TokenType.DoubleQuote, "\"", ref offset);
                        break;
                    case '=':
                        this.PushToken(TokenType.Equal, "=", ref offset);
                        break;
                    default:
                        this.PushToken(TokenType.Text, text[offset].ToString(), ref offset);
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the Tokenizer to its initial state before Load was called
        /// </summary>
        public void Clear()
        {
            this._tokens.Clear();
            this._current = 0;
        }

        /// <summary>
        /// Resets the Tokenizer back to its initial state after Load was called
        /// </summary>
        public void Reset()
        {
            this._current = 0;
        }

        /// <summary>
        /// Progresses the tokenizer to the next Token in the buffer and advances the internal state
        /// </summary>
        /// <returns>The next Token or null if the end is reached</returns>
        public Token Next()
        {
            if (this.Count == 0) return null;
            if (this.Current == this.Count) return null;
            Token token = this._tokens[this._current];
            this._current++;
            return token;
        }

        /// <summary>
        /// Returns the next token without advancing the internal state of the Tokenizer object
        /// </summary>
        /// <param name="count">The amount of tokens to skip before returning the next Token</param>
        /// <returns>The next Token or null if the end is reached</returns>
        public Token Peek(int count)
        {
            int offset = this._current + count;
            if (offset + 1 >= this.Count) return null;
            if (offset < 0) return null;
            return this._tokens[offset];
        }

        /// <summary>
        /// Finds the next occurrence of a Token of the specified type
        /// </summary>
        /// <param name="type">The type of Token to search for</param>
        /// <returns>The distance to the requested Token or -1 if the Token wasn't found</returns>
        public int Find(TokenType type)
        {
            int distance = 0;
            Token next = null;
            while ((next = this.Peek(distance)) != null)
            {
                if (next.Type == type)
                    return distance;
                distance++;
            }
            return -1;
        }

        #region Internal
        private List<Token> _tokens = new List<Token>();
        private Regex _identifierRegex = new Regex("^[a-z][a-z0-9_\\-]*", RegexOptions.IgnoreCase);
        private int _current = 0;

        private void PushToken(TokenType type, string value, ref int offset)
        {
            offset += value.Length;
            // Merge tokens if possible (only with WhiteSpace and Text)
            if (type == TokenType.WhiteSpace || type == TokenType.Text)
            {
                // Only when there actually is another token to merge
                if (this._tokens.Count > 0)
                {
                    // Only when the type matches
                    Token top = this._tokens[this._tokens.Count];
                    if (top.Type == type)
                    {
                        this._tokens[this._tokens.Count] = new Token(type, top.Value + value);
                        return;
                    }
                }
            }
            // Can't merge, just add a new token
            this._tokens.Add(new Token(type, value));
        }
        #endregion
    }
}
