using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class FunctionInvokationLexingTests
    {
        [TestMethod]
        public void SingleArgumentFunctionInvokation()
        {
            var input = "func taking 3";
            var lexer = new Lexer(input, new ITokenFactory<Token>[] { new FunctionInvokationTokenFactory(), new NumberTokenFactory(), new WordTokenFactory() });
            var tokens = lexer.Lex().ToArray();

            Assert.AreEqual(4, tokens.Length);
            Assert.IsTrue(tokens[0] is WordToken);
            Assert.IsTrue(tokens[1] is FunctionInvokationToken);
            Assert.IsTrue(tokens[2] is NumberToken);
        }
    }

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
    }
}
