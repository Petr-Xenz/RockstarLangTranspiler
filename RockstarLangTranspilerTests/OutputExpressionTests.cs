using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using RockstarLangTranspiler.Tokens;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class OutputExpressionTests
    {
        [TestMethod]
        public void ParseConstantOutputExpression()
        {
            var tokens = new Token[] { new OutputToken(0, 0, ""), new NumberToken(0, 0, "0") };
            var parser = new Parser(tokens);
            var tree = parser.Parse();


            Assert.AreEqual(1, tree.RootExpressions.Count());
            Assert.IsTrue(tree.RootExpressions.First() is OutputExpression);
            var outputExpression = tree.RootExpressions.First() as OutputExpression;
            Assert.IsTrue(outputExpression.ExpressionToOutput is ConstantExpression);
        }

        [TestMethod]
        public void ParseOutputExpresssionWithAddition()
        {
            var tokens = new Token[] 
            { 
                new OutputToken(0, 0, ""), 
                new NumberToken(0, 0, "0"),
                new AdditionToken(0, 0, "0"),
                new NumberToken(0, 0, "0")
            };
            var parser = new Parser(tokens);
            var tree = parser.Parse();
            Assert.AreEqual(1, tree.RootExpressions.Count());
        }
    }
}
