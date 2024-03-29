﻿using System.Collections.Generic;
using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class AndTokenFactory : KeyWordBasedTokenFactory<AndToken>
    {
        private static readonly string[] _keyWords = new[] { And };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override AndToken CreateToken(int linePosition, int lineNumber, string value) => new AndToken(linePosition, lineNumber, value);
    }
}
