﻿using System.Collections.Generic;

namespace RockstarLangTranspiler.Tokens.TokenFactories
{
    public class IfTokenFactory : KeyWordBasedTokenFactory<IfToken>
    {
        private static readonly string[] _keyWords = new[] { "if" };

        public override IReadOnlyCollection<string> KeyWords => _keyWords;

        public override bool CanParseFarther(string value)
        {
            throw new System.NotImplementedException();
        }

        public override IfToken CreateToken(int linePosition, int lineNumber, string value)
        {
            return new IfToken(linePosition, lineNumber, value);
        }
    }
}