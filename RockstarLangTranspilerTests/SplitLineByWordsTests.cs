using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockstarLangTranspiler;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class SplitLineByWordsTests
    {
        [TestMethod]
        public void SplitTwoWords()
        {
            var test = "foo   bar";
            var splitted = test.SplitLineByWords();
            Assert.AreEqual(2, splitted.Length);
            Assert.AreEqual(("foo", 0), splitted[0]);
            Assert.AreEqual(("bar", 6), splitted[1]);
        }

        [TestMethod]
        public void SplitThreeWords()
        {
            var test = "foo   bar baz";
            var splitted = test.SplitLineByWords();
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual(("foo", 0), splitted[0]);
            Assert.AreEqual(("bar", 6), splitted[1]);
            Assert.AreEqual(("baz", 10), splitted[2]);
        }

        [TestMethod]
        public void SplitWordsWithSpacesAtEdges()
        {
            var test = " foo   bar baz ";
            var splitted = test.SplitLineByWords();
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual(("foo", 1), splitted[0]);
            Assert.AreEqual(("bar", 7), splitted[1]);
            Assert.AreEqual(("baz", 11), splitted[2]);
        }

        [TestMethod]
        public void SplitsCommaAsSeparateWord()
        {
            var input = "foo, bar";
            var splitted = input.SplitLineByWords();
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual("foo", splitted[0].word);
            Assert.AreEqual(",", splitted[1].word);
            Assert.AreEqual("bar", splitted[2].word);
        }

        [TestMethod]
        public void SplitsCommaAsSeparateWordWithoutWhitespace()
        {
            var input = "foo,bar";
            var splitted = input.SplitLineByWords();
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual("foo", splitted[0].word);
            Assert.AreEqual(",", splitted[1].word);
            Assert.AreEqual("bar", splitted[2].word);
        }

        [TestMethod]
        public void SplitMultipleCommasInTheRaw()
        {
            var input = ",,,";
            var splitted = input.SplitLineByWords();
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual(",", splitted[0].word);
            Assert.AreEqual(",", splitted[1].word);
            Assert.AreEqual(",", splitted[2].word);
        }
    }
}
