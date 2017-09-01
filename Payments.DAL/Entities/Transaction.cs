using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal TransactionSum { get; set; }
        public string Recipient { get; set; }
        public string Comment { get; set; }

        public virtual Account Account { get; set; }
    }
}
