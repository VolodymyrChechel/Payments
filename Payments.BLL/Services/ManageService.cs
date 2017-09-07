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

        public void CreateDebitAccount(DebitAccountDTO accountDto)
        {
            var account = Mapper.Map<DebitAccountDTO, DebitAccount>(accountDto);
            var user = Database.ClientManager.Get(accountDto.ClientProfileId);
            
            account.ClientProfile = user ?? throw new ValidationException("User was not found", "Client profile");
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

        public string GetAccountProfileId(int? id)
        {
            var profileId = Database.Accounts.Get(id).ClientProfile.Id;
            return profileId;
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

        public bool IsCardExist(string number)
        {
            var card = Database.Cards.Get(number);

            return card != null;
        }

        public IEnumerable<CardDto> GetCardsByProfile(string id)
        {
            if(id == null)
                throw new ValidationException("Id was not passed", "");

            var accounts = Database.Accounts.Find(acc => acc.ClientProfile.Id == id as string).Include(acc => acc.Cards).SelectMany(acc => acc.Cards).ToList();

            var accountsDto = Mapper.Map<IEnumerable<Card>, IEnumerable<CardDto>>(accounts);

            return accountsDto;
        }

        public void DeleteCard(string number)
        {
            if (number == null)
                throw new ValidationException("Number was not passed", "");

            Database.Cards.Delete(number);
            Database.Save();
        }

        public void Replenish(PaymentDTO paymentDto)
        {
            if(paymentDto == null)
                throw new ValidationException("Payment object is not passed", "");

            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Replenish;

            var account = Database.Accounts.Get(payment.Recipient);
            account.Sum += payment.PaymentSum;
            Database.Accounts.Update(account);

            payment.PaymentStatus = PaymentStatus.Sent;
            payment.Account = account;

            Database.Payments.Create(payment);
            Database.Save();
        }

        public void Withdraw(PaymentDTO paymentDto)
        {
            if (paymentDto == null)
                throw new ValidationException("Payment object is not passed", "");

            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Withdraw;

            var account = Database.Accounts.Get(payment.Recipient);

            var finiteSum = account.Sum - payment.PaymentSum;

            if (finiteSum < 0)
                throw new ValidationException("Sum of withdrawal cannot be much than " + account.Sum, "Sum");

            account.Sum = finiteSum;
            Database.Accounts.Update(account);

            payment.PaymentStatus = PaymentStatus.Sent;
            payment.Account = account;

            Database.Payments.Create(payment);
            Database.Save();
        }

        public void Payment(PaymentDTO paymentDto)
        {
            if (paymentDto == null)
                throw new ValidationException("Payment object is not passed", "");
            
            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Payment;

            var account = Database.Accounts.Get(payment.AccountAccountNumber);
            if (account == null)
                throw new ValidationException("Cannot find the account", "");

            var finiteSum = account.Sum - payment.PaymentSum;

            if (finiteSum< 0)
                throw new ValidationException("Sum of payment cannot be much than " + account.Sum, "Sum");

            payment.PaymentStatus = PaymentStatus.Prepared;
            payment.Account = account;

            Database.Payments.Create(payment);
            Database.Save();
        }

        public IEnumerable<PaymentDTO> GetPaymentsByProfile(string id, string sortType)
        {
            if (id == null)
                throw new ValidationException("Cannot find user", "");

            var paymentsList = Database.Accounts.
                Find(account => account.ClientProfile.Id == id).
                SelectMany(acc => acc.Payments);

            if(sortType != null)
                switch (sortType)
                {
                    case "NUM_DESC":
                        paymentsList = paymentsList.OrderByDescending(p => p.Id);
                        break;
                    case "NUM_ASC":
                        paymentsList = paymentsList.OrderBy(p => p.Id);
                        break;
                    case "DATE_DESC":
                        paymentsList = paymentsList.OrderByDescending(p => p.PaymentDate);
                        break;
                    case "DATE_ASC":
                        paymentsList = paymentsList.OrderBy(p => p.PaymentDate);
                        break;
                }

            var paymentsDtoList = Mapper.Map<List<Payment>, List<PaymentDTO>>(paymentsList.ToList());
            return paymentsDtoList;
        }
    }
}