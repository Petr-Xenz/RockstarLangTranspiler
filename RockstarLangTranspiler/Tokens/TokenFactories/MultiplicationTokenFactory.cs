using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class MultiplicationTokenFactory : KeyWordBasedTokenFactory<MultiplicationToken>
    {
        private static readonly string[] _keyWords = { Times, Of };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override MultiplicationToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new MultiplicationToken(linePosition, lineNumber, value);
        }
    }
}
