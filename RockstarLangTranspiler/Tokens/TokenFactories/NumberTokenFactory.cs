using System;
using System.Collections.Generic;
using System.Globalization;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class NumberTokenFactory : ITokenFactory<NumberToken>
    {
        public IReadOnlyCollection<string> KeyWords => Array.Empty<string>();

        public bool CanParseFarther(string value) 
            => value.EndsWith('.') || IsValidForToken(value);

        public NumberToken CreateToken(int startLocation, string value)
        {
            return new NumberToken(startLocation, value.Length, value);
        }

        public bool IsValidForToken(string value) => float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }
}
