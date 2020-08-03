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
            int current = 0;
            while(current < _tokens.Length)
            {
                var (expression, position) = CreateExpressionBranch(current);
                if (expression is null)
                    break;
                rootExpressions.Add(expression);
                current = position;
            }

            return new SyntaxTree(rootExpressions);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionBranch(int currentTokenPosition)
        {
            if (currentTokenPosition < 0 || currentTokenPosition > _tokens.Length)
                return (null, -1);
            var token = _tokens[currentTokenPosition];

            return token switch
            {
                NumberToken number => (new ConstantExpression(float.Parse(number.Value)), currentTokenPosition + 1),
                AdditionToken _ => CreateAdditionExpression(currentTokenPosition),
                OutputToken _ => CreateOutputExpression(currentTokenPosition),
                _ => throw new ArgumentException(),
            };

            (AdditionExpression, int) CreateAdditionExpression(int currentTokenPosition)
            {
                return (new AdditionExpression(CreateExpressionBranch(currentTokenPosition - 1).expression, 
                    CreateExpressionBranch(currentTokenPosition + 1).expression),
                    currentTokenPosition + 2);
            }

            (OutputExpression, int) CreateOutputExpression(int currentTokenPosition)
            {
                var nextExpression = CreateExpressionBranch(currentTokenPosition + 1);
                return (new OutputExpression(nextExpression.expression), nextExpression.nextTokenPosition);
            }
        }
    }
}
