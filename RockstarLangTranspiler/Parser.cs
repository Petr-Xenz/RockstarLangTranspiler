using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
                AssigmentToken _ => CreateAssigmentExpression(currentTokenPosition),
                WordToken _ => ParseWordToken(currentTokenPosition),
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

        private (VariableAssigmentExpression, int) CreateAssigmentExpression(int currentTokenPosition)
        {
            var token = _tokens[currentTokenPosition] as AssigmentToken;

            return token.Value.ToLower() switch
            {
                "let" => LetAssigmentBranch(),
                "put" => PutAssigmentBranch(),
                _ => throw new NotSupportedException(token.Value),
            };

            (VariableAssigmentExpression, int) LetAssigmentBranch()
            {
                var expectedVariableToken = PeekNextToken(currentTokenPosition) as WordToken;
                if (expectedVariableToken is null)
                    throw new UnexpectedTokenException();
                var variable = expectedVariableToken.Value;
                var expectedAuxiliaryToken = PeekNextToken(currentTokenPosition + 1) as AssigmentToken;
                if (expectedAuxiliaryToken is null)
                    throw new UnexpectedTokenException();

                var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 3);

                return (new VariableAssigmentExpression(variable, expression),
                    nextTokenPosition);
            }

            (VariableAssigmentExpression, int) PutAssigmentBranch()
            {
                var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1, true);
                var expectedAuxiliaryToken = PeekNextToken(nextTokenPosition - 1) as AssigmentToken;
                if (expectedAuxiliaryToken is null || !expectedAuxiliaryToken.Value.Equals("into", StringComparison.OrdinalIgnoreCase))
                    throw new UnexpectedTokenException();

                var expectedVariableToken = PeekNextToken(nextTokenPosition) as WordToken;
                if (expectedVariableToken is null)
                    throw new UnexpectedTokenException(PeekNextToken(nextTokenPosition).ToString());

                var variable = expectedVariableToken.Value;

                return (new VariableAssigmentExpression(variable, expression),
                    nextTokenPosition + 2);
            }

        }

        private (IExpression expression, int nextTokenPosition) ParseWordToken(int currentTokenPosition)
        {
            var token = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);
            var result = nextToken switch
            {
                AssigmentToken _ => CreateSimpleAssigment(),
                _ => throw new NotSupportedException(),
            };

            return result;

            (IExpression, int) CreateSimpleAssigment()
            {
                var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 2);
                return (new VariableAssigmentExpression(token.Value, expression), nextTokenPosition);
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
