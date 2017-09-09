using System;

namespace Payments.BLL.Infrastructure
{
    // custom exception to send exception on presentation level
    // contain additional information about property with wrong data
    public class ValidationException : Exception
    {
        public string Property { get; set; }

        public ValidationException(string message, string property) :
            base(message)
        {
            Property = property;
        }
    }
}