namespace RockstarLangTranspiler.Expressions
{
    public class OutputExpression : IExpression
    {
        private readonly IExpression _expressionToOutput;

        public OutputExpression(IExpression expressionToOutput)
        {
            _expressionToOutput = expressionToOutput ?? throw new System.ArgumentNullException(nameof(expressionToOutput));
        }

        public IExpression ExpressionToOutput => _expressionToOutput;
    }
}
