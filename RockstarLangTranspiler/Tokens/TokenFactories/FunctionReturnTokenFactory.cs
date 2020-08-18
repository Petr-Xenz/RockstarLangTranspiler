using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionReturnTokenFactory : TokenFactory
    {

        private static string[] _keyWords = new[] { "gives", "back" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override Token CreateToken(int startLocation, string value)
        {
            return new FunctionReturnToken(0, value.Length, value);
        }

        public override bool IsValidForToken(string value)
        {
            return _keyWords.Any(w => w.Equals(value, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
