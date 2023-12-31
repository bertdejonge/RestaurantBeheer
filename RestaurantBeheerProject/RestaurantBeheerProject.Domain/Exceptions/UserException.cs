using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    public class UserException : Exception
    {
        public UserException()
        {
        }

        public UserException(string? message) : base(message)
        {
        }

        public UserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}