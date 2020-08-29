using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AssigmentTokenFactory : KeyWordBasedTokenFactory<AssigmentToken>
    {
        public override IReadOnlyCollection<string> KeyWords => new []{ "let", "be", "is", "are", "was", "were", "put", "into" };

        public override bool CanParseFarther(string value) => false;

        public override AssigmentToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new AssigmentToken(linePosition, lineNumber, value);
        }

    }
}
