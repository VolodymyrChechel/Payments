using System;
using System.ComponentModel.DataAnnotations;
using Payments.Common.StaticData;

namespace Payments.WEB.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(RegExStrings.Email, ErrorMessage = "It seems not email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(RegExStrings.Password, ErrorMessage = "Minimum eight characters, at least one letter, one number and one special character")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(RegExStrings.Name, ErrorMessage = "First name has incorrect format")]
        public string FirstName { get; set; }
        
        [Required]
        [RegularExpression(RegExStrings.Name, ErrorMessage = "Second name has incorrect format")]
        public string SecondName { get; set; }
        [Required]
        [RegularExpression(RegExStrings.Name, ErrorMessage = "Patronymic has incorrect format")]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(RegExStrings.Phone, ErrorMessage = "Phone has incorrect format")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string VAT { get; set; }
    }
}