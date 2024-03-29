﻿using RockstarLangTranspiler.Tokens;
using System;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Expressions
{
    public static class TokenFacts
    {
        public static bool IsCombiningToken(this Token token)
        {
            return token switch
            {
                CombiningToken _ => true,
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

        public static bool EndsBackTracking(this Token token)
        {
            return token switch
            {
                EndOfTheLineToken _ => true,
                EndOfFileToken _ => true,
                AssigmentToken _ => true,
                _ => false,
            };
        }

        public static bool BooleanValue(this BooleanToken token)
        {
            return token.Value switch
            {
                Right => true,
                Ok => true,
                Yes => true,
                Wrong => false,
                No => false,
                Lies => false,
                _ => throw new NotSupportedException("unknown boolean literal"),
            };
        }
    }

}
