namespace RockstarLangTranspiler.Tokens
{
    public class NullToken : Token
    {
        public NullToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}