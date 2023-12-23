using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    internal class ReservationManagerException : Exception
    {
        public ReservationManagerException()
        {
        }

        public ReservationManagerException(string? message) : base(message)
        {
        }

        public ReservationManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}