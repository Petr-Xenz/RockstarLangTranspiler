namespace RockstarLangTranspiler.Tokens
{
    public class IncrementToken : Token
    {
        public IncrementToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }

        public bool IsAuxiliary  => Value.Equals(KeyWords.Up, System.StringComparison.OrdinalIgnoreCase);
    }
}