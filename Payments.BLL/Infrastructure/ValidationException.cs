using System;

namespace Payments.BLL.Infrastructure
{
    // custom exception to send exception on presentation level
    public class ValidationException : Exception
    {
        public string Property { get; set; }

        public ValidationException(string message, string property) : base(message)
        {
            Property = property;
        }
    }
}