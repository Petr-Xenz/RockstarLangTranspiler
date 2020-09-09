using RockstarLangTranspiler.Expressions;
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
                OutputToken _ => CreateOutputExpression(currentTokenPosition),
                AssigmentToken _ => CreateAssigmentExpression(currentTokenPosition),
                IfToken _ => CreateConditionExpression(currentTokenPosition),
                WhileToken _ => CreateWhileExpression(currentTokenPosition),
                AndToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                CommaToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                FunctionArgumentSeparatorToken _ => CreateExpressionBranch(currentTokenPosition + 1),
                CommonVariablePrefixToken _ => ParseCommonVariable(currentTokenPosition),
                WordToken _ => ParseWordToken(currentTokenPosition),
                EndOfTheLineToken _ => (null, currentTokenPosition + 1),
                _ => throw new ArgumentException(),
            };

            _tokenPositionToExpression[(token.LinePosition, token.LineNumber)] = expression.expression;

            return expression;
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
            var (right, next) = CreateExpressionBranch(currentTokenPosition + 1);
            return (ctor(left, right), next);
        }

        private (WhileExpression, int) CreateWhileExpression(int currentTokenPosition)
        {
            var (conditionExpression, nextTokenPosition) = CreateExpressionWithBacktracking(currentTokenPosition);

            if (conditionExpression.IsVoidType())
                throw new InvalidOperationException("Condition expression must have return value");

            var nextToken = _tokens[nextTokenPosition];

            var inners = new List<IExpression>();
            _expressionsByDepth.Push(inners);
            while (!(nextToken is EndOfFileToken || _tokens[nextTokenPosition + 1] is EndOfTheLineToken))
            {
                var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                if (inner is not null)
                    inners.Add(inner);
                nextToken = _tokens[nextInner];
                nextTokenPosition = nextInner;
            }
            _expressionsByDepth.Pop();
            return (new WhileExpression(conditionExpression, inners), nextTokenPosition);
        }

        private (IfExpression, int) CreateConditionExpression(int currentTokenPosition)
        {
            var (conditionExpression, nextTokenPosition) = CreateExpressionWithBacktracking(currentTokenPosition);

            if (conditionExpression.IsVoidType())
                throw new InvalidOperationException("Condition expression must have return value");

            var nextToken = _tokens[nextTokenPosition];

            var inners = new List<IExpression>();
            _expressionsByDepth.Push(inners);
            while (!(nextToken is EndOfFileToken || nextToken is ElseToken || _tokens[nextTokenPosition + 1] is EndOfTheLineToken))
            {
                var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                if (inner is not null)
                    inners.Add(inner);
                nextToken = _tokens[nextInner];
                nextTokenPosition = nextInner;
            }
            _expressionsByDepth.Pop();

            var elseExpressions = new List<IExpression>();
            _expressionsByDepth.Push(elseExpressions);
            if (nextToken is ElseToken)
            {
                nextTokenPosition++;
                while (!(nextToken is EndOfFileToken || _tokens[nextTokenPosition + 1] is EndOfTheLineToken))
                {
                    var (inner, nextInner) = CreateExpressionBranch(nextTokenPosition);
                    if (inner is not null)
                        elseExpressions.Add(inner);
                    nextToken = _tokens[nextInner];
                    nextTokenPosition = nextInner;
                }
            }
            _expressionsByDepth.Pop();

            return (new IfExpression(conditionExpression, inners, elseExpressions), nextTokenPosition);
        }

        private (IExpression expression, int nextTokenPosition) CreateExpressionWithBacktracking(int currentTokenPosition)
        {
            var expressions = new List<IExpression>();
            var nextTokenPosition = currentTokenPosition + 1;
            IExpression expression = null;
            _expressionsByDepth.Push(expressions);
            while (_tokens[nextTokenPosition] is not EndOfTheLineToken)
            {
                (expression, nextTokenPosition) = CreateExpressionBranch(nextTokenPosition);
                expressions.Add(expression);
            }
            _expressionsByDepth.Pop();
            Debug.Assert(expressions.Count == 1);

            return (expression, nextTokenPosition + 1);
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
                var variable = CreateVaraibleExpression(currentTokenPosition + 1);

                var (expression, nextTokenPosition) = CreateExpressionBranch(variable.nextTokenPosition + 1);

                return (new VariableAssigmentExpression(variable.expression, expression),
                    nextTokenPosition);
            }

            (VariableAssigmentExpression, int) PutAssigmentBranch()
            {
                var (expression, nextTokenPosition) = CreateExpressionBranch(currentTokenPosition + 1);
                var expectedAuxiliaryToken = PeekNextToken(nextTokenPosition - 1) as AssigmentToken;
                if (expectedAuxiliaryToken is null || !expectedAuxiliaryToken.Value.Equals(Into, StringComparison.OrdinalIgnoreCase))
                    throw new UnexpectedTokenException();

                var variable = CreateVaraibleExpression(nextTokenPosition + 1);

                return (new VariableAssigmentExpression(variable.expression, expression),
                    variable.nextTokenPosition);
            }
        }

        private (IExpression expression, int nextTokenPosition) ParseWordToken(int currentTokenPosition)
        {
            var token = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);
            var result = nextToken switch
            {
                AssigmentToken _ => CreateSimpleAssigment(CreateSimpleVariableExpression(token, currentTokenPosition).expression, currentTokenPosition + 1),
                FunctionDeclarationToken _ => CreateFunctionExpression(currentTokenPosition),
                FunctionInvocationToken _ => CreateFunctionInvocationExpression(currentTokenPosition),

                CombiningToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),
                EndOfTheLineToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),
                EndOfFileToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),
                AndToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),
                CommaToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),
                FunctionArgumentSeparatorToken _ => CreateSimpleVariableExpression(token, currentTokenPosition),

                WordToken { IsStartingWithUpper: true } _ => CreateVaraibleExpression(currentTokenPosition),
                _ => throw new NotSupportedException(),
            };

            return result;
        }

        private (IExpression expression, int nextTokenPosition) CreateSimpleAssigment(VariableExpression variableExpression, int assigmentTokenPosition)
        {
            var (expression, nextTokenPosition) = CreateExpressionBranch(assigmentTokenPosition + 1);
            return (new VariableAssigmentExpression(variableExpression, expression), nextTokenPosition);
        }

        private static (VariableExpression expression, int nextTokenPosition) CreateSimpleVariableExpression(Token token, int currentTokenPosition) 
            => (new VariableExpression(token.Value), currentTokenPosition + 1);

        private (VariableExpression expression, int nextTokenPosition) CreateVaraibleExpression(int currentTokenPosition)
        {
            var currentToken = _tokens[currentTokenPosition];
            var nextToken = PeekNextToken(currentTokenPosition);

            return (currentToken, nextToken) switch
            {
                (CommonVariablePrefixToken _, WordToken _) => CreateCommonVariable(currentTokenPosition),
                (WordToken { IsStartingWithUpper: true } _, WordToken { IsStartingWithUpper: true } _) => CreateProperVariable(currentTokenPosition),
                (WordToken word, _) => CreateSimpleVariableExpression(word, currentTokenPosition),
                _ => throw new UnexpectedTokenException(currentToken.GetType().ToString()),
            };
        }

        private (VariableExpression expression, int nextTokenPosition) CreateProperVariable(int currentTokenPosition)
        {
            var builder = new StringBuilder(_tokens[currentTokenPosition].Value);
            while(PeekNextToken(currentTokenPosition) is WordToken wordToken && wordToken.IsStartingWithUpper)
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

            return tokenAfterVariable switch
            {
                AssigmentToken { Value: Is } => CreateSimpleAssigment(CreateCommonVariable(currentTokenPosition).expression, currentTokenPosition + 2),
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
            var (arguments, nextTokenPosition) = SelectArgumentExpressionsFromLine(currentTokenPosition + 2);

            return (new FunctionInvocationExpression(arguments, functionName), nextTokenPosition);

            (IEnumerable<IExpression> expression, int nextTokenPosition) SelectArgumentExpressionsFromLine(int tokenPosition)
            {
                var result = new List<IExpression>();
                _expressionsByDepth.Push(result);
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

                _expressionsByDepth.Pop();
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
            _expressionsByDepth.Push(innerExpressions);
            while(_tokens[nextLinePosition] is not EndOfFileToken)
            {
                if (_tokens[nextLinePosition] is FunctionReturnToken)
                {
                    var (returnExpression, nextTokenPosition) = CreateExpressionWithBacktracking(nextLinePosition + 1);
                    innerExpressions.Add(returnExpression);
                    _expressionsByDepth.Pop();
                    return (new FunctionExpression(innerExpressions, arguments, functionName), nextTokenPosition);
                }
                else
                {
                    var (functionExpression, _) = CreateExpressionBranch(nextLinePosition);
                    innerExpressions.Add(functionExpression);
                    nextLinePosition = GetNextLinePosition(nextLinePosition);
                }
            }

            _expressionsByDepth.Pop();
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
                ? new EndOfFileToken(currentTokenPosition + 1, _tokens.Last().LineNumber + 1) 
                : _tokens[currentTokenPosition + 1];
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
