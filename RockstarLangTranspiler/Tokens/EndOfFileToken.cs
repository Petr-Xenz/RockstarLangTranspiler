namespace RockstarLangTranspiler.Tokens
{
    public class EndOfFileToken : Token
    {
        public EndOfFileToken(int startLocation) : base(startLocation, 0, "")
        {
        }
    }

}