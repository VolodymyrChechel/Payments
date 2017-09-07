using System;
using Payments.Common.Enums;

namespace Payments.DAL.Entities
{
    // db set to store unblock account request
    public class UnblockAccountRequest
    {
        public int Id { get; set; }
        
        public UnblockRequestStatus Status { get; set; }
        public DateTime RequestTime { get; set; }

        public int AccountAccountNumber { get; set; }
        public virtual Account Account { get; set; }
    }
}