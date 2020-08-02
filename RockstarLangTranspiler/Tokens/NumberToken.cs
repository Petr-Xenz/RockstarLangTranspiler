namespace RockstarLangTranspiler.Tokens
{
    public class NumberToken : Token
    {
        public NumberToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}