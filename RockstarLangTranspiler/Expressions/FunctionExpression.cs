using System;
using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Expressions
{
    public class FunctionExpression : IExpression
    {
        public FunctionExpression(IExpression innerExpression, IEnumerable<FunctionArgument> arguments, string name)
        {
            InnerExpression = innerExpression ?? throw new ArgumentNullException(nameof(innerExpression));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IExpression InnerExpression { get; }
        
        public IEnumerable<FunctionArgument> Arguments { get; }

        public string Name { get; }
    }
}
