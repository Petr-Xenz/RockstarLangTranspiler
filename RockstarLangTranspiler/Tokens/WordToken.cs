namespace RockstarLangTranspiler.Tokens
{
    public class WordToken : Token
    {
        public WordToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}