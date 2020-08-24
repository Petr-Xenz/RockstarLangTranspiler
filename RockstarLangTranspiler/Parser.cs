﻿using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Schema;

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
            while(current < _tokens.Length && current >= 0)
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
            if (currentTokenPosition < 0 || currentTokenPosition > _tokens.Length || _tokens[currentTokenPosition] is EndOfFileToken)
                return (null, -1);
            var token = _tokens[currentTokenPosition];

            (IExpression expression, int nextTokenPosition) expression = token switch
            {
                NumberToken number => CreateConstantExpression(number, currentTokenPosition, isBackTracking),
                AdditionToken _ => CreateAdditionExpression(currentTokenPosition),
                OutputToken _ => CreateOutputExpression(currentTokenPosition),
                AssigmentToken _ => CreateAssigmentExpression(currentTokenPosition),
                IfToken _ => CreateConditionExpression(currentTokenPosition),
                AndToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                CommaToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                FunctionArgumentSeparatorToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                WordToken _ => ParseWordToken(currentTokenPosition, isBackTracking),
                EndOfTheLineToken _ => (null, currentTokenPosition + 1),
                _ => throw new ArgumentException(),
            };

            _tokenPositionToExpression[currentTokenPosition] = expression.expression;

            return expression;

            (AdditionExpression, int) CreateAdditionExpression(int currentTokenPosition)
            {
                var (left, _) = CreateExpressionBranch(currentTokenPosition - 1, true);
                var (right, next) = CreateExpressionBranch(currentTokenPosition + 1);
                return (new AdditionExpression(left, right), next);
            }

            (OutputExpression, int) CreateOutputExpression(int currentTokenPosition)
            {
                var nextExpression = CreateExpressionBranch(currentTokenPosition + 1);
                return (new OutputExpression(nextExpression.expression), nextExpression.nextTokenPosition);
            }

            (IExpression, int) CreateConstantExpression(NumberToken number, int currentTokenPosition, bool isBackTracking)
            {
                var nextToken = PeekNextToken(currentTokenPosition);
                if (isBackTracking || !nextToken.IsCombiningToken())
                    return (new ConstantExpression(float.Parse(number.Value, CultureInfo.InvariantCulture)), currentTokenPosition + 1);
                else 
                      return CreateExpressionBranch(currentTokenPosition + 1);
            }
        }

        private (IfExpression, int) CreateConditionExpression(int currentTokenPosition)
        {
            var (conditionExpression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1);
            if (conditionExpression.IsVoidType())
                throw new InvalidOperationException("Condition expression must have return value");

            nextTokenPosition = GetNextLinePosition(nextTokenPosition);
            var nextToken = _tokens[nextTokenPosition];

            var inners = new List<IExpression>();
            while(!(nextToken is EndOfFileToken || nextToken is ElseToken || _tokens[nextTokenPosition + 1] is EndOfTheLineToken))
            {
                var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                if (inner != null)
                    inners.Add(inner);
                nextToken = _tokens[nextInner];
                nextTokenPosition = nextInner;
            }

            var elseExpressions = new List<IExpression>();
            if (nextToken is ElseToken)
            {
                nextTokenPosition++;
                while (!(nextToken is EndOfFileToken ||  _tokens[nextTokenPosition + 1] is EndOfTheLineToken))
                {
                    var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                    if (inner != null)
                        elseExpressions.Add(inner);
                    nextToken = _tokens[nextInner];
                    nextTokenPosition = nextInner;
                }
            }

            return (new IfExpression(conditionExpression, inners, elseExpressions), nextTokenPosition);
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
                var expectedVariableToken = PeekNextToken(currentTokenPosition) as WordToken 
                    ?? throw new UnexpectedTokenException();
                var variable = expectedVariableToken.Value;
                var expectedAuxiliaryToken = PeekNextToken(currentTokenPosition + 1) as AssigmentToken 
                    ?? throw new UnexpectedTokenException();

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

                var expectedVariableToken = PeekNextToken(nextTokenPosition) as WordToken
                    ?? throw new UnexpectedTokenException(PeekNextToken(nextTokenPosition).ToString());

                var variable = expectedVariableToken.Value;

                return (new VariableAssigmentExpression(variable, expression),
                    nextTokenPosition + 2);
            }
        }

        private (IExpression expression, int nextTokenPosition) ParseWordToken(int currentTokenPosition, bool isBackTracking = false)
        {
            var token = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);
            var result = (nextToken, isBackTracking) switch
            {
                (AssigmentToken _, _) => CreateSimpleAssigment(),
                (FunctionDeclarationToken _, _) => CreateFunctionExpression(currentTokenPosition),
                (FunctionInvocationToken _, _) => CreateFunctionInvocationExpression(currentTokenPosition),
                (AdditionToken _, false) => CreateExpressionBranch(currentTokenPosition + 1, true),

                (AdditionToken _, true) => CreateSimpleVariableExpression(),
                (EndOfTheLineToken _, _) => CreateSimpleVariableExpression(),
                (EndOfFileToken _, _) => CreateSimpleVariableExpression(),
                (AndToken _, _) => CreateSimpleVariableExpression(),
                (CommaToken _, _) => CreateSimpleVariableExpression(),
                (FunctionArgumentSeparatorToken _, _) => CreateSimpleVariableExpression(),
                _ => throw new NotSupportedException(),
            };

            return result;

            (IExpression, int) CreateSimpleVariableExpression()
            {
                return (new VariableExpression(token.Value), currentTokenPosition + 1);
            }

            (IExpression, int) CreateSimpleAssigment()
            {
                var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 2);
                return (new VariableAssigmentExpression(token.Value, expression), nextTokenPosition);
            }
        }

        private (IExpression, int) CreateFunctionInvocationExpression(int currentTokenPosition)
        {
            var functionName = _tokens[currentTokenPosition].Value;
            var (arguments, nextTokenPosition) = SelectArgumentExpressionsFromLine(currentTokenPosition + 2);

            return (new FunctionInvocationExpression(arguments, functionName), nextTokenPosition);

            (IEnumerable<IExpression> expression, int nextTokenPosition) SelectArgumentExpressionsFromLine(int tokenPosition)
            {
                var result = new List<IExpression>();
                var nextToken = _tokens[tokenPosition];
                while(CanParseArgumentsFarther(nextToken))
                {
                    var (argumentExpression, nextTokenPosition) = CreateExpressionBranch(tokenPosition);
                    tokenPosition = nextTokenPosition;
                    nextToken = _tokens[tokenPosition];
                    if (argumentExpression is null)
                        break;
                    result.Add(argumentExpression);
                }

                return (result, tokenPosition);

                static bool CanParseArgumentsFarther(Token token)
                {
                    return token switch
                    {
                        AssigmentToken _ => false,
                        EndOfTheLineToken _ => false,
                        EndOfFileToken _ => false,
                        _ => true,
                    };
                }
            }
        }

        private (IExpression, int) CreateFunctionExpression(int currentTokenPosition)
        {
            var functionName = _tokens[currentTokenPosition].Value;
            var arguments = SelectArgumentsFromLine(currentTokenPosition + 2);
            var nextLinePosition = GetNextLinePosition(currentTokenPosition);

            var innerExpressions = new List<IExpression>();

            while(!(_tokens[nextLinePosition] is EndOfFileToken))
            {
                if (_tokens[nextLinePosition] is FunctionReturnToken)
                {
                    var (returnExpression, nextTokenPosition) = CreateExpressionBranch(nextLinePosition + 2);
                    innerExpressions.Add(returnExpression);
                    return (new FunctionExpression(innerExpressions, arguments, functionName), nextTokenPosition);
                }
                else
                {
                    var (functionExpression, _) = CreateExpressionBranch(nextLinePosition);
                    innerExpressions.Add(functionExpression);
                    nextLinePosition = GetNextLinePosition(nextLinePosition);
                }
            }

            throw new InvalidOperationException("Function does not return");

            IEnumerable<FunctionArgument> SelectArgumentsFromLine(int position)
            {
                while (_tokens.Length < position || !(_tokens[position] is EndOfTheLineToken))
                {
                    var token = _tokens[position];
                    position++;
                    if (token is WordToken)
                    {
                        yield return new FunctionArgument(token.Value);
                    }
                }
            }
        }        

        private int GetNextLinePosition(int currentTokenPosition)
        {
            while(currentTokenPosition < _tokens.Length)
            {
                if (_tokens[currentTokenPosition] is EndOfTheLineToken)
                    return currentTokenPosition + 1;
                else if (PeekNextToken(currentTokenPosition) is EndOfTheLineToken)
                    return currentTokenPosition + 2;
                else
                    currentTokenPosition++;
            }

            return currentTokenPosition;
        }

        private Token PeekNextToken(int currentTokenPosition)
        {
            return currentTokenPosition + 1 == _tokens.Length 
                ? new EndOfFileToken(currentTokenPosition + 1) 
                : _tokens[currentTokenPosition + 1];
        }
    }
}
