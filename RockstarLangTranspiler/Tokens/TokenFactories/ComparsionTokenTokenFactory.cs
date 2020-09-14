using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class ComparsionTokenTokenFactory : KeyWordBasedTokenFactory<ComparsionToken>
    {
        private static string[] _keyWords => new[] {
            Isnt, Aint, As, Than,
            High, Higher, Great, Greater, Big, Bigger, Strong, Stronger,
            Low, Lower, Small, Smaller, Little, Weak, Weaker, Less,            
        };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override ComparsionToken CreateToken(int linePosition, int lineNumber, string value) => new ComparsionToken(linePosition, lineNumber, value);
    }
}
