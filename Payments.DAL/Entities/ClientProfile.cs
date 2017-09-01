using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payments.DAL.Entities
{
    // use to separate user for authentification system and user of payment system
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public bool IsBlocked { get; set; }

        [Required]
        public string  FirstName { get; set; }

        [Required]
        public string  SecondName { get; set; }

        [Required]
        public string  Patronymic { get; set; }

        public string  Country { get; set; }

        public string  City { get; set; }

        public string  Address { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string VAT { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
       
    }
}