using System.Runtime.Serialization;

namespace RockstarLangTranspiler.Tokens
{
    public class WordToken : Token
    {
        public WordToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }

        public bool IsStartingWithUpper => char.IsUpper(Value[0]);
    }
}