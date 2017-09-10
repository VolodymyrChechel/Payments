using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.BLL.Util;
using Payments.Common.Enums;
using Payments.Common.NLog;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    // implementation of IManageService
    // contains methods to manage all entities by admin
    public class ManageService : IManageService
    {
        private IUnitOfWork Database { get; set; }

        public ManageService(IUnitOfWork uow)
        {
            NLog.LogInfo(this.GetType(), "Constructor ManageService execution");

            Database = uow;
        }

        public IEnumerable<UserInfoDTO> GetProfiles()
        {
            NLog.LogInfo(this.GetType(), "Method GetProfiles execution");


            var clients = Database.ClientManager.GetAll().
                Include(profile => profile.ApplicationUser);

            return Mapper.Map<IQueryable<ClientProfile>, IEnumerable<UserInfoDTO>>(clients);
        }

        public UserInfoDTO GetProfile(string id)
        {
            NLog.LogInfo(this.GetType(), "Method GetProfile execution");

            var client = Database.ClientManager.Get(id);

            return Mapper.Map<ClientProfile, UserInfoDTO>(client);
        }

        public void BlockUser(string id)
        {
            NLog.LogInfo(this.GetType(), "Method BlockUser execution");

            var client = Database.ClientManager.Get(id);
            client.IsBlocked = true;
            Database.ClientManager.Update(client);
            Database.Save();
        }

        public void UnblockUser(string id)
        {
            NLog.LogInfo(this.GetType(), "Method UnblockUser execution");

            var client = Database.ClientManager.Get(id);
            client.IsBlocked = false;
            Database.ClientManager.Update(client);
            Database.Save();
        }

        // get debit accounts which belong to user
        // we can get account withoud card and select sorting parameter
        public IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(
            string profileId, bool withoutCard = false,
            string sortType = null)
        {
            NLog.LogInfo(this.GetType(), "Method GetDebitAccountsByProfile execution");

            var accountsList = Database.DebitAccounts.
                Find(debAcc => debAcc.ClientProfileId == profileId).
                Include(debAcc => debAcc.Cards);
            
            if (sortType != null)
                switch (sortType)
                {
                    case "NUM_DESC":
                        accountsList = accountsList.
                            OrderByDescending(acc => acc.AccountNumber);
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

            var accountsDtoList = Mapper.Map<IEnumerable<DebitAccount>,
                IEnumerable<DebitAccountDTO>>(accountsList.ToList());

            return accountsDtoList;
        }

        public void CreateDebitAccount(DebitAccountDTO accountDto)
        {
            NLog.LogInfo(this.GetType(), "Method CreateDebitAccount execution");

            var account = Mapper.Map<DebitAccountDTO, DebitAccount>(accountDto);
            var user = Database.ClientManager.Get(accountDto.ClientProfileId);
            

            account.ClientProfile = user ?? 
                throw new ValidationException("User was not found", "Client profile");
            Database.DebitAccounts.Create(account);
            user.Accounts.Add(account);
            Database.Save();
        }

        public DebitAccountDTO GetDebitAccount(int? id)
        {
            NLog.LogInfo(this.GetType(), "Method GetDebitAccount execution");

            if (id == null)
                throw new ValidationException("Accounts id was not passed", "");
            
            var account = Database.DebitAccounts.Get(id.Value);

            if(account == null)
                throw new ValidationException("Account was not found", "");

            return Mapper.Map<DebitAccount, DebitAccountDTO>(account);
        }

        public void UpdateDebitAccount(DebitAccountDTO debitAccountDto)
        {
            NLog.LogInfo(this.GetType(), "Method UpdateDebitAccount execution");

            if (debitAccountDto == null)
                throw new ValidationException("Account was not passed", "");

            var debitAccount = Mapper.Map<DebitAccountDTO, DebitAccount>(debitAccountDto);
            Database.DebitAccounts.Update(debitAccount);

            Database.Save();
        }

        public string GetAccountProfileId(int? id)
        {
            NLog.LogInfo(this.GetType(), "Method GetAccountProfileId execution");

            var profileId = Database.Accounts.Get(id).ClientProfile.Id;
            return profileId;
        }

        public bool IsAccountExist(int? accountId)
        {
            NLog.LogInfo(this.GetType(), "Method IsAccountExist execution");

            if (accountId == null)
                throw new ValidationException("Accounts id was not passed", "");

            var account = Database.Accounts.Get(accountId.Value);
            
            return account != null;
        }

        public void DeleteAccount(int? accountId)
        {
            NLog.LogInfo(this.GetType(), "Method DeleteAccount execution");

            if (accountId == null)
                throw new ValidationException("Accounts id was not passed", "");
            
            Database.Accounts.Delete(accountId.Value);
            Database.Save();
        }

        public void CreateCard(CardDto card)
        {
            NLog.LogInfo(this.GetType(), "Method CreateCard execution");

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

        public bool IsCardExist(string number)
        {
            NLog.LogInfo(this.GetType(), "Method IsCardExist execution");

            var card = Database.Cards.Get(number);

            return card != null;
        }

        public IEnumerable<CardDto> GetCardsByProfile(string id)
        {
            NLog.LogInfo(this.GetType(), "Method GetCardsByProfile execution");

            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var accounts = Database.Accounts.
                Find(acc => acc.ClientProfileId == id).
                Include(acc => acc.Cards).SelectMany(acc => acc.Cards).
                ToList();

            var accountsDto = Mapper.Map<IEnumerable<Card>, IEnumerable<CardDto>>(accounts);

            return accountsDto;
        }

        public void DeleteCard(string number)
        {
            NLog.LogInfo(this.GetType(), "Method DeleteCard execution");

            if (number == null)
                throw new ValidationException("Number was not passed", "");

            Database.Cards.Delete(number);
            Database.Save();
        }

        public void Replenish(PaymentDTO paymentDto)
        {
            NLog.LogInfo(this.GetType(), "Method Replenish execution");

            if (paymentDto == null)
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
            NLog.LogInfo(this.GetType(), "Method Withdraw execution");

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

        // create payment
        public void Payment(PaymentDTO paymentDto)
        {
            NLog.LogInfo(this.GetType(), "Method Payment execution");

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
            NLog.LogInfo(this.GetType(), "Method GetPaymentsByProfile execution");

            if (id == null)
                throw new ValidationException("Cannot find user", "");

            var paymentsList = Database.Accounts.
                Find(account => account.ClientProfileId == id).
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
                        paymentsList = paymentsList.
                            OrderByDescending(p => p.PaymentDate);
                        break;
                    case "DATE_ASC":
                        paymentsList = paymentsList.OrderBy(p => p.PaymentDate);
                        break;
                }

            var paymentsDtoList = Mapper.Map<List<Payment>, List<PaymentDTO>>(paymentsList.ToList());
            return paymentsDtoList;
        }

        // method to confirm payment
        public void ConfirmPayment(string id)
        {
            NLog.LogInfo(this.GetType(), "Method ConfirmPayment execution");

            if (id == null)
                throw new ValidationException("Id was not set, cannot find paymnet", "Id");

            var payment = Database.Payments.Get(id);

            if(payment == null)
                throw new ValidationException("A payment with id" + id + " was not fount", "");

            var account = Database.Accounts.Get(payment.AccountAccountNumber);

            if (account == null)
                throw new ValidationException("An account with id" +
                    payment.AccountAccountNumber + " was not fount", "");

            if (account.IsBlocked)
            {
                payment.PaymentStatus = PaymentStatus.Rejected;
                payment.Comment += " | Rejected: account is blocked | ";
                Database.Save();
                throw new ValidationException("Account blocked", "");
            }

            // check sum after operation
            var finiteSum = account.Sum - payment.PaymentSum;
            if (finiteSum < 0)
            {
                payment.PaymentStatus = PaymentStatus.Rejected;
                payment.Comment += "| Rejected: no money on an account | ";
                Database.Save();
                throw new ValidationException("Account has no enough money", "");
            }

            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentStatus = PaymentStatus.Sent;
            payment.Comment += " | Payment was sent |";
            account.Sum = finiteSum;

            Database.Save();
        }
    }
}