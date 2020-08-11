using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AssigmentTokenFactory : TokenFactory
    {
        public override IReadOnlyCollection<string> KeyWords => new []{ "let", "be", "is", "put", "into" };

        public override bool CanParseFarther(string value) => false;

        public override Token CreateToken(int startLocation, string value)
        {
            return new AssigmentToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value) => KeyWords.Any(kw => kw.Equals(value, System.StringComparison.OrdinalIgnoreCase));
    }
}
