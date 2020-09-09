using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class ProperVariableExpressionTests
    {
        [TestMethod]
        public void ProperVariableAddition()
        {
            var tokens = new Token[]
            {
                new NumberToken(0, 0, "1337"),
                new AdditionToken(0, 0, "plus"),
                new WordToken(0, 0, "Cool"),
                new WordToken(0, 0, "Gun"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var rootExpression = tree.RootExpressions.Single() as AdditionExpression;
            Assert.IsNotNull(rootExpression);

            var left = rootExpression.Left as ConstantExpression;
            var right = rootExpression.Right as VariableExpression;

            Assert.IsNotNull(left);
            Assert.IsNotNull(right);

            Assert.AreEqual(1337f, left.Value);
            Assert.AreEqual("Cool_Gun", right.VariableName);
        }

        [TestMethod]
        public void ProperVariablesAddition()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Cool"),
                new WordToken(0, 0, "Gun"),
                new AdditionToken(0, 0, "plus"),
                new WordToken(0, 0, "Cool"),
                new WordToken(0, 0, "Ammo"),
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

            Assert.AreEqual("Cool_Gun", left.VariableName);
            Assert.AreEqual("Cool_Ammo", right.VariableName);
        }

        [TestMethod]
        public void FunctionIvocationWithProperVariable()
        {
            var tokens = new Token[]
            {
                new WordToken(0, 0, "Cooler"),
                new FunctionInvocationToken(0, 0, "taking"),
                new WordToken(0, 0, "Much"),
                new WordToken(0, 0, "Power"),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var rootExpression = tree.RootExpressions.Single() as FunctionInvocationExpression;

            Assert.IsNotNull(rootExpression);
            Assert.AreEqual(1, rootExpression.ArgumentExpressions.Count());

            var variable = rootExpression.ArgumentExpressions.Cast<VariableExpression>().Single();
            Assert.AreEqual("Much_Power", variable.VariableName);
        }

        [TestMethod]
        public void WhileWithLongVariable()
        {
            var tokens = new Token[]
            {
                new WhileToken(0, 0 , "while"),
                new WordToken(0, 0, "We"),
                new WordToken(0, 0, "Have"),
                new WordToken(0, 0, "Much"),
                new WordToken(0, 0, "Power"),
                new EndOfTheLineToken(0, 0),
                new OutputToken(0, 0, "Say"),
                new NumberToken(0, 0, "5"),
                new EndOfTheLineToken(0, 0),
                new EndOfTheLineToken(0, 0),
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());

            var rootExpression = tree.RootExpressions.Single() as WhileExpression;

            Assert.IsNotNull(rootExpression);
            var variable = rootExpression.ConditionExpression as VariableExpression;
            Assert.IsNotNull(rootExpression.ConditionExpression);

            Assert.AreEqual("We_Have_Much_Power", variable.VariableName);

            Assert.IsTrue(rootExpression.InnerExpressions.FirstOrDefault() is OutputExpression);
        }
    }
}
