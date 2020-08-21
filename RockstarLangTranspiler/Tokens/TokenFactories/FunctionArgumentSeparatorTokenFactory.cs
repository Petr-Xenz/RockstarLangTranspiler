using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionArgumentSeparatorTokenFactory : KeyWordBasedTokenFactory<FunctionArgumentSeparatorToken>
    {
        private static string[] _keyWords => new[] { "&", "'n'" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override FunctionArgumentSeparatorToken CreateToken(int startLocation, string value) 
            => new FunctionArgumentSeparatorToken(startLocation, value.Length, value);
    }
}
