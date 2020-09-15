namespace RockstarLangTranspiler.Tokens
{
    public class DecrementToken : Token
    {
        public DecrementToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }

        public bool IsAuxiliary => Value.Equals(KeyWords.Down, System.StringComparison.OrdinalIgnoreCase);
    }
}