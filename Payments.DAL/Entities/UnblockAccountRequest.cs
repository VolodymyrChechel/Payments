using System;

namespace Payments.DAL.Entities
{
    public class UnblockAccountRequest
    {
        public int Id { get; set; }
        
        // 0 - prepared, 1 - proceeded, 2 - cancelled
        public int Status { get; set; }
        public DateTime RequestTime { get; set; }
        
        public virtual Account Account { get; set; }
    }
}