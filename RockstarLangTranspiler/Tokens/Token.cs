namespace RockstarLangTranspiler.Tokens
{
    public abstract class Token
    {
        protected Token(int startLocation, int length, string value)
        {
            StartLocation = startLocation;
            Length = length;
            Value = value;
        }

        public int StartLocation { get; }

        public int Length { get; }

        public string Value { get; }
    }

}