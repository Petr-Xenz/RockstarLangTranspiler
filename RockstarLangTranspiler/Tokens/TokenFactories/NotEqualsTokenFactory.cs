using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class NotEqualsTokenFactory : KeyWordBasedTokenFactory<NotEqualsToken>
    {
        private static readonly string[] _keyWords = new[] { Isnt, Aint };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override NotEqualsToken CreateToken(int linePosition, int lineNumber, string value) => new NotEqualsToken(linePosition, lineNumber, value);
    }
}
