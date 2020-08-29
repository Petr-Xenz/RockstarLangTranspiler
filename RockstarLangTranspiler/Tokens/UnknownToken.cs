namespace RockstarLangTranspiler.Tokens
{
    public sealed class UnknownToken : Token
    {
        public UnknownToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}