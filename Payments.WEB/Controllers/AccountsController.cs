using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
    [Authorize(Roles = "user")]
    public class AccountsController : Controller
    {
        private IAccountsService service;

        public AccountsController(IAccountsService serv)
        {
            service = serv;
        }

        public ActionResult Index(string sort)
        {
            var userId = User.Identity.GetUserId();

            try
            {
                var accountsListDto = service.GetAccountsByUserId(userId, sort);
                var accountsList =
                    Mapper.Map<IEnumerable<DebitAccountDTO>, IEnumerable<DebitAccountViewModel>>(accountsListDto);

                ViewBag.Message = TempData["Message"]?.ToString();
                ViewBag.Selected = sort;
                
                return View(accountsList);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = e.Message;
            }

            return View();
        }

        public ActionResult UnblockAccountRequest(string id)
        {
            try
            {
                service.UnblockAccountRequest(id);
                TempData["Message"] = "Request was sent for review";
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult BlockAccount(string id)
        {
            try
            {
                service.BlockAccount(id);
                TempData["Message"] = "Account was blocked";
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateDebitAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitAccount(DebitAccountViewModel debitAcc)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var debitAccDto = Mapper.Map<DebitAccountViewModel, DebitAccountDTO>(debitAcc);
                debitAccDto.ClientProfileId = userId;

                try
                {
                    service.CreateDebitAccount(debitAccDto);

                    TempData["Message"] = "New debit account was created";
                }
                catch (ValidationException e)
                {
                    TempData["Message"] = e.Message;
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ChangeNameAccount(string id)
        {
            try
            {
                var accountDto = service.GetAccount(id);

                var account = Mapper.Map<DebitAccountDTO, DebitAccountViewModel>(accountDto);

                return View(account);
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ChangeNameAccount(DebitAccountViewModel model)
        {
            if(ModelState["Name"].Errors.Count == 0)
            {
                var modelDto = Mapper.Map<DebitAccountViewModel, DebitAccountDTO>(model);
                try
                {
                    service.EditAccountName(modelDto);
                    TempData["Message"] = "Account's name was update";
                }
                catch (ValidationException e)
                {
                    TempData["Message"] = e.Message;
                }
                return RedirectToAction("Index");
            }
            
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Replenish(string id)
        {
            try
            {
                var account = service.GetAccount(id);

                if(account.ClientProfileId != User.Identity.GetUserId())
                    throw new ValidationException("You cannot replenish this account", "");

                var payment = new PaymentViewModel
                {
                    Recipient = account.AccountNumber.ToString()
                };

                return View(payment);
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Replenish(PaymentViewModel payment)
        {
            if (ModelState.IsValid)
            {
                var paymentDto = Mapper.Map<PaymentViewModel, PaymentDTO>(payment);
                try
                {
                    service.Replenish(paymentDto);
                    TempData["Message"] = "Account " + payment.Recipient + " was successfully replenished";
                }
                catch (ValidationException e)
                {
                    TempData["Message"] = e.Message;
                }

                return RedirectToAction("Index");
            }

            return View(payment);
        }

        [HttpGet]
        public ActionResult Withdraw(string id)
        {
            try
            {
                var account = service.GetAccount(id);

                if (account.ClientProfileId != User.Identity.GetUserId())
                    throw new ValidationException("You cannot withdaw from this account", "");

                var payment = new PaymentViewModel
                {
                    Recipient = account.AccountNumber.ToString()
                };

                return View(payment);
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(PaymentViewModel payment)
        {
            if (ModelState.IsValid)
            {
                var paymentDto = Mapper.Map<PaymentViewModel, PaymentDTO>(payment);

                try
                {
                    service.Withdraw(paymentDto);

                    TempData["Message"] = "Withdrawal from an account " + payment.Recipient + " was successfully";
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                    return View(payment);
                }
                catch (Exception e)
                {
                    TempData["Message"] = e.Message;
                    
                }

                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // create payment with certain account
        [HttpGet]
        public ActionResult Payment(string id)
        {
            try
            {
                var account = service.GetAccount(id);

                if (account.ClientProfileId != User.Identity.GetUserId())
                {
                    TempData["Message"] = "You cannot access to account \"" + id + "\"";

                    return RedirectToAction("Index");
                }

                var payment = new PaymentViewModel
                {
                    AccountAccountNumber = account.AccountNumber
                };

                return View(payment);
                }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(PaymentViewModel payment)
        {
            if (ModelState.IsValid)
            {

                var paymentDto = Mapper.Map<PaymentViewModel, PaymentDTO>(payment);

                try
                {
                    service.Payment(paymentDto);
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                    return View(payment);
                }

                TempData["Message"] = "Payment with account " + payment.AccountAccountNumber + " was sent to processing";

                return RedirectToAction("Index");
            }

            return View(payment);
        }
    }
}