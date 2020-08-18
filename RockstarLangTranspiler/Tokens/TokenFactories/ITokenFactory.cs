using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public interface ITokenFactory<out T> where T : Token
    {
        public abstract bool IsValidForToken(string value);

        public abstract bool CanParseFarther(string value);

        public abstract T CreateToken(int startLocation, string value);

        public abstract IReadOnlyCollection<string> KeyWords { get; }
    }
}
