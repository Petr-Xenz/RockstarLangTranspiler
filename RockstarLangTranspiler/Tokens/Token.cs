namespace RockstarLangTranspiler.Tokens
{
    public abstract class Token
    {
        protected Token(int linePosition, int lineNumber, string value)
        {
            LinePosition = linePosition;
            LineNumber = lineNumber;
            Length = value.Length;
            Value = value;

        }

        public int LinePosition { get; }

        public int LineNumber { get; }

        public int Length { get; }

        public string Value { get; }
    }
         

}