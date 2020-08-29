using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AdditionTokenFactory : KeyWordBasedTokenFactory<AdditionToken>
    {
        private static string[] _keyWords = { "plus", "with" };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override AdditionToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new AdditionToken(linePosition, lineNumber, value);
        }
    }
}
