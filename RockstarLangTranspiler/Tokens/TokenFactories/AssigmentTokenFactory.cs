using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AssigmentTokenFactory : KeyWordBasedTokenFactory<AssigmentToken>
    {
        private static readonly string[] _keyWords = new[] { Let, Be, Is, Are, Was, Were, Put, Into };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override AssigmentToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new AssigmentToken(linePosition, lineNumber, value);
        }

    }
}
