using System;
using System.Collections.Generic;

namespace RockstarLangTranspiler.Expressions
{
    public class WhileExpression : IExpression
    {
        public WhileExpression(IExpression conditionExpression, IEnumerable<IExpression> innerExpressions)
        {
            ConditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
            InnerExpressions = innerExpressions ?? throw new ArgumentNullException(nameof(innerExpressions));
        }

        public IExpression ConditionExpression { get; }

        public IEnumerable<IExpression> InnerExpressions { get; }
    }
}
