using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class IsTokenFactory : KeyWordBasedTokenFactory<IsToken>
    {
        private static readonly string[] _keyWords = new[] { Is };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override IsToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new IsToken(linePosition, lineNumber, value);
        }
    }
}
