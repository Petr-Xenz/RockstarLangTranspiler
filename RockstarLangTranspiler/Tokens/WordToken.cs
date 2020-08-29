namespace RockstarLangTranspiler.Tokens
{
    public class WordToken : Token
    {
        public WordToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}