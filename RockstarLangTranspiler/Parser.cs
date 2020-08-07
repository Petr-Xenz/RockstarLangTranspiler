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
        private readonly Dictionary<int, IExpression> _tokenPositionToExpression = new Dictionary<int, IExpression>(); 

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens?.ToArray() ?? throw new ArgumentNullException(nameof(tokens));
        }

        public SyntaxTree Parse()
        {
            var rootExpressions = new List<IExpression>();
            _tokenPositionToExpression.Clear();
            int current = 0;
            while(current < _tokens.Length)
            {
                var (expression, position) = CreateExpressionBranch(current);
                current = position;
                if (expression is null)
                    continue;
                rootExpressions.Add(expression);
            }

            return new SyntaxTree(rootExpressions);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionBranch(int currentTokenPosition, bool isBackTracking = false)
        {
            if (currentTokenPosition < 0 || currentTokenPosition > _tokens.Length)
                return (null, -1);
            var token = _tokens[currentTokenPosition];

            (IExpression expression, int nextTokenPosition) expression = token switch
            {
                NumberToken number => CreateConstantExpression(number, currentTokenPosition, isBackTracking),
                AdditionToken _ => CreateAdditionExpression(currentTokenPosition),
                OutputToken _ => CreateOutputExpression(currentTokenPosition),
                EndOfTheLineToken _ => (null, currentTokenPosition + 1),
                _ => throw new ArgumentException(),
            };

            _tokenPositionToExpression[currentTokenPosition] = expression.expression;

            return expression;

            (AdditionExpression, int) CreateAdditionExpression(int currentTokenPosition)
            {
                return (new AdditionExpression(CreateExpressionBranch(currentTokenPosition - 1, true).expression, 
                    CreateExpressionBranch(currentTokenPosition + 1).expression),
                    currentTokenPosition + 2);
            }

            (OutputExpression, int) CreateOutputExpression(int currentTokenPosition)
            {
                var nextExpression = CreateExpressionBranch(currentTokenPosition + 1);
                return (new OutputExpression(nextExpression.expression), nextExpression.nextTokenPosition);
            }

            (IExpression, int) CreateConstantExpression(NumberToken number, int currentTokenPosition, bool isBackTracking)
            {
                var nextToken = PeekNextToken(currentTokenPosition);
                if (isBackTracking || nextToken is EndOfFileToken || nextToken is EndOfTheLineToken)
                    return (new ConstantExpression(float.Parse(number.Value)), currentTokenPosition + 1);
                else 
                    return CreateExpressionBranch(currentTokenPosition + 1);
            }
        }

        private Token PeekNextToken(int currentTokenPosition)
        {
            return currentTokenPosition + 1 == _tokens.Length 
                ? new EndOfFileToken(currentTokenPosition + 1) 
                : _tokens[currentTokenPosition + 1];
        }
    }
}
