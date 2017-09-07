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

        public IEnumerable<DebitAccountDTO> GetAccountsByUserId(string id)
        {
            if(id == null)
                throw new ValidationException("Id was not passed", "");

            var accounts = Database.ClientManager.Get(id)?.Accounts.ToList();

            if(accounts == null)
                throw new ValidationException("Accounts were not found", "");

            var accountsDto = Mapper.Map <IEnumerable<Account>, IEnumerable<DebitAccountDTO>>(accounts);

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

            if(account == null)
                throw new ValidationException("Account was not found", "");
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
            if (account.IsBlocked == false)
                throw new ValidationException("Account is unblocked", "");

            var lastAccount = Database.UnblockAccountRequests.
                Find(req => req.AccountAccountNumber == account.AccountNumber).
                OrderByDescending(req => req.RequestTime).
                FirstOrDefault();

            if(lastAccount?.Status == UnblockRequestStatus.Prepared)
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

            var account = Mapper.Map<DebitAccountDTO, DebitAccount>(accountDto);

            Database.Accounts.Create(account);
            Database.Save();
        }
    }
}