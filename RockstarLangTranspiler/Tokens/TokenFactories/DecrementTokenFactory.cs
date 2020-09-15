using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class DecrementTokenFactory : KeyWordBasedTokenFactory<DecrementToken>
    {
        private static readonly string[] _keyWords = { Knock, Down };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override DecrementToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new DecrementToken(linePosition, lineNumber, value);
        }
    }
}
