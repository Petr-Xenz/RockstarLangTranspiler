using System;
using System.Collections.Generic;

namespace RockstarLangTranspiler.Expressions
{
    public class ElseExpression : IExpression
    {
        public ElseExpression(IEnumerable<IExpression> innerExpressions)
        {
            InnerExpressions = innerExpressions ?? throw new ArgumentNullException(nameof(innerExpressions));
        }

        public IEnumerable<IExpression> InnerExpressions { get; }
    }
}
