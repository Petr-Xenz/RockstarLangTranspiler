using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class WhileTokenFactory : KeyWordBasedTokenFactory<WhileToken>
    {
        private static readonly string[] _keyWords = new[] { While, Until };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override WhileToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new WhileToken(linePosition, lineNumber, value);
        }
    }
}
