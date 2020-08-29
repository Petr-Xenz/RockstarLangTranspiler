using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class PronounTokenFactory : KeyWordBasedTokenFactory<PronounToken>
    {
        private static string[] _keyWords = new string[]
        {
            "it", "he", "she", "him", "her", "they", "them", "ze", "hir", "zie", "zir", "xe", "xem", "ve", "ver"
        };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override PronounToken CreateToken(int linePosition, int lineNumber, string value) => new PronounToken(linePosition, lineNumber, value);

    }
}
