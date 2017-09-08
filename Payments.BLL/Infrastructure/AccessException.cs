using System;

namespace Payments.BLL.Infrastructure
{
    // custom exception to send access exception on presentation level
    public class AccessException : Exception
    {
        public string Property { get; set; }

        public AccessException(string message, string property) : base(message)
        {
            Property = property;
        }
    }
}