using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class ComparsionExpressionsTests
    {
        [TestMethod]
        public void IfEqualityExpression()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
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
            Assert.IsTrue(tree.RootExpressions.First() is IfExpression);
            var e = (IfExpression)tree.RootExpressions.First();

            Assert.IsTrue(e.ConditionExpression is EqualityExpression);
            var equalityExpression = e.ConditionExpression as EqualityExpression;
            Assert.IsTrue(equalityExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(equalityExpression.Right is ConstantExpression cr && cr.Value == 1);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }

        [TestMethod]
        public void WhileEqualityExpression()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "while"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
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
            var e = (WhileExpression)tree.RootExpressions.First();

            Assert.IsTrue(e.ConditionExpression is EqualityExpression);
            var equalityExpression = e.ConditionExpression as EqualityExpression;
            Assert.IsTrue(equalityExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(equalityExpression.Right is ConstantExpression cr && cr.Value == 1);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }
    }
}
