using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class BreakTokenFactory : KeyWordBasedTokenFactory<BreakToken>
    {
        private static readonly string[] _keyWords = new[] { Break };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override BreakToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new BreakToken(linePosition, lineNumber, value);
        }
    }
}
