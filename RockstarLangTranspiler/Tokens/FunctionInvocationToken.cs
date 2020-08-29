using System;

namespace RockstarLangTranspiler.Tokens
{
    public class FunctionInvocationToken : Token
    {
        public FunctionInvocationToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}