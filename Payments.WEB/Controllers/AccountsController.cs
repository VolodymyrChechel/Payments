using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.Common.NLog;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
    [Authorize(Roles = "user")]
    public class AccountsController : Controller
    {
        private IAccountsService service;

        public AccountsController(IAccountsService serv)
        {
            NLog.LogInfo(this.GetType(), "Constructor execution");

            service = serv;
        }

        public ActionResult Index(string sort)
        {
            NLog.LogInfo(this.GetType(), "Method Index execution");

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
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                ViewBag.Message = e.Message;
            }

            return View();
        }

        public ActionResult UnblockAccountRequest(string id)
        {
            NLog.LogInfo(this.GetType(), "Method UnblockAccountRequest execution");

            try
            {
                service.UnblockAccountRequest(id);
                TempData["Message"] = "Request was sent for review";
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult BlockAccount(string id)
        {
            NLog.LogInfo(this.GetType(), "Method BlockAccount execution");

            try
            {
                service.BlockAccount(id);
                TempData["Message"] = "Account was blocked";
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateDebitAccount()
        {
            NLog.LogInfo(this.GetType(), "Method CreateDebitAccount GET execution");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitAccount(DebitAccountViewModel debitAcc)
        {
            NLog.LogInfo(this.GetType(), "Method CreateDebitAccount POST execution");

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
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

                    TempData["Message"] = e.Message;
                }

                return RedirectToAction("Index");
            }

            NLog.LogDebug(this.GetType(), "Model not valid");

            return View();
        }

        [HttpGet]
        public ActionResult ChangeNameAccount(string id)
        {
            NLog.LogInfo(this.GetType(), "Method ChangeNameAccount GET execution");

            try
            {
                var accountDto = service.GetAccount(id);

                var account = Mapper.Map<DebitAccountDTO, DebitAccountViewModel>(accountDto);

                return View(account);
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ChangeNameAccount(DebitAccountViewModel model)
        {
            NLog.LogInfo(this.GetType(), "Method ChangeNameAccount POST execution");

            if (ModelState["Name"].Errors.Count == 0)
            {
                var modelDto = Mapper.Map<DebitAccountViewModel, DebitAccountDTO>(model);
                try
                {
                    service.EditAccountName(modelDto);
                    TempData["Message"] = "Account's name was update";
                }
                catch (ValidationException e)
                {
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

                    TempData["Message"] = e.Message;
                }
                return RedirectToAction("Index");
            }
            
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Replenish(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Replenish GET execution");

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
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Replenish(PaymentViewModel payment)
        {
            NLog.LogInfo(this.GetType(), "Method Replenish POST execution");

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
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

                    TempData["Message"] = e.Message;
                }

                return RedirectToAction("Index");
            }

            return View(payment);
        }

        [HttpGet]
        public ActionResult Withdraw(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Withdraw GET execution");

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
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(PaymentViewModel payment)
        {
            NLog.LogInfo(this.GetType(), "Method Withdraw POST execution");

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
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

                    ModelState.AddModelError(e.Property, e.Message);
                    return View(payment);
                }
                catch (Exception e)
                {
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

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
            NLog.LogInfo(this.GetType(), "Method Payment GET execution");

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
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(PaymentViewModel payment)
        {
            NLog.LogInfo(this.GetType(), "Method Payment POST execution");

            if (ModelState.IsValid)
            {

                var paymentDto = Mapper.Map<PaymentViewModel, PaymentDTO>(payment);

                try
                {
                    service.Payment(paymentDto);
                }
                catch (ValidationException e)
                {
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

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