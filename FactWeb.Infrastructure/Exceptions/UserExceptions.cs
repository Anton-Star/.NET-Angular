using System;

namespace FactWeb.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for when a user already exists
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base() { }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Exception for when a users current password is incorrect
    /// </summary>
    public class UserPasswordIncorrectException : Exception
    {
        public UserPasswordIncorrectException() : base() { }

        public UserPasswordIncorrectException(string message) : base(message)
        {
        }
    }
}
