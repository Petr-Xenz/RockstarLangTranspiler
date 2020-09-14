namespace RockstarLangTranspiler.Expressions
{
    public class GreaterThanExpression : CompoundExpression
    {
        public GreaterThanExpression(IExpression left, IExpression right, bool isOrEquals) : base(left, right)
        {
            IsOrEquals = isOrEquals;
        }

        public bool IsOrEquals { get; }
    }
}
