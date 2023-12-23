using System.Runtime.Serialization;

namespace RestaurantProject.Datalayer.Exceptions
{
    [Serializable]
    internal class RestaurantRepositoryException : Exception
    {
        public RestaurantRepositoryException()
        {
        }

        public RestaurantRepositoryException(string? message) : base(message)
        {
        }

        public RestaurantRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RestaurantRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}