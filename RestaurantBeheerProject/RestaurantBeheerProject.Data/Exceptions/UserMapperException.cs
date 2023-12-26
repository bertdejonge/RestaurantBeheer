using System.Runtime.Serialization;

namespace RestaurantProject.Datalayer.Exceptions
{
    [Serializable]
    internal class UserMapperException : Exception
    {
        public UserMapperException()
        {
        }

        public UserMapperException(string? message) : base(message)
        {
        }

        public UserMapperException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}