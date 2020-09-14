namespace RockstarLangTranspiler.Expressions
{
    public class EqualityExpression : CompoundExpression
    {
        public EqualityExpression(IExpression left, IExpression right) : base(left, right)
        {
        }
    }
}
