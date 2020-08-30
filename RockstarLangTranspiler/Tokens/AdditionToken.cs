namespace RockstarLangTranspiler.Tokens
{
    public class AdditionToken : CombiningToken
    {
        public AdditionToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}