using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Payments.Common.Enums;

namespace Payments.WEB.Areas.Admin.Models
{
    public class DepositCardViewModel
    {
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CVV { get; set; }

        public string Holder { get; set; }

        [Required]
        public CreditCardType CreditCardTypes { get; set; }
        
        public int? AccountAccountNumber { get; set; }
    }
}