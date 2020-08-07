using RockstarLangTranspiler.Tokens.TokenFactories;
using System;
using System.Collections.Generic;
using System.IO;

namespace RockstarLangTranspiler
{
    class Program
    {
        private static IReadOnlyList<TokenFactory> _tokensFactories = new TokenFactory[]
        {
            new OutputTokenFactory(),
            new NumberTokenFactory(),
            new AdditionTokenFactory(),
            new WhitespaceTokenFactory(),
        };

        static void Main(string[] args)
        {
            if (args.Length < 1 && !File.Exists(args[0]))
                throw new ArgumentException("File path is expected");

            var lexer = new Lexer(File.ReadAllText(args[0]), _tokensFactories);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var syntaxTree = parser.Parse();

            var transpiler = new JsTranspiler();
            var js = transpiler.Transpile(syntaxTree);

            Console.WriteLine(js);

            if (args.Length >= 2 && File.Exists(args[1]))
            {
                File.WriteAllText(args[1], js);
            }
        }

    }
}
