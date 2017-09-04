using System.ComponentModel.DataAnnotations;
using Payments.Common.Enums;

namespace Payments.WEB.Areas.Admin.Models
{
    public class DebitAccountViewModel
    {
        public int AccountNumber { get; set; }
        public string Name { get; set; }


        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Sum must not be negative")]
        public decimal Sum { get; set; }

        [Display(Name="Block account")]
        public bool IsBlocked { get; set; }

        [Required]
        public Currency Currency { get; set; }
        public string ClientProfileId { get; set; }
    }
}