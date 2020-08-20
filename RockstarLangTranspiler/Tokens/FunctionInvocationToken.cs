using System;

namespace RockstarLangTranspiler.Tokens
{
    public class FunctionInvocationToken : Token
    {
        public FunctionInvocationToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}