using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class DisjunctionTokenFactory : KeyWordBasedTokenFactory<DisjunctionToken>
    {
        private static readonly string[] _keyWords = new[] { Or };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override DisjunctionToken CreateToken(int linePosition, int lineNumber, string value) 
            => new DisjunctionToken(linePosition, lineNumber, value);
    }
}
