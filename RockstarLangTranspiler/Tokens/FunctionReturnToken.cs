namespace RockstarLangTranspiler.Tokens
{
    public class FunctionReturnToken : Token
    {
        public FunctionReturnToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}