using RockstarLangTranspiler.Expressions;
using System;
using System.Collections;
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
                AdditionExpression addition => CreateAdditionExpression(addition),
                VariableAssigmentExpression assigment => CreateAssigmentExpression(assigment),
                FunctionExpression function => CreateFunctionExpression(function),
                _ => throw new NotSupportedException(expression.GetType().FullName)
            };
        }

        private string CreateFunctionExpression(FunctionExpression function)
        {
            return $"function {function.Name}({TransformArguments(function.Arguments)}){{\n{TranspileInnerExpressions(function.InnerExpressions)}\n}}";

            string TranspileInnerExpressions(IEnumerable<IExpression> expressions)
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

        private string CreateAssigmentExpression(VariableAssigmentExpression assigment) 
            => $"let {assigment.VariableName} = {TranspileExpression(assigment.AssigmentExpression)}";

        private string CreateConstantExpression(ConstantExpression constant) 
            => constant.Value.ToString();

        private string CreateConsoleInfoExpression(OutputExpression output) 
            => $"console.info({TranspileExpression(output.ExpressionToOutput)})";

        private string CreateAdditionExpression(AdditionExpression addition)
            => $"{TranspileExpression(addition.Left)} + {TranspileExpression(addition.Right)}";
    }
}
