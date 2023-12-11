using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    internal class ReservationException : Exception
    {
        public ReservationException()
        {
        }

        public ReservationException(string? message) : base(message)
        {
        }

        public ReservationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}