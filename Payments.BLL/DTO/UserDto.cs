using System;

namespace Payments.BLL.DTO
{
    // represent information that concerns to user
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string VAT { get; set; }

        public string Role { get; set; }
    }
}