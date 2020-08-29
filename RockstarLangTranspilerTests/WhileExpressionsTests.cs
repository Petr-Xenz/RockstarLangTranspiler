using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class WhileExpressionsTests
    {
        [TestMethod]
        public void WhileConstantConditionWithSingleExpressionUntilEndOfFile()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0),
                new EndOfFileToken(0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is ConstantExpression);
            Assert.IsTrue(we.InnerExpressions.Single() is OutputExpression);
        }

        [TestMethod]
        public void WhileConstantConditionWithSingleExpressionUntilEndOfBlock()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0),
                new EndOfTheLineToken(0),

                new OutputToken(0, 0, "say"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(2, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.First();

            Assert.IsTrue(we.ConditionExpression is ConstantExpression);
            Assert.IsTrue(we.InnerExpressions.Single() is OutputExpression);
        }

        [TestMethod]
        public void WhileComplexConditionWithSingleExpression()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new AdditionToken(0, 0, "plus"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0),
                new EndOfFileToken(0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.First();

            Assert.IsTrue(we.ConditionExpression is AdditionExpression);
            Assert.IsTrue(we.InnerExpressions.Single() is OutputExpression);
        }

        [TestMethod]
        public void WhileComplexConditionWithMultipleExpressionsExpression()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new AdditionToken(0, 0, "plus"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0),
                new EndOfFileToken(0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.First();

            Assert.IsTrue(we.ConditionExpression is AdditionExpression);
            Assert.AreEqual(2, we.InnerExpressions.Count());
        }
    }
}
