using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionReturnTokenFactory : KeyWordBasedTokenFactory<FunctionReturnToken>
    {

        private static string[] _keyWords = new[] { "gives", "back" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionReturnToken CreateToken(int startLocation, string value)
        {
            return new FunctionReturnToken(0, value.Length, value);
        }
    }
}
