using RockstarLangTranspiler.Tokens;
using System;

namespace RockstarLangTranspiler.Expressions
{
    public static class TokenFacts
    {
        public static bool IsCombiningToken(this Token token)
        {
            return token switch
            {
                AdditionToken _ => true,
                _ => false,
            };
        }

        public static bool CanBeArgumentSeparator(this Token token)
        {
            return token switch
            {
                AndToken _ => true,
                CommaToken _ => true,
                FunctionArgumentSeparatorToken _ => true,
                _ => false,
            };
        }
    }

}
