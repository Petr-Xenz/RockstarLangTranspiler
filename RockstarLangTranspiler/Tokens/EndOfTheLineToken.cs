using System;

namespace RockstarLangTranspiler.Tokens
{
    public sealed class EndOfTheLineToken : Token
    {
        public EndOfTheLineToken(int startLocation) : base (startLocation, Environment.NewLine.Length, Environment.NewLine)
        {

        }
    }
}