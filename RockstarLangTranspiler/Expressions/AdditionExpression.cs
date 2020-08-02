using System;

namespace RockstarLangTranspiler.Expressions
{
    public class AdditionExpression : IExpression
    {
        private IExpression _left;
        private IExpression _right;

        public AdditionExpression(IExpression left, IExpression right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }
    }
}
