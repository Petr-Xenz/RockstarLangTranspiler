using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class JointDenialTokenFactory : KeyWordBasedTokenFactory<JointDenialToken>
    {
        private static readonly string[] _keyWords = new[] { Nor };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override JointDenialToken CreateToken(int linePosition, int lineNumber, string value) 
            => new JointDenialToken(linePosition, lineNumber, value);
    }
}
