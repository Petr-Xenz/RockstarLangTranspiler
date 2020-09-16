using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class ContinueTokenFactory : KeyWordBasedTokenFactory<ContinueToken>
    {
        private static readonly string[] _keyWords = new[] { Continue, Take };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override ContinueToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new ContinueToken(linePosition, lineNumber, value);
        }
    }
}
