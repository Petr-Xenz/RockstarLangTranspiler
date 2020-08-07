using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class AdditionTokensTests
    {
        [TestMethod]
        public void SingleConstantAdditionExpression()
        {
            var file = "1 with 6.7";
            var lexer = new Lexer(file, new TokenFactory[] { new AdditionTokenFactory(), new NumberTokenFactory(), new WhitespaceTokenFactory() });
            var tokens = lexer.Lex().ToArray();

            Assert.AreEqual(4, tokens.Length);
            Assert.IsTrue(tokens[0] is NumberToken n && n.Value == "1");
            Assert.IsTrue(tokens[1] is AdditionToken a && a.Value == "with");
            Assert.IsTrue(tokens[2] is NumberToken n2 && n2.Value == "6.7");
        }
    }
}
