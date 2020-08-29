namespace RockstarLangTranspiler.Tokens
{
    public class NumberToken : Token
    {
        public NumberToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}