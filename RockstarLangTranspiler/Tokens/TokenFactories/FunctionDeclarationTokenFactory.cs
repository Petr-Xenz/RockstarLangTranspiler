using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class FunctionDeclarationTokenFactory : TokenFactory
    {

        private static readonly string[] _keyWords = new[] { "takes" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override Token CreateToken(int startLocation, string value)
        {
            return new FunctionDeclarationToken(startLocation, value.Length, value);
        }

        public override bool IsValidForToken(string value)
        {
            return value.Equals(_keyWords[0], System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
