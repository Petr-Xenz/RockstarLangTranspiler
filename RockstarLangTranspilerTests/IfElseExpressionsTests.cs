using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class IfElseExpressionsTests
    {
        [TestMethod]
        public void SingleIfWithConstantExpressionUntilEndOfInput()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions;

            Assert.IsTrue(result.Single() is IfExpression);
            var ifExpression = result.Single() as IfExpression;

            Assert.IsTrue(ifExpression.ConditionExpression is ConstantExpression);
            Assert.IsTrue(ifExpression.InnerExpressions.Single() is OutputExpression);
            var innerExpression = ifExpression.InnerExpressions.Single() as OutputExpression;

            Assert.IsTrue(innerExpression.ExpressionToOutput is ConstantExpression);
        }

        [TestMethod]
        public void SingleIfWithConstantExpressionAndAdditionConditionUntilEndOfInput()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new AdditionToken(0, 0, "plus"),
                new NumberToken(0, 0, "2"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions;

            Assert.IsTrue(result.Single() is IfExpression);
            var ifExpression = result.Single() as IfExpression;

            Assert.IsTrue(ifExpression.ConditionExpression is AdditionExpression);
            Assert.IsTrue(ifExpression.InnerExpressions.Single() is OutputExpression);
            var innerExpression = ifExpression.InnerExpressions.Single() as OutputExpression;

            Assert.IsTrue(innerExpression.ExpressionToOutput is ConstantExpression);
        }

        [TestMethod]
        public void SingleIfWithConstantExpressionUntilEndOfBlock()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new EndOfTheLineToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions;

            Assert.IsTrue(result.Single() is IfExpression);
            var ifExpression = result.Single() as IfExpression;

            Assert.IsTrue(ifExpression.ConditionExpression is ConstantExpression);
            Assert.IsTrue(ifExpression.InnerExpressions.Single() is OutputExpression);
            var innerExpression = ifExpression.InnerExpressions.Single() as OutputExpression;

            Assert.IsTrue(innerExpression.ExpressionToOutput is ConstantExpression);
        }

        [TestMethod]
        public void SingleIfWithMultipleExpressionsUntilEndOfInput()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "2"),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions;

            Assert.IsTrue(result.Single() is IfExpression);
            var ifExpression = result.Single() as IfExpression;

            Assert.IsTrue(ifExpression.ConditionExpression is ConstantExpression);
            var innerExpressions = ifExpression.InnerExpressions.ToArray();

            Assert.AreEqual(2, innerExpressions.Length);
            Assert.IsTrue(innerExpressions[0] is OutputExpression);
            Assert.IsTrue(innerExpressions[1] is OutputExpression);
            var firstExpression = innerExpressions[0] as OutputExpression;
            var secondExpression = innerExpressions[1] as OutputExpression;

            Assert.IsTrue(firstExpression.ExpressionToOutput is ConstantExpression);
            Assert.IsTrue(secondExpression.ExpressionToOutput is ConstantExpression);
        }

        [TestMethod]
        public void IfElseExpressionUntilEndOfFile()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new ElseToken(0, 0, "else"),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "2"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            var ifExpression = result[0] as IfExpression;
            Assert.IsNotNull(ifExpression);
            Assert.AreEqual(1, ifExpression.InnerExpressions.Count());

            var elseExpressions = ifExpression.ElseExpressions;
            Assert.AreEqual(1, elseExpressions.Count());
            var innerElse = elseExpressions.Single() as OutputExpression;
            Assert.IsNotNull(innerElse);
        }

        [TestMethod]
        public void IfElseExpressionUntilEndOfBlock()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "1"),
                    new EndOfTheLineToken(0, 0),
                new ElseToken(0, 0, "else"),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "2"),
                    new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "3"),
                    new EndOfTheLineToken(0, 0),
                new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var result = parser.Parse().RootExpressions.ToArray();

            Assert.AreEqual(1, result.Length);
            var ifExpression = result[0] as IfExpression;
            Assert.IsNotNull(ifExpression);
            Assert.AreEqual(1, ifExpression.InnerExpressions.Count());

            var elseExpressions = ifExpression.ElseExpressions;
            Assert.AreEqual(2, elseExpressions.Count());
            Assert.IsNotNull(elseExpressions.First() as OutputExpression);
            Assert.IsNotNull(elseExpressions.Last() as OutputExpression);
        }
    }
}
