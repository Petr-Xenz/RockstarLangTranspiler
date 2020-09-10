namespace RockstarLangTranspiler.Tokens
{
    public class CommentToken : Token
    {
        public CommentToken(int linePosition, int lineNumber, string value) : base(linePosition, lineNumber, value)
        {
        }

        public bool IsCommentStart => Value == "(";
    }
}