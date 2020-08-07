﻿using RockstarLangTranspiler.Tokens;
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
        private readonly IReadOnlyList<TokenFactory> _tokenFactories;

        public Lexer(string fileContents, IReadOnlyList<TokenFactory> tokenFactories)
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
            foreach(var line in lines)
            {
                var words = line.SplitLineByWords();
                foreach(var wordWithPosition in words)
                {
                    var factory = _tokenFactories.FirstOrDefault(f => f.IsValidForToken(wordWithPosition.word));
                    result.Add(factory?.CreateToken(wordWithPosition.position, wordWithPosition.word)
                        ?? new UnknownToken(wordWithPosition.position, wordWithPosition.word.Length, wordWithPosition.word));
                }
                result.Add(new EndOfTheLineToken(line.Length));

            }
            return result;
        }
    }
}
