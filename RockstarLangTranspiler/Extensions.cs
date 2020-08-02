using System;
using System.Collections.Generic;
using System.Text;

namespace RockstarLangTranspiler
{
    public static class Extensions
    {
        public static (string word, int position)[] SplitLineByWords(this string line)
        {
            var result = new List<(string word, int position)>();
            var position = 0;
            var wordStart = 0;
            while (position < line.Length)
            {
                if (IsSpace(position))
                {
                    position++;
                    wordStart = position;
                    continue;
                }
                else
                {
                    position++;
                    if (position < line.Length && IsSpace(position) || position == line.Length)
                    {
                        result.Add((line.Substring(wordStart, position - wordStart), wordStart));
                    }
                }
            }

            return result.ToArray();

            bool IsSpace(int current) => line[current] == ' ' || line[current] == '\t';
        }
    }
}
