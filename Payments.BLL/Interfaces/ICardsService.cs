using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    // service for accounts
    // allows to manage card with user role
    public interface ICardsService
    {
        void CreateCard(CardDto cardDto, string userId);
        IEnumerable<CardDto> GetCardsByProfile(string profile);
        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(
            string profileId, bool withoutCard = false,
            string sortType = null);
    }
}