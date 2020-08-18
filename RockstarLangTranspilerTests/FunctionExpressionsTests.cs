using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class FunctionExpressionsTests
    {
        [TestMethod]
        public void FunctionSingleArgumentDeclarationTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new NumberToken(0, 0, "1337")
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.IsTrue(fe.InnerExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void FunctionSingleArgumentDeclarationWithSingleLineComplexExpressionTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "DoubleLame"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new WordToken(0, 0, "lame"),
                new AdditionToken(0, 0, "with"),
                new WordToken(0, 0, "lame"),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("DoubleLame", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.IsTrue(fe.InnerExpressions.Single() is AdditionExpression);
        }

        [TestMethod]
        public void FunctionWithMultipleExpressionsDeclarationTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0),
                new OutputToken(0, 0, ""),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new NumberToken(0, 0, "1337")
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.AreEqual(2, fe.InnerExpressions.Count());
            Assert.IsTrue(fe.InnerExpressions.First() is OutputExpression);
            Assert.IsTrue(fe.InnerExpressions.Last() is ConstantExpression);
        }
    }
}
