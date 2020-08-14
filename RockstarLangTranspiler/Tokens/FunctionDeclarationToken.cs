namespace RockstarLangTranspiler.Tokens
{
    public class FunctionDeclarationToken : Token
    {
        public FunctionDeclarationToken(int startLocation, int length, string value) : base(startLocation, length, value)
        {
        }
    }
}