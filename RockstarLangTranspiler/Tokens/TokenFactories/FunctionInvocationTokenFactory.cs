using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionInvocationTokenFactory : KeyWordBasedTokenFactory<FunctionInvocationToken>
    {

        private static string[] _keyWords = new[] { "taking" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionInvocationToken CreateToken(int startLocation, string value)
        {
            return new FunctionInvocationToken(0, value.Length, value);
        }
    }
}
