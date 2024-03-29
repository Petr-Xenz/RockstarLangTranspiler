﻿using System;
using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class WhitespaceTokenFactory : ITokenFactory<WhitespaceToken>
    {
        public IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public bool CanParseFarther(string value) 
            => value.Length > 0 && string.IsNullOrWhiteSpace(value);

        public WhitespaceToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new WhitespaceToken(linePosition, lineNumber);
        }

        public bool IsValidForToken(string value) => value == " ";
    }
}
