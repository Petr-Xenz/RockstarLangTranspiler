using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class WordTokenFactory : TokenFactory
    {
        public override IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public override bool CanParseFarther(string value)
        {
            return true;
        }

        public override Token CreateToken(int startLocation, string value)
        {
            return new WordToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value)
        {
            return Regex.IsMatch(value, "(a-zA-Z)*");
        }
    }
}
