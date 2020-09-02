using System;

namespace RockstarLangTranspiler.Expressions
{
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
