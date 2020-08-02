using System;
using System.Collections.Generic;
using System.Globalization;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class NumberTokenFactory : TokenFactory
    {
        public override IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public override bool CanParseFarther(string value) 
            => value.EndsWith('.') || IsValidForToken(value);

        public override Token CreateToken(int startLocation, string value)
        {
            return new NumberToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value) => float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }
}
