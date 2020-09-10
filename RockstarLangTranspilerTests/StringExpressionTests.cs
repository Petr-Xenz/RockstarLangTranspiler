using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class StringExpressionTests
    {
        [TestMethod]
        public void ParseSingleString()
        {
            var tokens = new Token[]
            {
                new QuoteToken(0, 0),
                new WordToken(0, 0, "boy"),
                new AssigmentToken(0, 0, "is"),
                new UndefinedToken(0, 0, "mysterious"),
                new QuoteToken(0, 0),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var stringExpression = tree.RootExpressions.Single() as StringExpression;
            Assert.IsNotNull(stringExpression);

            Assert.AreEqual("boy is mysterious", stringExpression.Value);
        }

        [TestMethod]
        public void ParseStringAssignment()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "boy"),
                new AssigmentToken(0, 0, "is"),
                new QuoteToken(0, 0),
                new UndefinedToken(0, 0, "mysterious"),
                new QuoteToken(0, 0),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var assignExpression = tree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.IsNotNull(assignExpression);

            var stringExpression = assignExpression.AssigmentExpression as StringExpression;
            Assert.IsNotNull(stringExpression);
            Assert.AreEqual("mysterious", stringExpression.Value);
        }

        [TestMethod]
        public void ParseStringConcatination()
        {
            var tokens = new Token[]
            {
                new QuoteToken(0, 0),
                new WordToken(0, 0, "boy"),
                new AssigmentToken(0, 0, "is"),
                new QuoteToken(0, 0),
                new AdditionToken(0, 0, "with"),
                new QuoteToken(0, 0),
                new UndefinedToken(0, 0, "gun"),
                new QuoteToken(0, 0),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var addition = tree.RootExpressions.Single() as AdditionExpression;
            Assert.IsNotNull(addition);

            var left = addition.Left as StringExpression;
            Assert.IsNotNull(left);
            Assert.AreEqual("boy is", left.Value);

            var right = addition.Right as StringExpression;
            Assert.IsNotNull(right);
            Assert.AreEqual("gun", right.Value);
        }
    }
}
