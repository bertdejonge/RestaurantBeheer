using System.Runtime.Serialization;

namespace RestaurantProject.Datalayer.Exceptions
{
    [Serializable]
    internal class ContextException : Exception
    {
        public ContextException()
        {
        }

        public ContextException(string? message) : base(message)
        {
        }

        public ContextException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}