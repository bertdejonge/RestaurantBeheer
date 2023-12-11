using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions
{
    [Serializable]
    internal class TableException : Exception
    {
        public TableException()
        {
        }

        public TableException(string? message) : base(message)
        {
        }

        public TableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}