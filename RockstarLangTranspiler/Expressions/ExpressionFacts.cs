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

}
