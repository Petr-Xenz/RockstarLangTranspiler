using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class IncrementDecrementExpressionsTests
    {
        [TestMethod]
        public void IncrementProperVariable()
        {
            var tokens = new Token[]
            {
                new IncrementToken(0, 0, Build),
                new WordToken(0, 0, "A"),
                new WordToken(0, 0, "Common"),
                new WordToken(0, 0, "Name"),
                new IncrementToken(0, 0, Up),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();

            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is IncrementExpression ie
                && ie.Variable.VariableName == "A_Common_Name");
        }

        [TestMethod]
        public void IncrementSimpleVariable()
        {
            var tokens = new Token[]
            {
                new IncrementToken(0, 0, Build),
                new WordToken(0, 0, "world"),
                new IncrementToken(0, 0, Up),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();

            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is IncrementExpression ie
                && ie.Variable.VariableName == "world");
        }

        [TestMethod]
        public void IncrementFullVariable()
        {
            var tokens = new Token[]
            {
                new IncrementToken(0, 0, Build),
                new CommonVariablePrefixToken(0, 0, "the"),
                new WordToken(0, 0, "world"),
                new IncrementToken(0, 0, Up),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();

            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is IncrementExpression ie
                && ie.Variable.VariableName == "the_world");
        }

        [TestMethod]
        public void DecrementProperVariable()
        {
            var tokens = new Token[]
            {
                new DecrementToken(0, 0, Knock),
                new WordToken(0, 0, "A"),
                new WordToken(0, 0, "Common"),
                new WordToken(0, 0, "Name"),
                new DecrementToken(0, 0, Down),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();

            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is DecrementExpression de
                && de.Variable.VariableName == "A_Common_Name");
        }
    }
}
