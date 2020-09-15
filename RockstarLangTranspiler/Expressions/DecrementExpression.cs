using System;

namespace RockstarLangTranspiler.Expressions
{
    public class DecrementExpression: IExpression
    {
        public VariableExpression Variable { get; }

        public DecrementExpression(VariableExpression variable)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
        }
    }
}
