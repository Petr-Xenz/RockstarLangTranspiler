using RockstarLangTranspiler.Tokens;

namespace RockstarLangTranspiler.Expressions
{
    public class ConstantExpression : IExpression
    {
        public float Value { get; }

        public ConstantExpression(float value)
        {
            Value = value;
        }
    }
}
