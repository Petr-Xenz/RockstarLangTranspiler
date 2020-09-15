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
                case IfExpression _:
                case WhileExpression _:
                case IncrementExpression _:
                case DecrementExpression _:
                    return true;

                default: return false;
            }
        }
    }

}
