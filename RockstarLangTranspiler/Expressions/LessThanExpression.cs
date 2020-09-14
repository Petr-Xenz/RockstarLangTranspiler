namespace RockstarLangTranspiler.Expressions
{
    public class LessThanExpression : CompoundExpression
    {
        public LessThanExpression(IExpression left, IExpression right, bool isOrEquals) : base(left, right)
        {
            IsOrEquals = isOrEquals;
        }

        public bool IsOrEquals { get; }
    }
}
