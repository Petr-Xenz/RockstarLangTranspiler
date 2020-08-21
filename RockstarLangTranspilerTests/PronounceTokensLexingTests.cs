using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class PronounceTokensLexingTests
    {
        [TestMethod]
        public void LexFindsSingleToken()
        {
            var file = "she";
            var lexer = new Lexer(file, new[] { new PronounTokenFactory() });
            var tokens = lexer.Lex();
            var pronounceTokens = tokens.OfType<PronounToken>().ToArray();

            Assert.AreEqual(1, pronounceTokens.Length);

            var sheToken = new PronounToken(0, 3, "she");

            Assert.AreEqual(sheToken.Length, pronounceTokens[0].Length);
            Assert.AreEqual(sheToken.StartLocation, pronounceTokens[0].StartLocation);
            Assert.AreEqual(sheToken.Value, pronounceTokens[0].Value);
        }
    }
}
