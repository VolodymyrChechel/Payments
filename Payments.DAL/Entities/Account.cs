using Payments.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace Payments.DAL.Entities
{
    public class Account
    {
        [Key]
        public int AccountNumber { get; set; }
        public string Name { get; set; }

        public decimal Sum { get; set; }
        public bool IsBlocked { get; set; }

        public Currency Currency { get; set; }

        public string ClientProfileId { get; set; }

        public virtual ClientProfile ClientProfile { get; set; }
        public virtual ICollection<Card> Cards { get; set; } 
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UnblockAccountRequest> UnblockAccountRequests{ get; set; }
    }
}