using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockstarLangTranspiler
{
    public class Lexer
    {
        private readonly string _file;
        private int _currentPosition;
        private readonly IReadOnlyList<ITokenFactory<Token>> _tokenFactories;

        public Lexer(string fileContents, IReadOnlyList<ITokenFactory<Token>> tokenFactories)
        {
            if (string.IsNullOrWhiteSpace(fileContents))
            {
                throw new ArgumentException("File is empty", nameof(fileContents));
            }

            _file = fileContents;
            _tokenFactories = tokenFactories ?? throw new ArgumentNullException(nameof(tokenFactories));
        }

        private char PeekNext(int position) => _file[position + 1];

        public IEnumerable<Token> Lex()
        {
            var result = new List<Token>();
            var lines = _file.Split(Environment.NewLine);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var line = lines[lineNumber];
                var words = line.SplitLineByWords();
                foreach(var wordWithPosition in words)
                {
                    var factory = _tokenFactories.FirstOrDefault(f => f.IsValidForToken(wordWithPosition.word));
                    result.Add(factory?.CreateToken(wordWithPosition.position, lineNumber + 1, wordWithPosition.word)
                        ?? new UnknownToken(wordWithPosition.position, wordWithPosition.word.Length, wordWithPosition.word));
                }
                result.Add(new EndOfTheLineToken(line.Length, lineNumber + 1));

            }
            result.Add(new EndOfTheLineToken(0, lines.Length));
            return result;
        }
    }
}
