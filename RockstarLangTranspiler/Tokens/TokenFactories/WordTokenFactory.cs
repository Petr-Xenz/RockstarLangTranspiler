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

        public WordToken CreateToken(int startLocation, string value)
        {
            return new WordToken(startLocation, value.Length, value);
        }

        public bool IsValidForToken(string value)
        {
            return Regex.IsMatch(value, "(a-zA-Z)*");
        }
    }
}
