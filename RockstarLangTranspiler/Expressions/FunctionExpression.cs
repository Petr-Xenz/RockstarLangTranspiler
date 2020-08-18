using System;
using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Expressions
{
    public class FunctionExpression : IExpression
    {
        public FunctionExpression(IEnumerable<IExpression> innerExpressions, IEnumerable<FunctionArgument> arguments, string name)
        {
            InnerExpressions = innerExpressions ?? throw new ArgumentNullException(nameof(innerExpressions));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IEnumerable<IExpression> InnerExpressions { get; }
        
        public IEnumerable<FunctionArgument> Arguments { get; }

        public string Name { get; }
    }
}
