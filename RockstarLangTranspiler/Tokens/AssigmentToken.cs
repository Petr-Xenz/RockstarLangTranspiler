namespace RockstarLangTranspiler.Tokens
{
    public class AssigmentToken : Token
    {
        public AssigmentToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }

}