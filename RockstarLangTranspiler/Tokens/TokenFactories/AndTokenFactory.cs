using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AndTokenFactory : KeyWordBasedTokenFactory<AndToken>
    {
        private static readonly string[] _keyWords = new[] { "and" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override AndToken CreateToken(int startLocation, string value) => new AndToken(startLocation, 3, value);
    }

    public class ElseTokenFactory : KeyWordBasedTokenFactory<ElseToken>
    {
        private static readonly string[] _keyWords = new[] { "else" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override ElseToken CreateToken(int startLocation, string value)
        {
            return new ElseToken(startLocation, value.Length, value);
        }
    }
}
