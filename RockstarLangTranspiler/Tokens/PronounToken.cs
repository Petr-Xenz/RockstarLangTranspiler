using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens
{
    public sealed class PronounToken : Token
    {
        public PronounToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}