using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public abstract class TokenFactory
    {
        public abstract bool IsValidForToken(string value);

        public abstract bool CanParseFarther(string value);

        public abstract Token CreateToken(int startLocation, string value);

        public abstract IReadOnlyCollection<string> KeyWords { get; }
    }
}
