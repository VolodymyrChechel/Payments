using System;
using Payments.Common.Enums;
using Payments.DAL.Entities;

namespace Payments.BLL.DTO
{
    public class DepositCardDTO
    {
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CVV { get; set; }

        public string Holder { get; set; }

        public CreditCardType CreditCardTypes { get; set; }

    }
}