using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class FunctionInvokationExpressionTests
    {
        [TestMethod]
        public void SingleArgumentFunctionInvokation()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "func"),
                new FunctionInvokationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new EndOfTheLineToken(0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            var invokeExp = result[0] as FunctionInvokationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);
            Assert.AreEqual(1, invokeExp.ArgumentExpressions.Count());
            Assert.IsTrue(invokeExp.ArgumentExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void InvokeFunctionAsSimpleVariableAssigmentExpression()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "var"),
                new AssigmentToken(0, 0, "is"),
                new WordToken(0, 0, "func"),
                new FunctionInvokationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new EndOfTheLineToken(0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result[0] is VariableAssigmentExpression);
            var varExp = result[0] as VariableAssigmentExpression;

            Assert.AreEqual(varExp.VariableName, "var");
            Assert.IsTrue(varExp.AssigmentExpression is FunctionInvokationExpression);
            var invokeExp = varExp.AssigmentExpression as FunctionInvokationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);
            Assert.AreEqual(1, invokeExp.ArgumentExpressions.Count());
            Assert.IsTrue(invokeExp.ArgumentExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void InvokeFunctionAsPutVariableAssigmentExpression()
        {
            var tokens = new Token[]
            {
                new AssigmentToken(0, 0, "put"),
                new WordToken(0, 0, "func"),
                new FunctionInvokationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new AssigmentToken(0, 0, "into"),
                new WordToken(0, 0, "var"),
                new EndOfTheLineToken(0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result[0] is VariableAssigmentExpression);
            var varExp = result[0] as VariableAssigmentExpression;

            Assert.AreEqual(varExp.VariableName, "var");
            Assert.IsTrue(varExp.AssigmentExpression is FunctionInvokationExpression);
            var invokeExp = varExp.AssigmentExpression as FunctionInvokationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);
            Assert.AreEqual(1, invokeExp.ArgumentExpressions.Count());
            Assert.IsTrue(invokeExp.ArgumentExpressions.Single() is ConstantExpression);
        }
    }
}
