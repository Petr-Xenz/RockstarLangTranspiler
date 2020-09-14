using static RockstarLangTranspiler.KeyWords;

namespace RockstarLangTranspiler.Tokens
{
    public class ComparsionToken : CombiningToken
    {
        public ComparsionToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }

        public bool IsHigherOrEquals => Value.Equals(High, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Great, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Strong, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Big, System.StringComparison.OrdinalIgnoreCase);

        public bool IsHigher => Value.Equals(Higher, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Greater, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Stronger, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Bigger, System.StringComparison.OrdinalIgnoreCase);

        public bool IsLower => Value.Equals(Lower, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Less, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Smaller, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Weaker, System.StringComparison.OrdinalIgnoreCase);

        public bool IsLowerOrEquals => Value.Equals(Low, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Little, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Small, System.StringComparison.OrdinalIgnoreCase)
            || Value.Equals(Weak, System.StringComparison.OrdinalIgnoreCase);
    }
}