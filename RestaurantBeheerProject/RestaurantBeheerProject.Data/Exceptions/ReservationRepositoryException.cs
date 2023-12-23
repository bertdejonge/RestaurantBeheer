using System.Runtime.Serialization;

namespace RestaurantProject.Datalayer.Exceptions
{
    [Serializable]
    internal class ReservationRepositoryException : Exception
    {
        public ReservationRepositoryException()
        {
        }

        public ReservationRepositoryException(string? message) : base(message)
        {
        }

        public ReservationRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservationRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}