﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler.Expressions
{
    public class IfExpression : IExpression
    {
        public IfExpression(IExpression conditionExpression, IEnumerable<IExpression> innerExpressions, IEnumerable<IExpression> elseExpressions = null)
        {
            ConditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
            InnerExpressions = innerExpressions ?? throw new ArgumentNullException(nameof(innerExpressions));
            ElseExpressions = elseExpressions ?? Enumerable.Empty<IExpression>();

            if (!innerExpressions.Any())
                throw new ArgumentException("If statement must have have inner expressions");
        }

        public IExpression ConditionExpression { get; }

        public IEnumerable<IExpression> InnerExpressions { get; }

        public IEnumerable<IExpression> ElseExpressions { get; }
    }
}
