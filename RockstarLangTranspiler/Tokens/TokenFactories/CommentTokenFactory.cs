using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class CommentTokenFactory : KeyWordBasedTokenFactory<CommentToken>
    {
        private static string[] _keyWords => new[] { "(", ")" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override CommentToken CreateToken(int linePosition, int lineNumber, string value) => new CommentToken(linePosition, lineNumber, value);
    }
}
