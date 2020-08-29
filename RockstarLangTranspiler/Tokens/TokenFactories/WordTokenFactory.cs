using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class WordTokenFactory : ITokenFactory<WordToken>
    {
        public IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public bool CanParseFarther(string value)
        {
            return true;
        }

        public WordToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new WordToken(linePosition, lineNumber, value);
        }

        public bool IsValidForToken(string value)
        {
            return Regex.IsMatch(value, "(a-zA-Z)*");
        }
    }
}
