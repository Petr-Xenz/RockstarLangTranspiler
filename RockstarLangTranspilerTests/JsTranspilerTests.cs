using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class JsTranspilerTests
    {
        [TestMethod]
        public void TranspileOutputExpressionToConsoleInfo()
        {
            var tree = new SyntaxTree(new[] { new OutputExpression(new ConstantExpression(5)) });
            var transpiler = new JsTranspiler();
            var result = transpiler.Transpile(tree);

            Assert.AreEqual("console.info(5)", result);
        }

        [TestMethod]
        public void TranspileAdditionExpression()
        {
            var tree = new SyntaxTree(new[] { new AdditionExpression(new ConstantExpression(5), new ConstantExpression(2)) });
            var transpiler = new JsTranspiler();
            var result = transpiler.Transpile(tree);

            Assert.AreEqual("5 + 2", result);
        }

        [TestMethod]
        public void TranspileOutputWithAddition()
        {
            var tree = new SyntaxTree(new[]
            {
                new OutputExpression(
                    new AdditionExpression(
                        new ConstantExpression(3),
                        new ConstantExpression(44)))
            });

            var transpiler = new JsTranspiler();
            var result = transpiler.Transpile(tree);

            Assert.AreEqual("console.info(3 + 44)", result);
        }

        [TestMethod]
        public void TranspileAssigmentExpression()
        {
            var tree = new SyntaxTree(new[]
            {
                    new VariableAssigmentExpression("x", new ConstantExpression(3))
            });

            var transpiler = new JsTranspiler();
            var result = transpiler.Transpile(tree);

            Assert.AreEqual("let x = 3", result);
        }

        [TestMethod]
        public void TranspileFunctionExpression()
        {
            var tree = new SyntaxTree(new[]
            {
                    new FunctionExpression(new [] { new ConstantExpression(3) },
                    new[] { new FunctionArgument("x"), new FunctionArgument("y") },
                    "fun")
            });

            var result = new JsTranspiler().Transpile(tree);

            var function = "function fun(x, y){\n\treturn 3\n}";

            Assert.AreEqual(function, result);
        }

        [TestMethod]
        public void TranspileVariableFunctionFunctionExpression()
        {
            var tree = new SyntaxTree(new[]
            {
                    new FunctionExpression(new [] 
                    { 
                        new AdditionExpression(new VariableExpression("x"), new VariableExpression("y")) 
                    },
                    new[] { new FunctionArgument("x"), new FunctionArgument("y") },
                    "fun")
            });

            var result = new JsTranspiler().Transpile(tree);

            var function = "function fun(x, y){\n\treturn x + y\n}";

            Assert.AreEqual(function, result);
        }

        [TestMethod]
        public void TranspileSingleStatementFunction()
        {
            var tree = new SyntaxTree(new[]
{
                    new FunctionExpression(new [] { new OutputExpression(new ConstantExpression(3)) },
                    new[] { new FunctionArgument("x"), new FunctionArgument("y") },
                    "fun")
            });

            var result = new JsTranspiler().Transpile(tree);
            var function = "function fun(x, y){\n\tconsole.info(3)\n}";
            Assert.AreEqual(function, result);
        }

        [TestMethod]
        public void TranspileMultipleExpressionsFunction()
        {
            var tree = new SyntaxTree(new[]
{
                    new FunctionExpression(new IExpression[] 
                        {
                            new OutputExpression(new ConstantExpression(3)),
                            new OutputExpression(new ConstantExpression(4)),
                            new ConstantExpression(5),
                        },
                    new[] { new FunctionArgument("x"), new FunctionArgument("y") },
                    "fun")
            });

            var result = new JsTranspiler().Transpile(tree);
            var function = "function fun(x, y){ console.info(3) console.info(4) return 5}";
            Assert.AreEqual(function.RemoveNonPrintableChras(), result.RemoveNonPrintableChras());
        }

        [TestMethod]
        public void TranspileFunctionInvocation()
        {
            var tree = new SyntaxTree(new[]
            {
                new FunctionInvocationExpression(new IExpression[]
                {
                    new ConstantExpression(1),
                    new AdditionExpression(new ConstantExpression(2), new ConstantExpression(3)),
                    new FunctionInvocationExpression(Enumerable.Empty<IExpression>(), "argFunc")
                }, "func")
            });

            var result = new JsTranspiler().Transpile(tree);
            var expected = "func(1, 2 + 3, argFunc())";
            Assert.AreEqual(expected.RemoveNonPrintableChras(), result.RemoveNonPrintableChras());
        }

        [TestMethod]
        public void TranspileSingleIfExpression()
        {
            var tree = new SyntaxTree(new[]
            {
                new IfExpression(new ConstantExpression(1), new IExpression[]
                {
                    new OutputExpression(new ConstantExpression(1)),
                })
            });

            var result = new JsTranspiler().Transpile(tree);
            var expected = "if (1) { console.info(1) }";

            Assert.AreEqual(expected.RemoveNonPrintableChras(), result.RemoveNonPrintableChras());
        }

        [TestMethod]
        public void TranspileIfElseExpression()
        {
            var tree = new SyntaxTree(new[]
            {
                new IfExpression(new ConstantExpression(1), new IExpression[]
                {
                    new OutputExpression(new ConstantExpression(1)),
                }, new IExpression[]
                {
                    new OutputExpression(new ConstantExpression(2)),
                })
            });

            var result = new JsTranspiler().Transpile(tree);
            var expected = "if (1) { console.info(1) } else { console.info(2) }";

            Assert.AreEqual(expected.RemoveNonPrintableChras(), result.RemoveNonPrintableChras());
        }
    }

    public static class Extensions
    {
        public static string RemoveNonPrintableChras(this string str)
        {
            return str.Replace("\t", null).Replace("\n", null).Replace("\r", null).Replace(" ", null);
        }
    }
}
