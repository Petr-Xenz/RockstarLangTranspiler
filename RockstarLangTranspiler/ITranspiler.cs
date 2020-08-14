using System.Collections.Generic;
using System.Text;

namespace RockstarLangTranspiler
{
    public interface ITranspiler
    {
        string Transpile(SyntaxTree syntaxTree);
    }

    public class FunctionArgument
    {
        public FunctionArgument(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("Argument must have a name", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}
