﻿using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler
{
    public class Parser
    {
        private readonly Token[] _tokens;
        private readonly Dictionary<(int linePosition, int lineNumber), IExpression> _tokenPositionToExpression = new Dictionary<(int, int), IExpression>();
        private readonly Stack<IList<IExpression>> _expressionsByDepth = new Stack<IList<IExpression>>();

        private bool _isInConditionParsingContext;
        private bool _isInFunctionArgumentsContext;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens?.ToArray() ?? throw new ArgumentNullException(nameof(tokens));
        }

        public SyntaxTree Parse()
        {
            var rootExpressions = new List<IExpression>();
            _expressionsByDepth.Push(rootExpressions);
            _tokenPositionToExpression.Clear();
            int current = 0;
            while (current < _tokens.Length && current >= 0)
            {
                var (expression, position) = CreateExpressionBranch(current);
                current = position;
                if (expression is null)
                    continue;
                rootExpressions.Add(expression);
            }

            return new SyntaxTree(rootExpressions);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionBranch(int currentTokenPosition)
        {
            if (currentTokenPosition < 0 || currentTokenPosition > _tokens.Length || _tokens[currentTokenPosition] is EndOfFileToken)
                return (null, -1);
            var token = _tokens[currentTokenPosition];

            (IExpression expression, int nextTokenPosition) expression = token switch
            {
                NullToken _ => (new NullExpression(), ++currentTokenPosition),
                UndefinedToken _ => (new UndefinedExpression(), ++currentTokenPosition),
                NumberToken number => CreateConstantExpression(number, currentTokenPosition),
                BooleanToken boolean => CreateBooleanExpression(boolean, currentTokenPosition),
                AdditionToken _ => CreateCompoundExpression((l, r) => new AdditionExpression(l, r), currentTokenPosition),
                SubtractionToken _ => CreateCompoundExpression((l, r) => new SubtractionExpression(l, r), currentTokenPosition),
                MultiplicationToken _ => CreateCompoundExpression((l, r) => new MultiplicationExpression(l, r), currentTokenPosition),
                DivisionToken _ => CreateCompoundExpression((l, r) => new DivisionExpression(l, r), currentTokenPosition),
                IncrementToken { IsAuxiliary: false } _ => CreateIncrementExpression(currentTokenPosition),
                DecrementToken { IsAuxiliary: false } _ => CreateDecrementExpression(currentTokenPosition),
                OutputToken _ => CreateOutputExpression(currentTokenPosition),
                AssigmentToken _ => CreateAssigmentExpression(currentTokenPosition),
                IfToken _ => CreateConditionExpression(currentTokenPosition),
                WhileToken _ => CreateWhileExpression(currentTokenPosition),
                ContinueToken _ => CreateContinueExpressions(currentTokenPosition),
                BreakToken _ => CreateBreakExpression(currentTokenPosition),
                IsToken _ => ParseExpressionBasedOnState(currentTokenPosition),
                NotEqualsToken _ => CreateCompoundExpression((l, r) => new NotEqualExpression(l, r), currentTokenPosition),
                AndToken _ => CreateCompoundExpression((l, r) => new ConjunctionExpression(l, r), currentTokenPosition),
                DisjunctionToken _ => CreateCompoundExpression((l, r) => new DisjunctionExpression(l, r), currentTokenPosition),
                JointDenialToken _ => CreateCompoundExpression((l, r) => new JointDenialExpression(l, r), currentTokenPosition),
                CommonVariablePrefixToken _ => ParseCommonVariable(currentTokenPosition),
                CommentToken { IsCommentStart: true} _ => SkipComment(currentTokenPosition + 1),
                QuoteToken _ => CreateStringExpression(currentTokenPosition),
                WordToken _ => ParseWordToken(currentTokenPosition),
                EndOfTheLineToken _ => (null, currentTokenPosition + 1),
                _ => throw new ArgumentException(),
            };

            _tokenPositionToExpression[(token.LinePosition, token.LineNumber)] = expression.expression;

            return expression;
        }

        private (BreakExpression expression, int nextTokenPosition) CreateBreakExpression(int currentTokenPosition)
        {
            if (PeekNextToken(currentTokenPosition) is EndOfTheLineToken)                
            {
                return (new BreakExpression(), currentTokenPosition + 1);
            }

            var secondToken = PeekNextToken(currentTokenPosition);
            var thirdToken = PeekNextToken(currentTokenPosition + 1);

            if(secondToken.Value.Equals(It, StringComparison.OrdinalIgnoreCase)
                && thirdToken.Value.Equals(Down, StringComparison.OrdinalIgnoreCase)
                && PeekNextToken(currentTokenPosition + 2) is EndOfTheLineToken)
            {
                return (new BreakExpression(), currentTokenPosition + 3);
            }

            throw new UnexpectedTokenException();
        }

        private (ContinueExpression expression, int nextTokenPosition) CreateContinueExpressions(int currentTokenPosition)
        {
            var continueToken = (ContinueToken)_tokens[currentTokenPosition];
            if (continueToken.Value.Equals(Continue, StringComparison.OrdinalIgnoreCase)
                && PeekNextToken(currentTokenPosition) is EndOfTheLineToken)
            {
                return (new ContinueExpression(), currentTokenPosition + 1);
            }

            var secondToken = PeekNextToken(currentTokenPosition);
            var thirdToken = PeekNextToken(currentTokenPosition + 1);
            var forthToken = PeekNextToken(currentTokenPosition + 2);
            var fithToken = PeekNextToken(currentTokenPosition + 3);

            if (secondToken.Value.Equals(It, StringComparison.OrdinalIgnoreCase)
                && thirdToken.Value.Equals("to", StringComparison.OrdinalIgnoreCase)
                && forthToken.Value.Equals(The, StringComparison.OrdinalIgnoreCase)
                && fithToken.Value.Equals("top", StringComparison.OrdinalIgnoreCase)
                && PeekNextToken(currentTokenPosition + 4) is EndOfTheLineToken)
            {
                return (new ContinueExpression(), currentTokenPosition + 5);
            }

            throw new UnexpectedTokenException();
        }

        private (DecrementExpression expression, int nextTokenPosition) CreateDecrementExpression(int currentTokenPosition)
        {
            var (variable, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1); ;

            if (variable is VariableExpression ve
                && PeekNextToken(nextTokenPosition - 1) is DecrementToken ie
                && ie.IsAuxiliary)
            {
                return (new DecrementExpression(ve), nextTokenPosition + 1);
            }

            throw new UnexpectedTokenException();
        }

        private (IncrementExpression expression, int nextTokenPosition) CreateIncrementExpression(int currentTokenPosition)
        {
            var (variable, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1); ;

            if (variable is VariableExpression ve 
                && PeekNextToken(nextTokenPosition - 1) is IncrementToken ie 
                && ie.IsAuxiliary)
            {
                return (new IncrementExpression(ve), nextTokenPosition + 1);
            }

            throw new UnexpectedTokenException();
        }

        private (StringExpression expression, int nextTokenPosition) CreateStringExpression(int currentTokenPosition)
        {
            var nextTokenPosition = currentTokenPosition + 1;
            while (_tokens[nextTokenPosition] is not EndOfTheLineToken)
            {
                if (_tokens[nextTokenPosition] is QuoteToken)
                {                    
                    var builder = new StringBuilder();
                    for (int i = currentTokenPosition + 1; i < nextTokenPosition; i++)
                    {
                        builder.Append(_tokens[i].Value);
                        builder.Append(' ');
                    }

                    return (new StringExpression(builder.ToString().TrimEnd()), nextTokenPosition + 1);
                }

                nextTokenPosition++;
            }

            throw new UnexpectedTokenException("String is not closed");
        }

        private (IExpression, int) SkipComment(int currentTokenPosition)
        {
            while(_tokens[currentTokenPosition] is not EndOfTheLineToken)
            {
                if (_tokens[currentTokenPosition] is CommentToken t && !t.IsCommentStart)
                    return CreateExpressionBranch(currentTokenPosition + 1);

                currentTokenPosition++;
            }

            throw new UnexpectedTokenException("Comment is not closed");
        }

        private (IExpression, int) CreateBooleanExpression(BooleanToken boolean, int currentTokenPosition)
            => (new BooleanExpression(boolean.BooleanValue()), currentTokenPosition + 1);

        private (IExpression, int) CreateConstantExpression(NumberToken number, int currentTokenPosition)
            => (new ConstantExpression(float.Parse(number.Value, CultureInfo.InvariantCulture)), currentTokenPosition + 1);

        private (OutputExpression, int) CreateOutputExpression(int currentTokenPosition)
        {
            var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1);
            return (new OutputExpression(expression), nextTokenPosition);
        }

        private (T, int) CreateCompoundExpression<T>(Func<IExpression, IExpression, T> ctor, int currentTokenPosition)
        {
            var left = PopLastExpressionFromCurrentTreeLevel();
            var (right, next) = CreateExpressionWithBacktracking(currentTokenPosition + 1);
            return (ctor(left, right), next);
        }

        private (WhileExpression, int) CreateWhileExpression(int currentTokenPosition)
        {
            var (conditionExpression, nextTokenPosition) = CreateExpressionWithConditionState(currentTokenPosition + 1, _isInConditionParsingContext);

            if (conditionExpression.IsVoidType())
                throw new InvalidOperationException("Condition expression must have return value");

            var nextToken = _tokens[nextTokenPosition];

            var inners = new List<IExpression>();
            _expressionsByDepth.Push(inners);
            while (!(nextToken is EndOfFileToken))           
            {
                var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                if (inner is not null)
                    inners.Add(inner);
                nextToken = PeekNextToken(nextInner);
                nextTokenPosition = nextInner;

                if (IsEndOfBlock(nextTokenPosition))
                {
                    nextTokenPosition++;
                    break;
                }
            }

            _expressionsByDepth.Pop();
            return (new WhileExpression(conditionExpression, inners), nextTokenPosition);
        }

        private (IfExpression, int) CreateConditionExpression(int currentTokenPosition)
        {
            var (conditionExpression, nextTokenPosition) = CreateExpressionWithConditionState(currentTokenPosition + 1, _isInConditionParsingContext);

            if (conditionExpression.IsVoidType())
                throw new InvalidOperationException("Condition expression must have return value");

            var nextToken = _tokens[nextTokenPosition];

            var inners = new List<IExpression>();
            _expressionsByDepth.Push(inners);
            while (!(nextToken is EndOfFileToken || nextToken is ElseToken))
            {
                var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                if (inner is not null)
                    inners.Add(inner);
                nextToken = PeekNextToken(nextInner);
                nextTokenPosition = nextInner;

                if (IsEndOfBlock(nextTokenPosition))
                {
                    nextTokenPosition++;
                    break;
                }
            }
            _expressionsByDepth.Pop();

            var elseExpressions = new List<IExpression>();
            _expressionsByDepth.Push(elseExpressions);
            if (nextToken is ElseToken)
            {
                nextTokenPosition += 2;
                while (!(nextToken is EndOfFileToken))
                {
                    var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                    if (inner is not null)
                        elseExpressions.Add(inner);
                    nextToken = PeekNextToken(nextInner);
                    nextTokenPosition = nextInner;

                    if (IsEndOfBlock(nextTokenPosition))
                    {
                        nextTokenPosition++;
                        break;
                    }
                }
            }
            _expressionsByDepth.Pop();

            return (new IfExpression(conditionExpression, inners, elseExpressions), nextTokenPosition);
        }

        private bool IsEndOfBlock(int currentTokenPosition)
        {
            var currentToken = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);

            return nextToken is EndOfTheLineToken && currentToken is EndOfTheLineToken;
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionWithBacktracking(int currentTokenPosition)
        {
            var expressions = new List<IExpression>();
            var nextTokenPosition = currentTokenPosition;
            IExpression expression = null;
            _expressionsByDepth.Push(expressions);
            do
            {
                (expression, nextTokenPosition) = CreateExpressionBranch(nextTokenPosition);
                if (expression is not null)
                {
                    expressions.Add(expression);
                }
                else
                {
                    expression = expressions.Last();
                }
            }
            while (!PeekToken(nextTokenPosition).EndsBackTracking());            
            
            _expressionsByDepth.Pop();
            Debug.Assert(expressions.Count == 1);

            return (expression, nextTokenPosition);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionWithConditionState(int currentTokenPosition, bool currentContextState)
        {
            try
            {
                _isInConditionParsingContext = true;
                return CreateExpressionWithBacktracking(currentTokenPosition);
            }
            finally
            {
                _isInConditionParsingContext = currentContextState;
            }
        }

        private (VariableAssigmentExpression, int) CreateAssigmentExpression(int currentTokenPosition)
        {
            var token = _tokens[currentTokenPosition] as AssigmentToken;

            return token.Value.ToLower() switch
            {
                Let => LetAssigmentBranch(),
                Put => PutAssigmentBranch(),
                _ => throw new NotSupportedException(token.Value),
            };

            (VariableAssigmentExpression, int) LetAssigmentBranch()
            {
                try
                {
                    _isInConditionParsingContext = true;
                    var variable = CreateVaraibleExpression(currentTokenPosition + 1);

                    var (expression, nextTokenPosition) = CreateExpressionWithBacktracking(variable.nextTokenPosition+ 1);

                    return (new VariableAssigmentExpression(variable.expression, expression),
                        nextTokenPosition);
                }
                finally
                {
                    _isInConditionParsingContext = false;
                }
            }

            (VariableAssigmentExpression, int) PutAssigmentBranch()
            {
                var (expression, nextTokenPosition) = CreateExpressionWithBacktracking(currentTokenPosition + 1);
                var expectedAuxiliaryToken = PeekToken(nextTokenPosition) as AssigmentToken;
                if (expectedAuxiliaryToken is null || !expectedAuxiliaryToken.Value.Equals(Into, StringComparison.OrdinalIgnoreCase))
                    throw new UnexpectedTokenException();

                var variable = CreateVaraibleExpression(nextTokenPosition + 1);

                return (new VariableAssigmentExpression(variable.expression, expression),
                    variable.nextTokenPosition);
            }
        }

        private (IExpression expression, int nextTokenPosition) ParseWordToken(int currentTokenPosition)
        {
            var nextToken = PeekNextToken(currentTokenPosition);
            var previousToken = PeekPreviousToken(currentTokenPosition);

            var result = (nextToken, previousToken) switch
            {
                (IsToken _, _) => ParseExpressionBasedOnState(currentTokenPosition),
                (FunctionDeclarationToken _, _) => CreateFunctionExpression(currentTokenPosition),
                (FunctionInvocationToken _, _) => CreateFunctionInvocationExpression(currentTokenPosition),
                
                (WordToken { IsStartingWithUpper: true } _, _) => CreateVaraibleExpression(currentTokenPosition),
                (WordToken { IsStartingWithUpper: false } _, _) => CreatePoeticLiteralExpression(currentTokenPosition),
                (EndOfTheLineToken _, IsToken _) => CreatePoeticLiteralExpression(currentTokenPosition),
                _ => CreateSimpleVariableExpression(currentTokenPosition),
            };

            return result;
        }

        private (IExpression expression, int nextTokenPosition) ParseExpressionBasedOnState(int currentTokenPosition)
        {
            return (_isInConditionParsingContext, _isInFunctionArgumentsContext) switch
            {
                (true, false) => CreateComparsionExpression(currentTokenPosition),
                (false, true) => CreateSimpleVariableExpression(currentTokenPosition),
                (true, true) => throw new UnexpectedTokenException(),
                _ => CreateSimpleAssigment(CreateSimpleVariableExpression(currentTokenPosition).expression, currentTokenPosition + 1),
            };
        }

        private (IExpression expression, int nextTokenPosition) CreateComparsionExpression(int currentTokenPosition)
        {
            var left = PopLastExpressionFromCurrentTreeLevel();
            var (right, nextTokenPosition) = GetNextExpression(currentTokenPosition + 1);

            var nextToken = PeekNextToken(currentTokenPosition);
            var secondToken = PeekNextToken(currentTokenPosition + 1);
            var thirdToken = PeekNextToken(currentTokenPosition + 2);

            return (nextToken, secondToken, thirdToken) switch
            {               
                (ComparsionToken { Value: As } _, ComparsionToken { IsHigherOrEquals: true } _, ComparsionToken { Value: As } _) => (new GreaterThanExpression(left, right, true), nextTokenPosition),
                (ComparsionToken { Value: As } _, ComparsionToken { IsLowerOrEquals: true } _, ComparsionToken { Value: As } _) => (new LessThanExpression(left, right, true), nextTokenPosition),
                (ComparsionToken { IsHigher: true } _, ComparsionToken { Value: Than } _, _) => (new GreaterThanExpression(left, right, false), nextTokenPosition),
                (ComparsionToken { IsLower: true } _, ComparsionToken { Value: Than } _, _) => (new LessThanExpression(left, right, false), nextTokenPosition),
                (NotToken _, _, _) => (new NotEqualExpression(left, right), nextTokenPosition),
                _ => (new EqualityExpression(left, right), nextTokenPosition),
            };

            (IExpression expression, int nextTokenPosition) GetNextExpression(int currentTokenPosition)
            {
                while (PeekNextToken(currentTokenPosition) is ComparsionToken 
                    || _tokens[currentTokenPosition] is NotToken
                    || _tokens[currentTokenPosition] is ComparsionToken)
                {
                    currentTokenPosition++;
                }

                return CreateExpressionBranch(currentTokenPosition);
            }
        }

        private (IExpression expression, int nextTokenPosition) CreatePoeticLiteralExpression(int currentTokenPosition)
        {
            if (_tokens[currentTokenPosition - 1] is not IsToken)
                throw new UnexpectedTokenException("Poetic constant literal parsed in unexpected position");

            var lastPoeticLiteralPosition = GetNextLinePosition(currentTokenPosition) - 2;
            var poeticWords = _tokens.AsSpan(currentTokenPosition, lastPoeticLiteralPosition - currentTokenPosition + 1);
            double number = 0;
            for (var i = 0; i < poeticWords.Length; i++)
            {
                number += poeticWords[i].Length % 10 * Math.Pow(10, poeticWords.Length - i - 1);
            }

            return (new ConstantExpression((float)number), lastPoeticLiteralPosition + 1);
        }

        private (IExpression expression, int nextTokenPosition) CreateSimpleAssigment(VariableExpression variableExpression, int assigmentTokenPosition)
        {
            var (expression, nextTokenPosition) = CreateExpressionWithBacktracking(assigmentTokenPosition + 1);
            return (new VariableAssigmentExpression(variableExpression, expression), nextTokenPosition);
        }

        private (VariableExpression expression, int nextTokenPosition) CreateSimpleVariableExpression(int currentTokenPosition)
            => (new VariableExpression(PeekToken(currentTokenPosition).Value), currentTokenPosition + 1);

        private (VariableExpression expression, int nextTokenPosition) CreateVaraibleExpression(int currentTokenPosition)
        {
            var currentToken = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);

            return (currentToken, nextToken) switch
            {
                (CommonVariablePrefixToken _, WordToken _) => CreateCommonVariable(currentTokenPosition),
                (WordToken { IsStartingWithUpper: true } _, WordToken { IsStartingWithUpper: true } _) => CreateProperVariable(currentTokenPosition),
                (WordToken _, _) => CreateSimpleVariableExpression(currentTokenPosition),
                _ => throw new UnexpectedTokenException(currentToken.GetType().ToString()),
            };
        }

        private (VariableExpression expression, int nextTokenPosition) CreateProperVariable(int currentTokenPosition)
        {
            var builder = new StringBuilder(_tokens[currentTokenPosition].Value);
            while (PeekNextToken(currentTokenPosition) is WordToken wordToken && wordToken.IsStartingWithUpper)
            {
                builder.Append($"_{wordToken.Value}");
                currentTokenPosition++;
            }

            return (new VariableExpression(builder.ToString()), currentTokenPosition + 1);
        }

        private (IExpression expression, int nextTokenPosition) ParseCommonVariable(int currentTokenPosition)
        {
            var nextToken = PeekNextToken(currentTokenPosition);
            if (nextToken is not WordToken)
                throw new UnexpectedTokenException(nextToken.GetType().Name);

            var tokenAfterVariable = PeekNextToken(currentTokenPosition + 1);

            return (tokenAfterVariable, _isInConditionParsingContext) switch
            {
                (IsToken { Value: Is }, false) => CreateSimpleAssigment(CreateCommonVariable(currentTokenPosition).expression, currentTokenPosition + 2),
                _ => CreateCommonVariable(currentTokenPosition),
            };
        }

        private (VariableExpression expression, int nextTokenPosition) CreateCommonVariable(int currentTokenPosition)
        {
            var nextToken = _tokens[currentTokenPosition + 1];

            if (nextToken is not WordToken)
                throw new UnexpectedTokenException(nextToken.Value);

            return (new VariableExpression($"{_tokens[currentTokenPosition].Value}_{nextToken.Value}"), currentTokenPosition + 2);
        }

        private (IExpression, int) CreateFunctionInvocationExpression(int currentTokenPosition)
        {
            var functionName = _tokens[currentTokenPosition].Value;

            try
            {
                _isInFunctionArgumentsContext = true;
                var (arguments, nextTokenPosition) = WithConditionParsingState(() => SelectArgumentExpressionsFromLine(currentTokenPosition + 2), false);
                return (new FunctionInvocationExpression(arguments, functionName), nextTokenPosition);
            }
            finally
            {
                _isInFunctionArgumentsContext = false;
            }

            (IEnumerable<IExpression> expression, int nextTokenPosition) SelectArgumentExpressionsFromLine(int tokenPosition)
            {
                var result = new List<IExpression>();
                _expressionsByDepth.Push(result);
                var nextToken = _tokens[tokenPosition];
                while (CanParseArgumentsFarther(nextToken))
                {
                    var (argumentExpression, nextTokenPosition) = CreateExpressionBranch(tokenPosition);
                    if (PeekToken(nextTokenPosition).CanBeArgumentSeparator())
                        nextTokenPosition++;

                    tokenPosition = nextTokenPosition;
                    nextToken = _tokens[tokenPosition];
                    if (argumentExpression is null)
                        break;
                    result.Add(argumentExpression);
                }

                _expressionsByDepth.Pop();
                return (result, tokenPosition);

                static bool CanParseArgumentsFarther(Token token)
                {
                    return token switch
                    {
                        AssigmentToken _ => false,
                        IsToken _ => false,
                        EndOfTheLineToken _ => false,
                        EndOfFileToken _ => false,
                        _ => true,
                    };
                }
            }
        }

        private T WithConditionParsingState<T>(Func<T> func, bool state)
        {
            var current = _isInConditionParsingContext;
            try
            {
                _isInConditionParsingContext = state;
                return func();
            }
            finally
            {
                _isInConditionParsingContext = current;
            }
        }

        private (IExpression, int) CreateFunctionExpression(int currentTokenPosition)
        {
            var functionName = _tokens[currentTokenPosition].Value;
            var arguments = SelectArgumentsFromLine(currentTokenPosition + 2);
            var nextLinePosition = GetNextLinePosition(currentTokenPosition);
            var innerExpressions = new List<IExpression>();
            _expressionsByDepth.Push(innerExpressions);
            while (_tokens[nextLinePosition] is not EndOfFileToken)
            {
                if (PeekToken(nextLinePosition) is FunctionReturnToken
                    && PeekNextToken(nextLinePosition) is FunctionReturnToken)
                {
                    var (returnExpression, nextTokenPosition) = CreateExpressionWithBacktracking(nextLinePosition + 2);
                    innerExpressions.Add(returnExpression);
                    _expressionsByDepth.Pop();
                    return (new FunctionExpression(innerExpressions, arguments, functionName), nextTokenPosition);
                }
                else
                {
                    var (functionExpression, nextToken) = CreateExpressionBranch(nextLinePosition);
                    if (functionExpression is not null)
                        innerExpressions.Add(functionExpression);
                    nextLinePosition = nextToken;
                }
            }
            _expressionsByDepth.Pop();
            throw new InvalidOperationException("Function does not return");

            IEnumerable<FunctionArgument> SelectArgumentsFromLine(int position)
            {
                _isInFunctionArgumentsContext = true;
                try
                {
                    var startPosition = position;
                    var nextLinePosition = GetNextLinePosition(startPosition);

                    for (; position < nextLinePosition; position++)
                    {
                        var token = PeekToken(position);
                        if (token.CanBeArgumentSeparator() || token is EndOfTheLineToken)
                        {
                            var argTokens = _tokens[startPosition..position].ToArray();
                            yield return new FunctionArgument(argTokens.Select(t => t.Value).Aggregate((p, c) => $"{p}_{c}"));
                            startPosition = position + 1;
                        }
                    }
                }
                finally
                {
                    _isInFunctionArgumentsContext = false;
                }            
            }
        }

        private int GetNextLinePosition(int currentTokenPosition)
        {
            while (currentTokenPosition < _tokens.Length)
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

        private Token PeekToken(int currentTokenPosition)
        {
            return currentTokenPosition >= _tokens.Length
                ? new EndOfFileToken(currentTokenPosition, _tokens.Last().LineNumber + 1)
                : _tokens[currentTokenPosition];
        }

        private Token PeekNextToken(int currentTokenPosition)
        {
            return currentTokenPosition + 1 >= _tokens.Length
                ? new EndOfFileToken(currentTokenPosition + 1, _tokens.Last().LineNumber + 1)
                : _tokens[currentTokenPosition + 1];
        }

        private Token PeekPreviousToken(int currentTokenPosition)
        {
            return currentTokenPosition <= 0
                ? new EndOfTheLineToken(0, 0)
                : _tokens[currentTokenPosition - 1];
        }

        private IExpression PopLastExpressionFromCurrentTreeLevel()
        {
            var currentLevel = _expressionsByDepth.Peek();
            var lastExpression = currentLevel[currentLevel.Count - 1];
            currentLevel.RemoveAt(currentLevel.Count - 1);

            return lastExpression;
        }
    }
}
