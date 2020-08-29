using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionDeclarationTokenFactory : KeyWordBasedTokenFactory<FunctionDeclarationToken>
    {

        private static readonly string[] _keyWords = new[] { Takes };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionDeclarationToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new FunctionDeclarationToken(linePosition, lineNumber, value);
        }
    }
}
