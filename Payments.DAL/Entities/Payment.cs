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
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public decimal PaymentSum { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public string Recipient { get; set; }
        public string Comment { get; set; }
        
        public virtual Account Account { get; set; }
    }
}
