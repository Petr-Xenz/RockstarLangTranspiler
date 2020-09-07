using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class FunctionInvocationExpressionTests
    {
        [TestMethod]
        public void SingleArgumentFunctionInvoсation()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "func"),
                new FunctionInvocationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new EndOfTheLineToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            var invokeExp = result[0] as FunctionInvocationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);
            Assert.AreEqual(1, invokeExp.ArgumentExpressions.Count());
            Assert.IsTrue(invokeExp.ArgumentExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void MultipleArgumentsFunctionInvoсation()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "func"),
                new FunctionInvocationToken(0, 0, "taking"),
                new NumberToken(0, 0, "1"),
                new AndToken(0, 0, "and"),
                new WordToken(0, 0, "x"),
                new CommaToken(0, 0),
                new NumberToken(0, 0, "3.2"),
                new FunctionArgumentSeparatorToken(0, 0, "&"),
                new WordToken(0, 0, "y"),
                new EndOfTheLineToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            var invokeExp = result[0] as FunctionInvocationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);

            var arguments = invokeExp.ArgumentExpressions.ToArray();
            Assert.AreEqual(4, arguments.Length);
            Assert.IsTrue(arguments[0] is ConstantExpression);
            Assert.IsTrue(arguments[1] is VariableExpression);
            Assert.IsTrue(arguments[2] is ConstantExpression);
            Assert.IsTrue(arguments[3] is VariableExpression);
        }

        [TestMethod]
        public void InvokeFunctionAsSimpleVariableAssigmentExpression()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "var"),
                new AssigmentToken(0, 0, "is"),
                new WordToken(0, 0, "func"),
                new FunctionInvocationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new EndOfTheLineToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result[0] is VariableAssigmentExpression);
            var varExp = result[0] as VariableAssigmentExpression;

            Assert.AreEqual("var", varExp.Variable.VariableName);
            Assert.IsTrue(varExp.AssigmentExpression is FunctionInvocationExpression);
            var invokeExp = varExp.AssigmentExpression as FunctionInvocationExpression;

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
                new FunctionInvocationToken(0, 0, "taking"),
                new NumberToken(0, 0, "3"),
                new AssigmentToken(0, 0, "into"),
                new WordToken(0, 0, "var"),
                new EndOfTheLineToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result[0] is VariableAssigmentExpression);
            var varExp = result[0] as VariableAssigmentExpression;

            Assert.AreEqual("var", varExp.Variable.VariableName);
            Assert.IsTrue(varExp.AssigmentExpression is FunctionInvocationExpression);
            var invokeExp = varExp.AssigmentExpression as FunctionInvocationExpression;

            Assert.IsNotNull(invokeExp);
            Assert.AreEqual("func", invokeExp.Name);
            Assert.AreEqual(1, invokeExp.ArgumentExpressions.Count());
            Assert.IsTrue(invokeExp.ArgumentExpressions.Single() is ConstantExpression);
        }
    }
}
