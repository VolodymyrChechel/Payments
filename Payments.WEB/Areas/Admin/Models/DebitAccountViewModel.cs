using System.ComponentModel.DataAnnotations;
using Payments.Common.Enums;

namespace Payments.WEB.Areas.Admin.Models
{
    public class DebitAccountViewModel
    {
        public string AccountNumber { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Sum { get; set; }
        public bool IsBlocked { get; set; }

        [Required]
        public Currency Currency { get; set; }
        public string ClientProfileId { get; set; }
    }
}