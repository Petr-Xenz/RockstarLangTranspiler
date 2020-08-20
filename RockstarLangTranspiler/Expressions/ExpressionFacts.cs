using RockstarLangTranspiler.Tokens;

namespace RockstarLangTranspiler.Expressions
{
    public static class ExpressionFacts
    {
        public static bool IsVoidType(this IExpression expression)
        {
            switch (expression)
            {
                case VariableAssigmentExpression _:
                case OutputExpression _: 
                    return true;

                default: return false;
            }
        }
    }

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
    }

}
