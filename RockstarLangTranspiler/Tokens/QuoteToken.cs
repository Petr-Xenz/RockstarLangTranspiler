namespace RockstarLangTranspiler.Tokens
{
    public class QuoteToken : Token
    {
        public QuoteToken(int linePosition, int lineNumber) : base(linePosition, lineNumber, "\"")
        {
        }
    }
}