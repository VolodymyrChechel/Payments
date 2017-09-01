using System.ComponentModel.DataAnnotations;
using System.Web.Services.Protocols;
using Payments.Common.StaticData;

namespace Payments.WEB.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(RegExStrings.Email, ErrorMessage = "It seems not email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}