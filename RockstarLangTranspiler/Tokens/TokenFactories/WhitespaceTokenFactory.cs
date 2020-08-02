using System;
using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class WhitespaceTokenFactory : TokenFactory
    {
        public override IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public override bool CanParseFarther(string value) 
            => value.Length > 0 && string.IsNullOrWhiteSpace(value);

        public override Token CreateToken(int startLocation, string value)
        {
            return new WhitespaceToken(startLocation);
        }

        public override bool IsValidForToken(string value) => value == " ";
    }
}
