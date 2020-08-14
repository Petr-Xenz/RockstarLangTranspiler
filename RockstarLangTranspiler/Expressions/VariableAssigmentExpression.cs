using System;

namespace RockstarLangTranspiler.Expressions
{
    public class VariableAssigmentExpression : IExpression
    {
        public VariableAssigmentExpression(string variableName, IExpression assigmentExpression)
        {
            VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
            AssigmentExpression = assigmentExpression ?? throw new ArgumentNullException(nameof(assigmentExpression));
        }

        public string VariableName { get; }

        public IExpression AssigmentExpression { get; }
    }

    public class VariableExpression : IExpression
    {
        public string VariableName { get; }

        public VariableExpression(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Creating variable expression without name", nameof(variableName));
            }

            VariableName = variableName;
        }
    }
        
}
