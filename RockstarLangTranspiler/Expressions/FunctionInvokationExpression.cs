using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace RockstarLangTranspiler.Expressions
{
    public class FunctionInvokationExpression : IExpression
    {
        public FunctionInvokationExpression(IEnumerable<IExpression> argumentExpressions, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Function name cannot be empty", nameof(name));
            }

            ArgumentExpressions = argumentExpressions ?? throw new ArgumentNullException(nameof(argumentExpressions));
            Name = name;
        }

        public IEnumerable<IExpression> ArgumentExpressions { get; }

        public string Name { get; }
    }
}
