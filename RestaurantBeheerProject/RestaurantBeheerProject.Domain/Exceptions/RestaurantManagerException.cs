using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    internal class RestaurantManagerException : Exception
    {
        public RestaurantManagerException()
        {
        }

        public RestaurantManagerException(string? message) : base(message)
        {
        }

        public RestaurantManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RestaurantManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}