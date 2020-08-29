namespace RockstarLangTranspiler.Tokens
{
    public class EndOfFileToken : Token
    {
        public EndOfFileToken(int linePosition, int lineNumber) : base(linePosition, lineNumber, "")
        {
        }
    }

}