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

namespace Vha.AOML
{
    /// <summary>
    /// Identifies the type of a Token
    /// </summary>
    public enum TokenType
    {
        TagBegin, // <
        TagEnd, // >
        Identifier, // [a-z0-9_\-]
        WhiteSpace, // space, tab, etc
        LineBreak, // newline
        SingleQuote, // '
        DoubleQuote, // "
        Equal, // =
        Slash, // /
        Text // anything, else
    }

    /// <summary>
    /// A token produced by Tokenizer
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Returns the type of this token
        /// </summary>
        public readonly TokenType Type;

        /// <summary>
        /// Returns the value of this token.
        /// Returns null if no value is associated with this token type.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new token
        /// </summary>
        /// <param name="type">The type of the token</param>
        /// <param name="value">The value of the token</param>
        public Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
