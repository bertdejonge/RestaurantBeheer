using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    internal class CheckPropertyException : Exception
    {
        public CheckPropertyException()
        {
        }

        public CheckPropertyException(string? message) : base(message)
        {
        }

        public CheckPropertyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CheckPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}