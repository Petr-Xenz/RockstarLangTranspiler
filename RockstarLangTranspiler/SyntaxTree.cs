using RockstarLangTranspiler.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace RockstarLangTranspiler
{
    public class SyntaxTree
    {
        public IEnumerable<IExpression> RootExpressions { get; }

        public SyntaxTree(IEnumerable<IExpression> rootExpressions)
        {
            RootExpressions = rootExpressions ?? throw new System.ArgumentNullException(nameof(rootExpressions));
        }
    }
}