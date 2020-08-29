using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionInvocationTokenFactory : KeyWordBasedTokenFactory<FunctionInvocationToken>
    {

        private static string[] _keyWords = new[] { Taking };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionInvocationToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new FunctionInvocationToken(0, value.Length, value);
        }
    }
}
