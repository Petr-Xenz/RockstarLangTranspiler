using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class SubtractionExpressionTests
    {
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
    }
}
