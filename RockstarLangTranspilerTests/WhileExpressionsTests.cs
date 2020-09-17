using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;
using static RockstarLangTranspiler.KeyWords;

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
                new BooleanToken(0, 0, "right"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is BooleanExpression be && be.Value == true);
            Assert.IsTrue(we.InnerExpressions.Single() is OutputExpression);
        }

        [TestMethod]
        public void WhileWithSingleContinueToken()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new BooleanToken(0, 0, "right"),
                new EndOfTheLineToken(0, 0),
                    new ContinueToken(0, 0, Continue),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is BooleanExpression be && be.Value == true);
            Assert.IsTrue(we.InnerExpressions.Single() is ContinueExpression);
        }

        [TestMethod]
        public void WhileWithContinueAliasTokens()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new BooleanToken(0, 0, "right"),
                new EndOfTheLineToken(0, 0),
                    new ContinueToken(0, 0, Take),
                    new WordToken(0, 0, "it"),
                    new WordToken(0, 0, "to"),
                    new CommonVariablePrefixToken(0, 0, The),
                    new WordToken(0, 0, "top"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is BooleanExpression be && be.Value == true);
            Assert.IsTrue(we.InnerExpressions.Single() is ContinueExpression);
        }

        [TestMethod]
        public void WhileWithSingleBreakToken()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new BooleanToken(0, 0, "right"),
                new EndOfTheLineToken(0, 0),
                    new BreakToken(0, 0, Break),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is BooleanExpression be && be.Value == true);
            Assert.IsTrue(we.InnerExpressions.Single() is BreakExpression);
        }

        [TestMethod]
        public void WhileWithBreakToken()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "foo"),
                new IsToken(0, 0, Is),
                new NumberToken(0, 0, "5"),
                new EndOfTheLineToken(0, 0),

                new WhileToken(0, 0, "while"),
                new WordToken(0, 0, "foo"),
                new EndOfTheLineToken(0, 0),

                    new DecrementToken(0, 0, Knock),
                    new WordToken(0, 0, "foo"),
                    new DecrementToken(0, 0, Down),
                    new EndOfTheLineToken(0, 0),

                    new IfToken(0, 0, If),
                    new NumberToken(0, 0, "4"),
                    new IsToken(0, 0, Is),
                    new NumberToken(0, 0, "4"),
                    new EndOfTheLineToken(0, 0),

                    new BreakToken(0, 0, Break),
                    new EndOfTheLineToken(0, 0),
                    new EndOfTheLineToken(0, 0),

                    new OutputToken(0, 0, Say),
                    new WordToken(0, 0, "foo"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(2, tree.RootExpressions.Count());
        }

        [TestMethod]
        public void WhileWithBreakAliasTokens()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new BooleanToken(0, 0, "right"),
                new EndOfTheLineToken(0, 0),
                    new BreakToken(0, 0, Break),
                    new WordToken(0, 0, "it"),
                    new DecrementToken(0, 0, Down),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is WhileExpression);
            var we = (WhileExpression)tree.RootExpressions.Single();

            Assert.IsTrue(we.ConditionExpression is BooleanExpression be && be.Value == true);
            Assert.IsTrue(we.InnerExpressions.Single() is BreakExpression);
        }

        [TestMethod]
        public void WhileConstantConditionWithSingleExpressionUntilEndOfBlock()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new EndOfTheLineToken(0, 0),

                new OutputToken(0, 0, "say"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
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
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
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
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
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
