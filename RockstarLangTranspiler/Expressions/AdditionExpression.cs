using System;

namespace RockstarLangTranspiler.Expressions
{
    public class AdditionExpression : IExpression
    {
        public AdditionExpression(IExpression left, IExpression right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public IExpression Left { get; }

        public IExpression Right { get; }
    }
}
