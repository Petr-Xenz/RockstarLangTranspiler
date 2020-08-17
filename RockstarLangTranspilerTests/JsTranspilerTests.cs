using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using System.Security.Cryptography.X509Certificates;

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
                    new FunctionExpression(new ConstantExpression(3), 
                    new[] { new FunctionArgument("x"), new FunctionArgument("y") }, 
                    "fun")
            });

            var result = new JsTranspiler().Transpile(tree);

            var function = "function fun(x, y){\n\treturn 3\n}";

            Assert.AreEqual(function, result);
        }
    }
}
