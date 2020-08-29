namespace RockstarLangTranspiler.Tokens
{
    public class CommaToken : Token
    {
        public CommaToken(int linePosition, int lineNumber) : base(linePosition, lineNumber, ",")
        {
        }
    }
}