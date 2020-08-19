using System;

namespace RockstarLangTranspiler.Tokens
{
    public class FunctionInvokationToken : Token
    {
        public FunctionInvokationToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}