using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class NotTokenFactory : KeyWordBasedTokenFactory<NotToken>
    {
        private static readonly string[] _keyWords = new[] { Not };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override NotToken CreateToken(int linePosition, int lineNumber, string value) => new NotToken(linePosition, lineNumber, value);
    }
}
