using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Common.Enums;

namespace Payments.DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public decimal TransactionSum { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public TransactionStatus TransactionStatus { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public string Recipient { get; set; }
        public string Comment { get; set; }
        
        public virtual Account Account { get; set; }
    }
}
