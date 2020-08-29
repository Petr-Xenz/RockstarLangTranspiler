using System.Collections.Generic;

using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class BooleanTokenFactory : KeyWordBasedTokenFactory<BooleanToken>
    {
        private static readonly string[] _keyWords = new[] { Right, Ok, Yes, Wrong, No, Lies };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override BooleanToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new BooleanToken(linePosition, lineNumber, value);
        }
    }
}
