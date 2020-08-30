﻿using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AdditionTokenFactory : KeyWordBasedTokenFactory<AdditionToken>
    {
        private static readonly string[] _keyWords = { Plus, With };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override AdditionToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new AdditionToken(linePosition, lineNumber, value);
        }
    }

    public class SubtractionTokenFactory : KeyWordBasedTokenFactory<SubtractionToken>
    {
        private static readonly string[] _keyWords = { Minus, Without };
        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value) => false;

        public override SubtractionToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new SubtractionToken(linePosition, lineNumber, value);
        }
    }
}
