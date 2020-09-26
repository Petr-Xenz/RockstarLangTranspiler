namespace RockstarLangTranspiler.Expressions
{
    public class DisjunctionExpression : CompoundExpression
    {
        public DisjunctionExpression(IExpression left, IExpression right) : base(left, right)
        {
        }
    }

}
