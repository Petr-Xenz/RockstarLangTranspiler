using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class AssigmentExpressionTests
    {
        [TestMethod]
        public void ParseSimpleConstantAssigment()
        {
            var tokens = new Token[] { new WordToken(0, 0, "Foo"), new AssigmentToken(0, 0, ""), new NumberToken(0, 0, "55") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("Foo", e.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 55f);
        }

        [TestMethod]
        public void ParseLetIsAssigmentExpression()
        {
            var tokens = new Token[] { new AssigmentToken(0, 0, "Let"), new WordToken(0, 0, "x"), new AssigmentToken(0, 0, "is"), new NumberToken(0, 0, "33") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("x", e.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 33f);
        }

        [TestMethod]
        public void ParsePutIntoAssigmentExpression()
        {
            var tokens = new Token[] { new AssigmentToken(0, 0, "Put"), new NumberToken(0, 0, "5"), new AssigmentToken(0, 0, "into"), new WordToken(0, 0, "x") };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("x", e.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 5f);
        }
    }
}
