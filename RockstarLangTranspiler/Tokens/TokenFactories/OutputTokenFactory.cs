using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class OutputTokenFactory : TokenFactory
    {
        private static string[] _keyWords = { "say", "whisper", "shout", "scream" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override Token CreateToken(int startLocation, string value)
        {
            return new OutputToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value)
            => _keyWords.Any(k => string.Equals(k, value, System.StringComparison.OrdinalIgnoreCase));

    }
}
