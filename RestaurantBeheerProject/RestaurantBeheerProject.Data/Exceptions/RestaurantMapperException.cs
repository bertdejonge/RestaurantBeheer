using System.Runtime.Serialization;

namespace RestaurantProject.Datalayer.Exceptions
{
    [Serializable]
    internal class RestaurantMapperException : Exception
    {
        public RestaurantMapperException()
        {
        }

        public RestaurantMapperException(string? message) : base(message)
        {
        }

        public RestaurantMapperException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RestaurantMapperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}