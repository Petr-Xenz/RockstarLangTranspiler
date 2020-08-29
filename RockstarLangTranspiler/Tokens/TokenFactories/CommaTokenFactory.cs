using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class CommaTokenFactory : KeyWordBasedTokenFactory<CommaToken>
    {
        private static string[] _keyWords => new[] { "," };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override CommaToken CreateToken(int linePosition, int lineNumber, string value) => new CommaToken(linePosition, lineNumber);
    }
}
