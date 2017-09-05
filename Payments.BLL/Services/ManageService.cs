using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.BLL.Util;
using Payments.Common.Enums;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class ManageService : IManageService
    {
        private IUnitOfWork Database { get; set; }

        public ManageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<UserInfoDTO> GetProfiles()
        {
            var clients = Database.ClientManager.GetAll().Include(profile => profile.ApplicationUser);

            return Mapper.Map<IQueryable<ClientProfile>, IEnumerable<UserInfoDTO>>(clients); ;
        }

        public UserInfoDTO GetProfile(string id)
        {
            var client = Database.ClientManager.Get(id);

            return Mapper.Map<ClientProfile, UserInfoDTO>(client);
        }

        public IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string profileId, bool withoutCard)
        {
            var accountsList = Database.DebitAccounts.Find(debAcc => debAcc.ClientProfileId == profileId).Include(debAcc => debAcc.Cards).ToList();

            if (withoutCard)
                accountsList = accountsList.Where(debAcc => debAcc.Cards.Count == 0).ToList();
            
            var accountsDtoList = Mapper.Map<IEnumerable<DebitAccount>, IEnumerable<DebitAccountDTO>>(accountsList);

            return accountsDtoList;
        }

        public void CreateDebitAccount(DebitAccountDTO accountDto)
        {
            var account = Mapper.Map<DebitAccountDTO, DebitAccount>(accountDto);
            var user = Database.ClientManager.Get(accountDto.ClientProfileId);
            account.ClientProfile = user;
            Database.DebitAccounts.Create(account);
            user.Accounts.Add(account);
            Database.Save();
        }

        public DebitAccountDTO GetDebitAccount(int? id)
        {
            if(id == null)
                throw new ValidationException("Accounts id was not passed", "");
            
            var account = Database.DebitAccounts.Get(id.Value);

            if(account == null)
                throw new ValidationException("Account was not found", "");

            return Mapper.Map<DebitAccount, DebitAccountDTO>(account);
        }

        public void UpdateDebitAccount(DebitAccountDTO debitAccountDto)
        {
            if (debitAccountDto == null)
                throw new ValidationException("Account was not passed", "");

            var debitAccount = Mapper.Map<DebitAccountDTO, DebitAccount>(debitAccountDto);
            Database.DebitAccounts.Update(debitAccount);

            Database.Save();
        }


        public bool IsAccountExist(int? accountId)
        {
            if(accountId == null)
                throw new ValidationException("Accounts id was not passed", "");

            var account = Database.Accounts.Get(accountId.Value);
            
            return account != null;
        }

        public void DeleteAccount(int? accountId)
        {
            if (accountId == null)
                throw new ValidationException("Accounts id was not passed", "");

            Database.Accounts.Delete(accountId.Value);
            Database.Save();
        }

        public void CreateCard(CardDto card)
        {
            Random random = new Random();

            //danger
            var cardHolder = Database.Accounts.Get(card.AccountAccountNumber.Value).ClientProfile;
            card.Holder = cardHolder.FirstName + " " + cardHolder.SecondName;
            //end danger

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

            card.ExpiryDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddYears(2);

            var cardEntity = Mapper.Map<CardDto, Card>(card);

            Database.Cards.Create(cardEntity);
            Database.Save();

        }

        public IEnumerable<CardDto> GetCardsByProfile(string id)
        {
            if(id == null)
                throw new ValidationException("Id was not passed", "");

            var accounts = Database.Accounts.Find(acc => acc.ClientProfile.Id == id).Include(acc => acc.Cards).SelectMany(acc => acc.Cards).ToList();

            var accountsDto = Mapper.Map<IEnumerable<Card>, IEnumerable<CardDto>>(accounts);

            return accountsDto;
        }

        void DeleteCard(string number)
        {
            if (number == null)
                throw new ValidationException("Number was not passed", "");

            Database.Cards.Delete();
        }

    }
}