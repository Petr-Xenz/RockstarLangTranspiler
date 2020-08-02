using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockstarLangTranspiler
{
    public class Parser
    {
        private readonly Token[] _tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens?.ToArray() ?? throw new ArgumentNullException(nameof(tokens));
        }

        public SyntaxTree Parse()
        {
            var rootExpressions = new List<IExpression>();
            for (int current = 0; current < _tokens.Length; current++)
            {
                var (expression, position) = CreateExpressionBranch(current);
                rootExpressions.Add(expression);
                current = position;
            }

            return new SyntaxTree(rootExpressions);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionBranch(int currentTokenPosition)
        {
            throw new NotImplementedException();
        }
    }
}
