using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface ICardService
    {
        void CreateNewDepositCard(DepositCardDTO depositCardDto);
        void CreateNewCreditCard(CreditCardDTO creditCardDto);

    }
}