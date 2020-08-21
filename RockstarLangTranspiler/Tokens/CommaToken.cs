namespace RockstarLangTranspiler.Tokens
{
    public class CommaToken : Token
    {
        public CommaToken(int startLocation, int length) : base(startLocation, length, ",")
        {
        }
    }
}