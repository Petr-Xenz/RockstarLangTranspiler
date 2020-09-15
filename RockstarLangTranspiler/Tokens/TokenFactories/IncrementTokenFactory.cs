using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class IncrementTokenFactory : KeyWordBasedTokenFactory<IncrementToken>
    {
        private static readonly string[] _keyWords = { Build, Up };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override IncrementToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new IncrementToken(linePosition, lineNumber, value);
        }
    }
}
