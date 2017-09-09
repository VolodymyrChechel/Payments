using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IPaymentsService
    {
        IEnumerable<PaymentDTO> GetPayments(string id, string sortType = null);
        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string id);
        void CreatePayment(PaymentDTO payment);
    }
}