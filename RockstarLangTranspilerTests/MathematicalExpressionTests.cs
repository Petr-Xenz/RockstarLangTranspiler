using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class MathematicalExpressionTests
    {
        [TestMethod]
        public void ParseSimpleAdditionExpression()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "5"), new AdditionToken(0, 0, ""), new NumberToken(0, 0, "55") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is AdditionExpression);
        }

        [TestMethod]
        public void ParseMultipleAdditionExpressions()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "1"), new AdditionToken(0, 0, ""), new NumberToken(0, 0, "2"), new AdditionToken(0, 0, ""), new NumberToken(0, 0, "3") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is AdditionExpression);
            var a = syntaxTree.RootExpressions.Single() as AdditionExpression;

            Assert.IsTrue(a.Left is ConstantExpression);
            Assert.IsTrue(a.Right is AdditionExpression);

            var secondAddition = a.Right as AdditionExpression;
            Assert.IsTrue(secondAddition.Left is ConstantExpression);
            Assert.IsTrue(secondAddition.Right is ConstantExpression);
        }

        [TestMethod]
        public void ParseSimpleSubtractionExpression()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "5"), new SubtractionToken(0, 0, ""), new NumberToken(0, 0, "55") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is SubtractionExpression);
        }

        [TestMethod]
        public void ParseMultipleSubtractionExpressions()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "1"), new SubtractionToken(0, 0, ""), new NumberToken(0, 0, "2"), new SubtractionToken(0, 0, ""), new NumberToken(0, 0, "3") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is SubtractionExpression);
            var s = syntaxTree.RootExpressions.Single() as SubtractionExpression;

            Assert.IsTrue(s.Left is ConstantExpression);
            Assert.IsTrue(s.Right is SubtractionExpression);

            var secondSubtraction = s.Right as SubtractionExpression;
            Assert.IsTrue(secondSubtraction.Left is ConstantExpression);
            Assert.IsTrue(secondSubtraction.Right is ConstantExpression);
        }

        [TestMethod]
        public void ParseSimpleMultiplicationExpression()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "5"), new MultiplicationToken(0, 0, ""), new NumberToken(0, 0, "55") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is MultiplicationExpression);
        }

        [TestMethod]
        public void ParseMultipleMultiplicationExpressions()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "1"), new MultiplicationToken(0, 0, ""), new NumberToken(0, 0, "2"), new MultiplicationToken(0, 0, ""), new NumberToken(0, 0, "3") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is MultiplicationExpression);
            var m = syntaxTree.RootExpressions.Single() as MultiplicationExpression;

            Assert.IsTrue(m.Left is ConstantExpression);
            Assert.IsTrue(m.Right is MultiplicationExpression);

            var second = m.Right as MultiplicationExpression;
            Assert.IsTrue(second.Left is ConstantExpression);
            Assert.IsTrue(second.Right is ConstantExpression);
        }

        [TestMethod]
        public void ParseSimpleDivisionExpression()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "5"), new DivisionToken(0, 0, ""), new NumberToken(0, 0, "55") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is DivisionExpression);
        }

        [TestMethod]
        public void ParseMultipleDivisionExpressions()
        {
            var tokens = new Token[] { new NumberToken(0, 0, "1"), new DivisionToken(0, 0, ""), new NumberToken(0, 0, "2"), new DivisionToken(0, 0, ""), new NumberToken(0, 0, "3") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is DivisionExpression);
            var d = syntaxTree.RootExpressions.Single() as DivisionExpression;

            Assert.IsTrue(d.Left is ConstantExpression);
            Assert.IsTrue(d.Right is DivisionExpression);

            var second = d.Right as DivisionExpression;
            Assert.IsTrue(second.Left is ConstantExpression);
            Assert.IsTrue(second.Right is ConstantExpression);
        }
    }
}
