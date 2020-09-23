using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class CommonVariableExpressionTests
    {
        [TestMethod]
        public void ParsingProperVariableInSimpleAssigment()
        {
            var tokens = new Token[]
            {
                new CommonVariablePrefixToken(0, 0, "a"),
                new WordToken(0, 0, "boy"),
                new IsToken(0, 0, "is"),
                new UndefinedToken(0, 0, "mysterious"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var assigmentExpression = tree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.IsNotNull(assigmentExpression);

            Assert.AreEqual("a_boy", assigmentExpression.Variable.VariableName);
        }

        [TestMethod]
        public void ParsingProperVariableInAssigment()
        {
            var tokens = new Token[]
            {
                new AssigmentToken(0, 0, "put"),
                new UndefinedToken(0, 0, "mysterious"),
                new AssigmentToken(0, 0, "into"),
                new CommonVariablePrefixToken(0, 0, "a"),
                new WordToken(0, 0, "boy"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var assigmentExpression = tree.RootExpressions.Single() as VariableAssigmentExpression;
            Assert.IsNotNull(assigmentExpression);

            Assert.AreEqual("a_boy", assigmentExpression.Variable.VariableName);
        }

        [TestMethod]
        public void AdditionExpressionWithProperVariables()
        {
            var tokens = new Token[]
            {
                new CommonVariablePrefixToken(0, 0, "a"),
                new WordToken(0, 0, "boy"),
                new AdditionToken(0, 0, "plus"),
                new CommonVariablePrefixToken(0, 0, "the"),
                new WordToken(0, 0, "gun"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var rootExpression = tree.RootExpressions.Single() as AdditionExpression;
            Assert.IsNotNull(rootExpression);

            var left = rootExpression.Left as VariableExpression;
            var right = rootExpression.Right as VariableExpression;

            Assert.IsNotNull(left);
            Assert.IsNotNull(right);

            Assert.AreEqual("a_boy", left.VariableName);
            Assert.AreEqual("the_gun", right.VariableName);
        }
    }
}
