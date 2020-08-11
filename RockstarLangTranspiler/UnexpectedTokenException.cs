using System;
using System.Runtime.Serialization;

namespace RockstarLangTranspiler
{
    [Serializable]
    internal class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException()
        {
        }

        public UnexpectedTokenException(string message) : base(message)
        {
        }

        public UnexpectedTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}