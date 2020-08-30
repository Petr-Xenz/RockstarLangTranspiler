namespace RockstarLangTranspiler.Tokens
{
    public abstract class CombiningToken : Token
    {
        protected CombiningToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}