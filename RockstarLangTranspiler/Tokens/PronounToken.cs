using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens
{
    public sealed class PronounToken : Token
    {
        public PronounToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}