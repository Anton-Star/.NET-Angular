using System;

namespace FactWeb.Infrastructure.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException() : base("Not Authorized")
        {
        }

        public NotAuthorizedException(string message): base(message) { }
    }
}
