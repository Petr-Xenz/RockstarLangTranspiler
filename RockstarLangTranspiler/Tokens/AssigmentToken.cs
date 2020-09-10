namespace RockstarLangTranspiler.Tokens
{
    public class AssigmentToken : Token
    {
        public AssigmentToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }
    }
}