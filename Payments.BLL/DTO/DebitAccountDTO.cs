using System.Collections.Generic;
using Payments.Common.Enums;
using Payments.DAL.Entities;

namespace Payments.BLL.DTO
{
    public class DebitAccountDTO
    {
        public int AccountNumber { get; set; }
        public string Name { get; set; }
        
        public decimal Sum { get; set; }
        public bool IsBlocked { get; set; }

        public Currency Currency { get; set; }
        public string ClientProfileId { get; set; }
    }
}