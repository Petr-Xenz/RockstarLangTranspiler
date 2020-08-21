using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class AssigmentAndWordsTokensLexingTests
    {
        [TestMethod]
        public void SimpleAssigmentTest()
        {
            var file = "Foo is 5";
            var lexer = new Lexer(file, new ITokenFactory<Token>[] { new AssigmentTokenFactory(), new NumberTokenFactory(), new WordTokenFactory() });
            var tokens = lexer.Lex().ToArray();

            Assert.AreEqual(4, tokens.Length);
            Assert.IsTrue(tokens[0] is WordToken n && n.Value == "Foo");
            Assert.IsTrue(tokens[1] is AssigmentToken a && a.Value == "is");
            Assert.IsTrue(tokens[2] is NumberToken n2 && n2.Value == "5");
        }

        [TestMethod]
        public void MultiWordAssigmentTest()
        {
            var file = "Let foo is bar";
            var lexer = new Lexer(file, new ITokenFactory<Token>[] { new AssigmentTokenFactory(), new NumberTokenFactory(), new WordTokenFactory() });
            var tokens = lexer.Lex().ToArray();

            Assert.AreEqual(5, tokens.Length);
            Assert.IsTrue(tokens[0] is AssigmentToken t0 && t0.Value == "Let");
            Assert.IsTrue(tokens[1] is WordToken t1 && t1.Value == "foo");
            Assert.IsTrue(tokens[2] is AssigmentToken t2 && t2.Value == "is");
            Assert.IsTrue(tokens[3] is WordToken t3 && t3.Value == "bar");
        }
    }
}
