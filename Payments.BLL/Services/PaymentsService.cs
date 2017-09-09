using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    // implementation of IPaymentsService
    // allows to manage payments by user
    public class PaymentsService : IPaymentsService
    {
        private IUnitOfWork Database { get; set; }

        public PaymentsService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public PaymentDTO GetPayment(int? id)
        {
            if (id == null)
                throw new ValidationException("Payment id was not passed", "");

            var payment = Database.Payments.Get(id);

            if (payment == null)
                throw new ValidationException("Payment was not found", "");

            var paymentDto = Mapper.Map<Payment, PaymentDTO>(payment);

            return paymentDto;
        }
        
        public IEnumerable<PaymentDTO> GetPayments(string id, string sortType)
        {
            if (id == null)
                throw new ValidationException("Cannot find user", "");

            var paymentsList = Database.Accounts.
                Find(account => account.ClientProfileId == id).
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

        public IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string profileId)
        {
            if(profileId == null)
                throw new ValidationException("Profile id is not found", "");

            var profileStatus = Database.ClientManager.Get(profileId).IsBlocked;

            if (profileStatus)
                throw new ValidationException("Your profile is blocked. All operations are forbidden", "");

            var accountsList = Database.DebitAccounts.
                Find(acc => acc.ClientProfileId == profileId).
                Where(acc => !acc.IsBlocked);

            if (!accountsList.Any())
                throw new ValidationException("There is no available accounts", "");
            
            var accountsDtoList = Mapper.Map<IEnumerable<DebitAccount>,
                IEnumerable<DebitAccountDTO>>(accountsList.ToList());

            return accountsDtoList;
        }

        public void CreatePayment(PaymentDTO paymentDto)
        {
            if (paymentDto == null)
                throw new ValidationException("Payment object is not passed", "");

            var payment = Mapper.Map<PaymentDTO, Payment>(paymentDto);
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentType = PaymentType.Payment;

            var account = Database.Accounts.Get(payment.AccountAccountNumber);
            if (account == null)
                throw new ValidationException("Cannot find the account", "");


            // check finite sum
            var finiteSum = account.Sum - payment.PaymentSum;

            if (finiteSum < 0)
                throw new ValidationException("Sum of payment cannot be much than " + account.Sum, "Sum");

            payment.PaymentStatus = PaymentStatus.Prepared;
            payment.Account = account;

            Database.Payments.Create(payment);
            Database.Save();
        }
    }
}