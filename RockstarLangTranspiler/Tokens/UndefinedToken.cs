namespace RockstarLangTranspiler.Tokens
{
    public class UndefinedToken : Token
    {
        public UndefinedToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}