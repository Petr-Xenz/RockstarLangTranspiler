namespace RockstarLangTranspiler.Tokens
{
    public class WhitespaceToken : Token
    {
        public WhitespaceToken(int linePosition, int lineNumber) : base(linePosition, lineNumber, " ")
        {
        }
    }
}