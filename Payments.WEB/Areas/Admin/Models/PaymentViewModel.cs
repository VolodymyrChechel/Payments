using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Common.Enums;

namespace Payments.WEB.Areas.Admin.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sum of payment")]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Sum must not be negative or less than 1")]
        [Required]
        public decimal PaymentSum { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Recipient { get; set; }
        public string Comment { get; set; }
    }
}
