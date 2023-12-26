using System.Runtime.Serialization;

namespace RestaurantProject.API.Exceptions
{
    [Serializable]
    internal class MapToDomainException : Exception
    {
        public MapToDomainException()
        {
        }

        public MapToDomainException(string? message) : base(message)
        {
        }

        public MapToDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}