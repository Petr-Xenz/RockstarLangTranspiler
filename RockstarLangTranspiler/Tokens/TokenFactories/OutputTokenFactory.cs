using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class OutputTokenFactory : KeyWordBasedTokenFactory<OutputToken>
    {
        private static string[] _keyWords = { Say, Whisper, Shout, Scream };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override OutputToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new OutputToken(linePosition, lineNumber, value);
        }
    }
}
