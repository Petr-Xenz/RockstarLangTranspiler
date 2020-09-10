using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class ParsingExpressionsWithCommentsTests
    {
        [TestMethod]
        public void CommentAtTheStartOfTheLine()
        {
            var tokens = new Token[]
            {
                new CommentToken(0, 0, "("),
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new CommentToken(0 ,0, ")"),
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("Foo", e.Variable.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 55f);
        }

        [TestMethod]
        public void CommentAtThEndOfTheLine()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new CommentToken(0, 0, "("),
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new CommentToken(0 ,0, ")"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("Foo", e.Variable.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 55f);
        }

        [TestMethod]
        public void CommentWithOpenCommentTokenInside()
        {
            var tokens = new Token[]
            {
                new CommentToken(0, 0, "("),
                new WordToken(0, 0, "Foo"),
                new CommentToken(0, 0, "("),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new CommentToken(0 ,0, ")"),
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, ""),
                new NumberToken(0, 0, "55"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("Foo", e.Variable.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 55f);
        }

        [TestMethod]
        public void CommentInTheMiddleOfExpression()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Foo"),
                new AssigmentToken(0, 0, "is"),
                new CommentToken(0, 0, "("),
                new WordToken(0, 0, "variable"),
                new WordToken(0, 0, "assigment"),
                new CommentToken(0 ,0, ")"),
                new NumberToken(0, 0, "55"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);

            var syntaxTree = parser.Parse();
            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            Assert.IsTrue(syntaxTree.RootExpressions.Single() is VariableAssigmentExpression);
            var e = syntaxTree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.AreEqual("Foo", e.Variable.VariableName);
            Assert.IsTrue(e.AssigmentExpression is ConstantExpression c && c.Value == 55f);
        }
    }
}
