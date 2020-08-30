using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class AdditionExpressionTests
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
    }
}
