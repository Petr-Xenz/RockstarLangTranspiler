namespace RockstarLangTranspiler.Tokens
{
    public class WhitespaceToken : Token
    {
        public WhitespaceToken(int startLocation) : base(startLocation, 1, " ")
        {
        }
    }
}