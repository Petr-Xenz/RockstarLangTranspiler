using System.Collections.Generic;
using System.Text;

namespace RockstarLangTranspiler
{
    public interface ITranspiler
    {
        string Transpile(SyntaxTree syntaxTree);
    }
}
