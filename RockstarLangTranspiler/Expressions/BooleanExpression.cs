namespace RockstarLangTranspiler.Expressions
{
    public class BooleanExpression : IExpression
    {
        public bool Value { get; }

        public BooleanExpression(bool value)
        {
            Value = value;
        }
    }
}
