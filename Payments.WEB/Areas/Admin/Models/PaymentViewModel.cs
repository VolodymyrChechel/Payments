using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Common.Enums;
using Payments.Common.StaticData;

namespace Payments.WEB.Areas.Admin.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sum of payment")]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Sum must not be negative or less than 1")]
        [RegularExpression(RegExStrings.SumNumber, ErrorMessage = "Sum is not correct")]
        [Required]
        public decimal PaymentSum { get; set; }
        
        public PaymentType PaymentType { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        public string Recipient { get; set; }
        public string Comment { get; set; }

        [Display(Name = "Account number")]
        public int AccountAccountNumber { get; set; }

    }
}
