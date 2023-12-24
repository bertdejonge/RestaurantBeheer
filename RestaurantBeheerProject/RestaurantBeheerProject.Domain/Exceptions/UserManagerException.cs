﻿using System.Runtime.Serialization;

namespace RestaurantProject.Domain.Exceptions {
    [Serializable]
    internal class UserManagerException : Exception {
        public UserManagerException() {
        }

        public UserManagerException(string? message) : base(message) {
        }

        public UserManagerException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}