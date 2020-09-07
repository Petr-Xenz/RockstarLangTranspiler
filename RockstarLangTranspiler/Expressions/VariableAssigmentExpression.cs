using System;

namespace RockstarLangTranspiler.Expressions
{
    public class VariableAssigmentExpression : IExpression
    {
        public VariableAssigmentExpression(VariableExpression variable, IExpression assigmentExpression)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            AssigmentExpression = assigmentExpression ?? throw new ArgumentNullException(nameof(assigmentExpression));
        }

        public VariableExpression Variable { get; }

        public IExpression AssigmentExpression { get; }
    }
        
}
