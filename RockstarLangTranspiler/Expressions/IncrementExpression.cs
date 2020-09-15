using System;

namespace RockstarLangTranspiler.Expressions
{
    public class IncrementExpression : IExpression
    {
        public IncrementExpression(VariableExpression variable)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
        }

        public VariableExpression Variable { get; }
    }
}
