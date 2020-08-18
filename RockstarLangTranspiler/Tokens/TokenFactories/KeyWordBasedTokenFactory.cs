using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public abstract class KeyWordBasedTokenFactory<T> : ITokenFactory<T> where T : Token
    {
        public abstract IReadOnlyCollection<string> KeyWords { get; }

        public abstract bool CanParseFarther(string value);

        public abstract T CreateToken(int startLocation, string value);

        public bool IsValidForToken(string value) 
            => KeyWords.Any(w => w.Equals(value, System.StringComparison.OrdinalIgnoreCase));
    }
}
