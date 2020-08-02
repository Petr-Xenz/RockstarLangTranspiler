namespace RockstarLangTranspiler.Tokens
{
    public sealed class UnknownToken : Token
    {
        public UnknownToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}