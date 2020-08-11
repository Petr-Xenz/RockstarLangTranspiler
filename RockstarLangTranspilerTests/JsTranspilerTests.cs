using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;

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
    }
}
