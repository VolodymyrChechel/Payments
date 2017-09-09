using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.Common.Enums;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class AccountsService : IAccountsService
    {
        private IUnitOfWork Database { get; set; }

        public AccountsService(IUnitOfWork uow)
        {
            Database = uow;
        }

        // return list of account which belongs to a con
        public IEnumerable<DebitAccountDTO> GetAccountsByUserId(string id, string ordering = null)
        {
            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var accounts = Database.ClientManager.Get(id)?.Accounts;
            
            if (accounts == null)
                throw new ValidationException("Accounts were not found", "");

            if (ordering != null)
                switch (ordering)
                {
                    case "NUM_DESC":
                        accounts = accounts.OrderByDescending(acc => acc.AccountNumber).ToList();
                        break;
                    case "NUM_ASC":
                        accounts = accounts.OrderBy(acc => acc.AccountNumber).ToList();
                        break;
                    case "NAME_DESC":
                        accounts = accounts.OrderByDescending(acc => acc.Name).ToList();
                        break;
                    case "NAME_ASC":
                        accounts = accounts.OrderBy(acc => acc.Name).ToList();
                        break;
                    case "SUM_DESC":
                        accounts = accounts.OrderByDescending(acc => acc.Sum).ToList();
                        break;
                    case "SUM_ASC":
                        accounts = accounts.OrderBy(acc => acc.Sum).ToList();
                        break;
                }

            var accountsDto = Mapper.Map<IEnumerable<Account>, IEnumerable<DebitAccountDTO>>(accounts);

            return accountsDto;

        }

        public DebitAccountDTO GetAccount(string id)
        {
            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var account = Database.Accounts.Get(id);

            if (account == null)
                throw new ValidationException("Account was not found", "");

            var accountDto = Mapper.Map<Account, DebitAccountDTO>(account);

            return accountDto;
        }

        public void BlockAccount(string id)
        {
            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var account = Database.Accounts.Get(id);

            if (account == null)
                throw new ValidationException("Account was not found", "");
            
            if(account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden" , "");

            if (account.IsBlocked == true)
                throw new ValidationException("Account is blocked already", "");

            account.IsBlocked = true;
            Database.Accounts.Update(account);
            Database.Save();
        }

        public void UnblockAccountRequest(string id)
        {
            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var account = Database.Accounts.Get(id);

            if (account == null)
                throw new ValidationException("Account was not found", "");

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            if (account.IsBlocked == false)
                throw new ValidationException("Account is unblocked already", "");

            var lastAccount = Database.UnblockAccountRequests
                .Find(req => req.AccountAccountNumber == account.AccountNumber)
                .OrderByDescending(req => req.RequestTime).FirstOrDefault();

            if (lastAccount?.Status == UnblockRequestStatus.Prepared)
                throw new ValidationException("Last request was not considered", "");

            var unblockRequest = new UnblockAccountRequest
            {
                Account = account,
                RequestTime = DateTime.UtcNow,
                Status = UnblockRequestStatus.Prepared
            };

            Database.UnblockAccountRequests.Create(unblockRequest);
            Database.Save();
        }

        public void CreateDebitAccount(DebitAccountDTO accountDto)
        {
            if (accountDto == null)
                throw new ValidationException("Account was not passed", "");

            if (Database.ClientManager.Get(accountDto.ClientProfileId).IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            var account = Mapper.Map<DebitAccountDTO, DebitAccount>(accountDto);

            Database.Accounts.Create(account);
            Database.Save();
        }

        public void EditAccountName(DebitAccountDTO accountDto)
        {
            var account = Database.Accounts.Get(accountDto.AccountNumber);

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            account.Name = accountDto.Name;

            Database.Accounts.Update(account);
            Database.Save();
        }

        public void Replenish(PaymentDTO paymentDto)
        {
            if (paymentDto == null)
                throw new ValidationException("Payment object is not passed", "");

            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Replenish;

            var account = Database.Accounts.Get(payment.Recipient);

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");
            if (account.IsBlocked)
                throw new ValidationException("The account is blocked. Replenish is canceled", "");

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
                throw new NullReferenceException("Payment object is not passed");

            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Withdraw;

            var account = Database.Accounts.Get(payment.Recipient);

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");
            if (account.IsBlocked)
                throw new AccessException("The account is blocked. Withdrawal is canceled", "");

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

            if (account.ClientProfile.IsBlocked)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            var finiteSum = account.Sum - payment.PaymentSum;

            if (finiteSum < 0)
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

            if (sortType != null)
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