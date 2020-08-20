using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
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
            var lexer = new Lexer(input, new ITokenFactory<Token>[] { new FunctionInvocationTokenFactory(), new NumberTokenFactory(), new WordTokenFactory() });
            var tokens = lexer.Lex().ToArray();

            Assert.AreEqual(4, tokens.Length);
            Assert.IsTrue(tokens[0] is WordToken);
            Assert.IsTrue(tokens[1] is FunctionInvocationToken);
            Assert.IsTrue(tokens[2] is NumberToken);
        }
    }
}
