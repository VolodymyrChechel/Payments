using System;
using System.ComponentModel.DataAnnotations;
using Payments.Common.Enums;

namespace Payments.DAL.Entities
{
    // entity for a card
    public class Card
    {
        [Key]
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CVV { get; set; }

        public string Holder { get; set; }

        public CreditCardType CreditCardTypes { get; set; }

        public int? AccountAccountNumber { get; set; }
        public virtual Account Account { get; set; }

    }
}