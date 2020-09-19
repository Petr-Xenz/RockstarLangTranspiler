using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionReturnTokenFactory : KeyWordBasedTokenFactory<FunctionReturnToken>
    {

        private static string[] _keyWords = new[] { Give, Back };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionReturnToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new FunctionReturnToken(0, value.Length, value);
        }
    }
}
