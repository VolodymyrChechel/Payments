using System;
using Payments.Common.Enums;
using Payments.DAL.Entities;

namespace Payments.BLL.DTO
{
    // DTO class for card
    public class CardDto
    {
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CVV { get; set; }

        public string Holder { get; set; }

        public CreditCardType CreditCardTypes { get; set; }

        public int? AccountAccountNumber { get; set; }
    }
}