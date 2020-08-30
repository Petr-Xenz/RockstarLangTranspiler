using RockstarLangTranspiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockstarLangTranspiler
{
    public class JsTranspiler : ITranspiler
    {
        public string Transpile(SyntaxTree syntaxTree)
        {
            return syntaxTree.RootExpressions
                .Select(TranspileExpression)
                .Aggregate((p, c) => $"{p}\n{c}");
        }

        private string TranspileExpression(IExpression expression)
        {
            return expression switch
            {
                OutputExpression output => CreateConsoleInfoExpression(output),
                ConstantExpression constant => CreateConstantExpression(constant),
                BooleanExpression boolean => CreateBooleanExpression(boolean),
                VariableExpression variable => CreateVariableExpression(variable),
                AdditionExpression addition => CreateAdditionExpression(addition),
                SubtractionExpression subtraction => CreateSubtractionExpression(subtraction),
                VariableAssigmentExpression assigment => CreateAssigmentExpression(assigment),
                FunctionExpression function => CreateFunctionExpression(function),
                FunctionInvocationExpression invokation => CreateFunctionInvocationExpression(invokation),
                IfExpression ifExpression => CreateIfExpression(ifExpression),
                WhileExpression whileExpression => CreateWhileExpression(whileExpression),
                _ => throw new NotSupportedException(expression.GetType().FullName)
            };
        }

        private string CreateWhileExpression(WhileExpression whileExpression)
        {
            var inner = TranspileInnerExpressions(whileExpression.InnerExpressions);
            return $"while ({TranspileExpression(whileExpression.ConditionExpression)}) {{\n {inner} \n}}";
        }

        private string CreateIfExpression(IfExpression ifExpression)
        {
            var inner = TranspileInnerExpressions(ifExpression.InnerExpressions);
            var elseExpressions = ifExpression.ElseExpressions.Any()
                ? $"\n else {{ \n{TranspileInnerExpressions(ifExpression.ElseExpressions)}\n}}"
                : string.Empty;

            return $"if ({TranspileExpression(ifExpression.ConditionExpression)}) {{\n {inner} \n}}{elseExpressions}";
        }

        private string CreateFunctionInvocationExpression(FunctionInvocationExpression invocation)
        {
            return $"{invocation.Name}({TranspileArguments(invocation.ArgumentExpressions)})";

            string TranspileArguments(IEnumerable<IExpression> args) => args.Any()
                    ? invocation.ArgumentExpressions.Select(TranspileExpression).Aggregate((p, c) => $"{p}, {c}")
                    : string.Empty;

        }

        private string CreateVariableExpression(VariableExpression variable) => variable.VariableName;

        private string CreateFunctionExpression(FunctionExpression function)
        {
            return $"function {function.Name}({TransformArguments(function.Arguments)}){{\n{TranspileInnerExpressions(function.InnerExpressions)}\n}}";

            static string TransformArguments(IEnumerable<FunctionArgument> args)
            {
                if (!args.Any())
                    return string.Empty;

                return args
                    .Select(a => a.Name)
                    .Aggregate((p, c) => $"{p}, {c}")
                    .Trim(',');                    
            }
        }

        private string TranspileInnerExpressions(IEnumerable<IExpression> expressions)
        {
            var builder = new StringBuilder();
            var enumerated = expressions.ToArray();

            for (int i = 0; i < enumerated.Length - 1; i++)
            {
                var expression = enumerated[i];
                builder.AppendLine($"\t{TranspileExpression(expression)}");
            }

            var lastExpression = enumerated[enumerated.Length - 1];

            var lastLine = lastExpression.IsVoidType()
                ? $"\t{TranspileExpression(lastExpression)}"
                : $"\treturn {TranspileExpression(lastExpression)}";

            builder.Append(lastLine);
            return builder.ToString();
        }

        private string CreateAssigmentExpression(VariableAssigmentExpression assigment) 
            => $"let {assigment.VariableName} = {TranspileExpression(assigment.AssigmentExpression)}";

        private string CreateBooleanExpression(BooleanExpression boolean)
            => boolean.Value ? "true" : "false";

        private string CreateConstantExpression(ConstantExpression constant) 
            => constant.Value.ToString();

        private string CreateConsoleInfoExpression(OutputExpression output) 
            => $"console.info({TranspileExpression(output.ExpressionToOutput)})";

        private string CreateAdditionExpression(AdditionExpression addition)
            => $"{TranspileExpression(addition.Left)} + {TranspileExpression(addition.Right)}";

        private string CreateSubtractionExpression(SubtractionExpression addition)
            => $"{TranspileExpression(addition.Left)} - {TranspileExpression(addition.Right)}";
    }
}
