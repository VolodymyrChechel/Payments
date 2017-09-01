using System;

namespace Payments.WEB.Models
{
    public class ShowUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string VAT { get; set; }
    }
}