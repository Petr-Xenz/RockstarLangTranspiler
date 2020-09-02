using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class UndefinedTokenFactory : KeyWordBasedTokenFactory<UndefinedToken>
    {
        private static readonly string[] _keyWords = { Undefined };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override UndefinedToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new UndefinedToken(linePosition, lineNumber, value);
        }
    }
}
