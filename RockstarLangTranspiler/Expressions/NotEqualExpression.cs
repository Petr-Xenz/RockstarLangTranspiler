namespace RockstarLangTranspiler.Expressions
{
    public class NotEqualExpression : CompoundExpression
    {
        public NotEqualExpression(IExpression left, IExpression right) : base(left, right)
        {
        }
    }
}
