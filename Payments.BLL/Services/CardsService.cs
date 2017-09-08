using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.BLL.Util;
using Payments.Common.Enums;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class CardsService : ICardsService
    {
        private IUnitOfWork Database { get; set; }

        public CardsService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void CreateCard(CardDto card, string userId)
        {
            var account = Database.Accounts.Get(card.AccountAccountNumber);

            if(account?.ClientProfileId != userId)
                throw new ValidationException("Account deos not belong user", "");

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            Random random = new Random();

            // generate holder name
            // if it was not passed generate name based on account name
            if (card.Holder == null)
            {
                var cardHolder = Database.Accounts.Get(card.AccountAccountNumber.Value).ClientProfile;

                card.Holder = cardHolder.FirstName + " " + cardHolder.SecondName;
            }

            // generate credit card number and CVV code 
            card.CVV = random.Next(1, 1000).ToString("D3");

            if (card.CreditCardTypes == CreditCardType.AmericanExpress)
            {
                card.CardNumber = CreditCardNumberGenerator.GenerateAmericanExpressNumber();
                card.CVV += random.Next(0, 10);
            }
            else if (card.CreditCardTypes == CreditCardType.MasterCard)
                card.CardNumber = CreditCardNumberGenerator.GenerateMasterCardNumber();
            else if (card.CreditCardTypes == CreditCardType.Visa)
                card.CardNumber = CreditCardNumberGenerator.GenerateVisaNumber();

            // generate expiry date
            card.ExpiryDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddYears(2);

            var cardEntity = Mapper.Map<CardDto, Card>(card);

            Database.Cards.Create(cardEntity);
            Database.Save();
        }
        
        public IEnumerable<CardDto> GetCardsByProfile(string profileId)
        {
            if (profileId == null)
                throw new ValidationException("Id was not passed", "");

            var userAccounts = Database.ClientManager.Get(profileId).Accounts;
            var cards = userAccounts.SelectMany(user => user.Cards).ToList();

            var cardsDto = Mapper.Map<IEnumerable<Card>, IEnumerable<CardDto>>(cards);
            return cardsDto;
        }

        public IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string profileId, bool withoutCard = false, string sortType = null)
        {
            var accountsList = Database.DebitAccounts.Find(debAcc => debAcc.ClientProfileId == profileId).Include(debAcc => debAcc.Cards);

            if (sortType != null)
                switch (sortType)
                {
                    case "NUM_DESC":
                        accountsList = accountsList.OrderByDescending(acc => acc.AccountNumber);
                        break;
                    case "NUM_ASC":
                        accountsList = accountsList.OrderBy(acc => acc.AccountNumber);
                        break;
                    case "NAME_DESC":
                        accountsList = accountsList.OrderByDescending(acc => acc.Name);
                        break;
                    case "NAME_ASC":
                        accountsList = accountsList.OrderBy(acc => acc.Name);
                        break;
                    case "SUM_DESC":
                        accountsList = accountsList.OrderByDescending(acc => acc.Sum);
                        break;
                    case "SUM_ASC":
                        accountsList = accountsList.OrderBy(acc => acc.Sum);
                        break;
                }

            if (withoutCard)
                accountsList = accountsList.Where(acc => acc.Cards.Count == 0);

            var accountsDtoList = Mapper.Map<IEnumerable<DebitAccount>, IEnumerable<DebitAccountDTO>>(accountsList.ToList());

            return accountsDtoList;
        }
    }
}