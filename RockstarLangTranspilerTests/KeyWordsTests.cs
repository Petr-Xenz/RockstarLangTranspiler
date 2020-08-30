using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspilerTests
{
    [TestClass]
    public class KeyWordsTests
    {
        [TestMethod]
        public void AreAllKeyWordsUnique()
        {
            var repeated = new List<string>();
            var checkSet = new HashSet<string>();

            foreach (var word in AllReservedWords)
            {
                if (checkSet.Contains(word))
                {
                    repeated.Add(word);
                    continue;
                }

                checkSet.Add(word);
            }

            if (repeated.Any())
            {
                throw new Exception(repeated.Aggregate((p, c) => $"{p}, {c}"));
            }
        }
    }
}
