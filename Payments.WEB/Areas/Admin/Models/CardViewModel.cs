using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Payments.Common.Enums;
using Payments.Common.StaticData;

namespace Payments.WEB.Areas.Admin.Models
{
    public class CardViewModel
    {
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CVV { get; set; }

        [RegularExpression(RegExStrings.HolderName, ErrorMessage = "Holder fiels has incorrect format")]
        public string Holder { get; set; }

        [Required]
        [Display(Name="Type")]
        public CreditCardType CreditCardTypes { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int? AccountAccountNumber { get; set; }
    }
}