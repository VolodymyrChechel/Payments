using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Common.Enums;
using Payments.DAL.Entities;

namespace Payments.BLL.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }

        public decimal PaymentSum { get; set; }

        public PaymentType PaymentType { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Recipient { get; set; }

        public string Comment { get; set; }
    }
}
