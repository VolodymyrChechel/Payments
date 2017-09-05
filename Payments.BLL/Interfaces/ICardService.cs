using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface ICardService
    {
        void CreateNewDepositCard(CardDto cardDto);
        void CreateNewCreditCard(CreditCardDTO creditCardDto);

    }
}