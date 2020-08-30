namespace RockstarLangTranspiler.Tokens
{
    public class SubtractionToken : CombiningToken
    {
        public SubtractionToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}