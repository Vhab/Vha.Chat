/*
* Vha.AOML
* Copyright (C) 2010-2011 Remco van Oosterhout
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Vha.AOML
{
    public class TokenCollection : IEnumerable<Token>
    {
        #region Private members
        private List<Token> tokens = new List<Token>();
        private int current = 0;
        #endregion

        #region Public accessors
        /// <summary>
        /// Returns the amount of tokens currently contained within this object
        /// </summary>
        public int Count
        {
            get
            {
                if (this.tokens == null)
                {
                    return 0;
                }
                return this.tokens.Count;
            }
        }

        /// <summary>
        /// Returns the current position of the tokenizer.
        /// This value is always 0 or greater and less or equal to Count.
        /// </summary>
        public int Current
        {
            get { return this.current; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Expecting value greater than zero");
                }
                if (value > this.Count)
                {
                    throw new ArgumentException("Expecting value smaller or equal to Count");
                }
                this.current = value;
            }
        }

        /// <summary>
        /// Returns the remaining about of tokens that can be obtained with Next().
        /// </summary>
        public int Remaining { get { return this.Count - this.Current; } }
        #endregion


        /// <summary>
        /// Returns the Tokenizer to its initial state before Load was called
        /// </summary>
        public void Clear()
        {
            this.tokens.Clear();
            this.current = 0;
        }

        /// <summary>
        /// Resets the Tokenizer back to its initial state after Load was called
        /// </summary>
        public void Reset()
        {
            this.current = 0;
        }

        /// <summary>
        /// Progresses the tokenizer to the next Token in the buffer and advances the internal state
        /// </summary>
        /// <returns>The next Token or null if the end is reached</returns>
        public Token Next()
        {
            if (this.Count == 0) { return null; }
            if (this.Current == this.Count) { return null; }
            Token token = this.tokens[this.current];
            this.current++;
            return token;
        }

        /// <summary>
        /// Returns the next token without advancing the internal state of the Tokenizer object
        /// </summary>
        /// <param name="count">The amount of tokens to skip before returning the next Token</param>
        /// <returns>The next Token or null if the end is reached</returns>
        public Token Peek(int count)
        {
            int offset = this.current + count;
            if (offset + 1 >= this.Count) { return null; }
            if (offset < 0) { return null; }
            return this.tokens[offset];
        }

        /// <summary>
        /// Finds the next occurrence of a Token of the specified type
        /// </summary>
        /// <param name="type">The type of Token to search for</param>
        /// <returns>The distance to the requested Token or -1 if the Token wasn't found</returns>
        public int Find(TokenType type)
        {
            return this.Find(type, 0);
        }

        /// <summary>
        /// Finds the next occurrence of a Token of the specified type
        /// </summary>
        /// <param name="type">The type of Token to search for</param>
        /// <param name="offset">The token count offset to start searching at</param>
        /// <returns>The distance to the requested Token or -1 if the Token wasn't found</returns>
        public int Find(TokenType type, int offset)
        {
            int distance = offset;
            Token next = null;
            while ((next = this.Peek(distance)) != null)
            {
                if (next.Type == type)
                {
                    return distance;
                }
                distance++;
            }
            return -1;
        }

        /// <summary>
        /// Reads a certain amount of tokens and returns the concatenated string
        /// </summary>
        /// <param name="tokens">The amount of tokens that should be read</param>
        /// <param name="peek">Whether to use peek or to consume all read tokens</param>
        /// <returns>A concatenated string of all token values in the sequence they are read</returns>
        public string Read(int tokens, bool peek)
        {
            StringBuilder value = new StringBuilder();
            int count = 0;
            while (tokens > 0)
            {
                tokens--;
                Token token = null;
                if (peek)
                {
                    token = this.Peek(count);
                }
                else
                {
                    token = this.Next();
                }
                if (token == null) { break; }
                value.Append(token.Value);
                count++;
            }
            return value.ToString();
        }

        public void Add(TokenType type, string value)
        {
            int offset = 0;
            this.Add(type, value, ref offset);
        }

        public void Add(TokenType type, string value, ref int offset)
        {
            offset += value.Length;
            // Merge tokens if possible (only with WhiteSpace and Text)
            if (type == TokenType.WhiteSpace || type == TokenType.Text)
            {
                // Only when there actually is another token to merge
                if (this.tokens.Count > 0)
                {
                    // Only when the type matches
                    Token top = this.tokens[this.tokens.Count - 1];
                    if (top.Type == type)
                    {
                        this.tokens[this.tokens.Count - 1] = new Token(type, top.Value + value);
                        return;
                    }
                }
            }
            // Can't merge, just add a new token
            this.tokens.Add(new Token(type, value));
        }

        /// <summary>
        /// Returns a type safe enumerator that iterates through this collection
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
        {
            return this.tokens.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through this collection
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tokens.GetEnumerator();
        }

        
    }
}
