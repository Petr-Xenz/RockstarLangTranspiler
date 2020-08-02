using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AdditionTokenFactory : TokenFactory
    {

        private static string[] _keyWords = { "plus", "with" };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override Token CreateToken(int startLocation, string value)
        {
            return new AdditionToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value) => KeyWords.Any(kw => string.Compare(value, kw, true) == 0);
    }
}
