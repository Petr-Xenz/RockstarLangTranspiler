using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class FunctionExpressionsTests
    {
        [TestMethod]
        public void FunctionSingleArgumentDeclarationTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0, 0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.IsTrue(fe.InnerExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void FunctionSingleArgumentDeclarationReturnsFunctionCallTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0, 0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new WordToken(0, 0, "NotSoLeet"),
                new FunctionInvocationToken(0, 0, "taking"),
                new WordToken(0, 0, "lame"),
                new AdditionToken(0, 0, "with"),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);

            Assert.IsTrue(fe.InnerExpressions.Single() is FunctionInvocationExpression);
            var returnExpression = (FunctionInvocationExpression)fe.InnerExpressions.Single();

            Assert.AreEqual("NotSoLeet", returnExpression.Name);
            Assert.IsTrue(returnExpression.ArgumentExpressions.Single() is AdditionExpression);

            var addition = (AdditionExpression)returnExpression.ArgumentExpressions.Single();
            Assert.AreEqual("lame", (addition.Left as VariableExpression)?.VariableName);
            Assert.AreEqual(1337, (addition.Right as ConstantExpression)?.Value);
        }

        [TestMethod]
        public void FunctionMultipleArgumentsDeclarationTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new CommaToken(0, 0),
                new WordToken(0, 0, "shame"),
                new FunctionArgumentSeparatorToken(0, 0, "&"),
                new WordToken(0, 0, "blame"),
                new AndToken(0, 0, "and"),
                new WordToken(0, 0, "fame"),
                new EndOfTheLineToken(0, 0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);

            var arguments = fe.Arguments.ToArray();

            Assert.AreEqual(4, arguments.Length);
            Assert.AreEqual("lame", arguments[0].Name);
            Assert.AreEqual("shame", arguments[1].Name);
            Assert.AreEqual("blame", arguments[2].Name);
            Assert.AreEqual("fame", arguments[3].Name);

            Assert.IsTrue(fe.InnerExpressions.Single() is ConstantExpression);
        }

        [TestMethod]
        public void FunctionSingleArgumentDeclarationWithSingleLineComplexExpressionTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "DoubleLame"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0, 0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new WordToken(0, 0, "lame"),
                new AdditionToken(0, 0, "with"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("DoubleLame", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.IsTrue(fe.InnerExpressions.Single() is AdditionExpression);
        }

        [TestMethod]
        public void FunctionWithMultipleExpressionsDeclarationTest()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Leet"),
                new FunctionDeclarationToken(0, 0, "takes"),
                new WordToken(0, 0, "lame"),
                new EndOfTheLineToken(0, 0),
                new OutputToken(0, 0, ""),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0, 0),
                new FunctionReturnToken(0, 0, "Gives"),
                new FunctionReturnToken(0, 0, "back"),
                new NumberToken(0, 0, "1337"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.Single() is FunctionExpression);
            var fe = (FunctionExpression)tree.RootExpressions.Single();
            Assert.AreEqual("Leet", fe.Name);
            Assert.AreEqual(1, fe.Arguments.Count());
            Assert.AreEqual("lame", fe.Arguments.Single().Name);
            Assert.AreEqual(2, fe.InnerExpressions.Count());
            Assert.IsTrue(fe.InnerExpressions.First() is OutputExpression);
            Assert.IsTrue(fe.InnerExpressions.Last() is ConstantExpression);
        }

        [TestMethod]
        public void ParseFunctionWithInnerWhile()
        {
            var src = @"Midnight takes your heart and your soul
While your heart is as high as your soul
Put your heart without your soul into your heart

Give back your heart";

            var tokens = new Lexer(src).Lex();
            var syntaxTree = new Parser(tokens).Parse();

            Assert.AreEqual(1, syntaxTree.RootExpressions.Count());
            var fe = syntaxTree.RootExpressions.Single() as FunctionExpression;

            Assert.IsNotNull(fe);
            var args = fe.Arguments.ToArray();
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("your_heart", args[0].Name);
            Assert.AreEqual("your_soul", args[1].Name);

            Assert.AreEqual(2, fe.InnerExpressions.Count());
            var we = fe.InnerExpressions.First() as WhileExpression;
            var re = fe.InnerExpressions.Last() as VariableExpression;

            Assert.IsNotNull(we);
            Assert.IsNotNull(re);

        }
    }

}
