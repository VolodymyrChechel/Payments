using System;
using Payments.Common.Enums;

namespace Payments.BLL.DTO
{
    // DTO class for unblock requests
    public class UnblockAccountRequestDTO
    {
        public int Id { get; set; }
        
        public UnblockRequestStatus Status { get; set; }
        public DateTime RequestTime { get; set; }

        public int AccountAccountNumber { get; set; }
    }
}