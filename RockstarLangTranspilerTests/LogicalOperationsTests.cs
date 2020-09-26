using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;
using RockstarLangTranspiler.Expressions;
using System.Linq;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class LogicalOperationsTests
    {
        [TestMethod]
        public void SimpleConjunction()
        {
            var src = "live and nothing";
            var tokens = new Lexer(src).Lex();

            var tree = new Parser(tokens).Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var e = tree.RootExpressions.First() as ConjunctionExpression;
            Assert.IsNotNull(e);

            Assert.IsTrue(e.Left is VariableExpression);
            Assert.IsTrue(e.Right is NullExpression);
        }

        [TestMethod]
        public void SimpleDisjunction()
        {
            var src = "live or wallet";
            var tokens = new Lexer(src).Lex();

            var tree = new Parser(tokens).Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var e = tree.RootExpressions.First() as DisjunctionExpression;
            Assert.IsNotNull(e);

            Assert.IsTrue(e.Left is VariableExpression);
            Assert.IsTrue(e.Right is VariableExpression);
        }

        [TestMethod]
        public void SimpleJointDenial()
        {
            var src = "life nor death";
            var tokens = new Lexer(src).Lex();

            var tree = new Parser(tokens).Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var e = tree.RootExpressions.First() as JointDenialExpression;
            Assert.IsNotNull(e);

            Assert.IsTrue(e.Left is VariableExpression);
            Assert.IsTrue(e.Right is VariableExpression);
        }

        [TestMethod]
        public void AssigmentOfAllCompountOperations()
        {
            var src = "Life is pain and suffering or bliss";
            var tokens = new Lexer(src).Lex();

            var tree = new Parser(tokens).Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var e = tree.RootExpressions.First() as VariableAssigmentExpression;
            Assert.IsNotNull(e);

            Assert.AreEqual("Life", e.Variable.VariableName);

            var ae = e.AssigmentExpression as ConjunctionExpression;
            Assert.IsNotNull(ae);
            Assert.IsTrue(ae.Right is DisjunctionExpression);
            Assert.IsTrue(ae.Left is VariableExpression p && p.VariableName == "pain");
            

            var right = ae.Right as DisjunctionExpression;

            Assert.IsTrue(right.Left is VariableExpression s && s.VariableName == "suffering");
            Assert.IsTrue(right.Right is VariableExpression b && b.VariableName == "bliss");
        }

        [TestMethod]
        public void PutAssigmentOfAllCompountOperations()
        {
            var src = "Put pain and suffering or bliss into Life";
            var tokens = new Lexer(src).Lex();

            var tree = new Parser(tokens).Parse();

            Assert.AreEqual(1, tree.RootExpressions.Count());
            var e = tree.RootExpressions.First() as VariableAssigmentExpression;
            Assert.IsNotNull(e);

            Assert.AreEqual("Life", e.Variable.VariableName);

            var ae = e.AssigmentExpression as ConjunctionExpression;
            Assert.IsNotNull(ae);
            Assert.IsTrue(ae.Left is VariableExpression p && p.VariableName == "pain");

            var right = ae.Right as DisjunctionExpression;

            Assert.IsTrue(right.Left is VariableExpression s && s.VariableName == "suffering");
            Assert.IsTrue(right.Right is VariableExpression b && b.VariableName == "bliss");
        }
    }
}
