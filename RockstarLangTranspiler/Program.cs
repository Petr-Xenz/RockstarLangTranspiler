using RockstarLangTranspiler.Tokens;
using RockstarLangTranspiler.Tokens.TokenFactories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace RockstarLangTranspiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length < 1 && !File.Exists(args[0]))
                throw new ArgumentException("File path is expected");

            var file = File.ReadAllText(args[0]);

            Console.WriteLine(file);
            Console.WriteLine();

            var factories = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("TokenFactory"))
                .Where(t => !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .OfType<ITokenFactory<Token>>()
                .ToList();

            var lexer = new Lexer(file, factories);
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
