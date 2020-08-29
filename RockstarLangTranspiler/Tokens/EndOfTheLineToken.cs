using System;

namespace RockstarLangTranspiler.Tokens
{
    public sealed class EndOfTheLineToken : Token
    {
        public EndOfTheLineToken(int linePosition, int lineNumber) : base (linePosition, lineNumber, Environment.NewLine)
        {

        }
    }
}