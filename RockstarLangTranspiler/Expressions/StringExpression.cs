using System;

namespace RockstarLangTranspiler.Expressions
{
    public class StringExpression : IExpression
    {
        public StringExpression(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}
