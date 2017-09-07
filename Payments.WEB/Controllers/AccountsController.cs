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

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            try
            {
                var accountsListDto = service.GetAccountsByUserId(userId);
                var accountsList =
                    Mapper.Map<IEnumerable<DebitAccountDTO>, IEnumerable<DebitAccountViewModel>>(accountsListDto);

                ViewBag.Message = TempData["Message"]?.ToString();
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

                service.CreateDebitAccount(debitAccDto);

                TempData["Message"] = "New debit account was created";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ChangeNameAccount(string id)
        {
            
            return View();
        }
        
    }
}