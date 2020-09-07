using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class NullTokenFactory : KeyWordBasedTokenFactory<NullToken>
    {
        private static readonly string[] _keyWords = { Null };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override NullToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new NullToken(linePosition, lineNumber, value);
        }
    }

    public class ProperVariablePrefixTokenFactory : KeyWordBasedTokenFactory<ProperVariablePrefixToken>
    {
        private static readonly string[] _keyWords = { A, An, The, My, Your };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override ProperVariablePrefixToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new ProperVariablePrefixToken(linePosition, lineNumber, value);
        }
    }
}
