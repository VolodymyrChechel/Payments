using System;

namespace Payments.DAL.Entities
{
    public class CreditAccount : Account
    {
        public decimal CreditSum { get; set; }
        public double CreditRate { get; set; }
        public DateTime CreditDate { get; set; }
    }
}