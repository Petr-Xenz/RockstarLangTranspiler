using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class ElseTokenFactory : KeyWordBasedTokenFactory<ElseToken>
    {
        private static readonly string[] _keyWords = new[] { "else" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override ElseToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new ElseToken(linePosition, lineNumber, value);
        }
    }
}
