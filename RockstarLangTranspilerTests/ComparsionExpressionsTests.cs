using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;
using static RockstarLangTranspiler.KeyWords;

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
        public void IfNonEqualityExpression()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new NotEqualsToken(0, 0, "ain't"),
                new NumberToken(0, 0, "2"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "3"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is IfExpression);
            var e = (IfExpression)tree.RootExpressions.First();

            Assert.IsTrue(e.ConditionExpression is NotEqualExpression);
            var equalityExpression = e.ConditionExpression as NotEqualExpression;
            Assert.IsTrue(equalityExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(equalityExpression.Right is ConstantExpression cr && cr.Value == 2);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }


        [TestMethod]
        public void WhileNonEqualityExpression()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
                new NotToken(0, 0, "not"),
                new NumberToken(0, 0, "2"),
                new EndOfTheLineToken(0, 0),
                    new OutputToken(0, 0, "say"),
                    new NumberToken(0, 0, "3"),
                    new EndOfTheLineToken(0, 0),
                new EndOfFileToken(0, 0),
            };

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is WhileExpression);
            var e = (WhileExpression)tree.RootExpressions.First();

            Assert.IsTrue(e.ConditionExpression is NotEqualExpression);
            var equalityExpression = e.ConditionExpression as NotEqualExpression;
            Assert.IsTrue(equalityExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(equalityExpression.Right is ConstantExpression cr && cr.Value == 2);

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

        [TestMethod]
        public void ParseLetIsAssigmentExpressionWithComparsion()
        {
            var tokens = new Token[] 
            { 
                new AssigmentToken(0, 0, "Let"), 
                new WordToken(0, 0, "x"), 
                new IsToken(0, 0, "is"), 
                    new NumberToken(0, 0, "33"),
                    new IsToken(0, 0, "is"),
                    new ComparsionToken(0, 0, Higher),
                    new ComparsionToken(0, 0, Than),
                    new NumberToken(0, 0, "31"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("x", e.Variable.VariableName);
            Assert.IsTrue(e.AssigmentExpression is GreaterThanExpression);

            var assigment = (GreaterThanExpression)e.AssigmentExpression;
            Assert.IsFalse(assigment.IsOrEquals);

            Assert.IsTrue(assigment.Left is ConstantExpression cl && cl.Value == 33);
            Assert.IsTrue(assigment.Right is ConstantExpression cr && cr.Value == 31);
        }

        [TestMethod]
        public void IfGreaterOrEqualExpression()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
                new ComparsionToken(0, 0, As),
                new ComparsionToken(0, 0, Great),
                new ComparsionToken(0, 0, As),
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

            Assert.IsTrue(e.ConditionExpression is GreaterThanExpression ce && ce.IsOrEquals);
            var comparsionExpression = e.ConditionExpression as GreaterThanExpression;
            Assert.IsTrue(comparsionExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(comparsionExpression.Right is ConstantExpression cr && cr.Value == 1);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }

        [TestMethod]
        public void IfLessOrEqualExpression()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
                new ComparsionToken(0, 0, As),
                new ComparsionToken(0, 0, Weak),
                new ComparsionToken(0, 0, As),
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

            Assert.IsTrue(e.ConditionExpression is LessThanExpression ce && ce.IsOrEquals);
            var comparsionExpression = e.ConditionExpression as LessThanExpression;
            Assert.IsTrue(comparsionExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(comparsionExpression.Right is ConstantExpression cr && cr.Value == 1);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }

        [TestMethod]
        public void IfLesserExpression()
        {
            var tokens = new Token[]
            {
                new IfToken(0, 0, "if"),
                new NumberToken(0, 0, "1"),
                new IsToken(0, 0, "is"),
                new ComparsionToken(0, 0, Weaker),
                new ComparsionToken(0, 0, Than),
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

            Assert.IsTrue(e.ConditionExpression is LessThanExpression ce && !ce.IsOrEquals);
            var comparsionExpression = e.ConditionExpression as LessThanExpression;
            Assert.IsTrue(comparsionExpression.Left is ConstantExpression cl && cl.Value == 1);
            Assert.IsTrue(comparsionExpression.Right is ConstantExpression cr && cr.Value == 1);

            Assert.AreEqual(1, e.InnerExpressions.Count());
        }
    }
}
