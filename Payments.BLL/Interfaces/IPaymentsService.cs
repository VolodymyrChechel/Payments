using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    // service for payments
    // used by the user to manage with payments
    public interface IPaymentsService
    {
        PaymentDTO GetPayment(int? id);
        IEnumerable<PaymentDTO> GetPayments(string id, string sortType = null);
        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string id);
        void CreatePayment(PaymentDTO payment);
    }
}