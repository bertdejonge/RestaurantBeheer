using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Util
{
    [Serializable]
    internal class ContactInfoException : Exception
    {
        public ContactInfoException()
        {
        }

        public ContactInfoException(string? message) : base(message)
        {
        }

        public ContactInfoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}