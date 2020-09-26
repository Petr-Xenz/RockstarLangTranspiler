namespace RockstarLangTranspiler.Expressions
{
    public class ConjunctionExpression : CompoundExpression
    {
        public ConjunctionExpression(IExpression left, IExpression right) : base(left, right)
        {
        }
    }

}
