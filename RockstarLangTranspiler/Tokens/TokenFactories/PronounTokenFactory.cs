using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class PronounTokenFactory : TokenFactory
    {
        private static string[] _keyWords = new string[]
        {
            "it", "he", "she", "him", "her", "they", "them", "ze", "hir", "zie", "zir", "xe", "xem", "ve", "ver"
        };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override Token CreateToken(int startLocation, string value) => new PronounToken(startLocation, value.Length, value);

        public override bool IsValidForToken(string value) => KeyWords.Any(kw => string.Compare(value, kw, true) == 0);
    }
}
