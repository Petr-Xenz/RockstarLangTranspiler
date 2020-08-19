using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionInvokationTokenFactory : KeyWordBasedTokenFactory<FunctionInvokationToken>
    {

        private static string[] _keyWords = new[] { "taking" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionInvokationToken CreateToken(int startLocation, string value)
        {
            return new FunctionInvokationToken(0, value.Length, value);
        }
    }
}
