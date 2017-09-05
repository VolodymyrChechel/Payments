using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.Common.Enums;
using Payments.WEB.Areas.Admin.Models;
using Payments.WEB.Util;

namespace Payments.WEB.Areas.Admin.Controllers
{
    // admin controller to manage users
    public class UsersController : Controller
    {
        private IManageService service;

        public UsersController(IManageService serv)
        {
            service = serv;
        }

        public ActionResult List()
        {
            ViewBag.Message = TempData["Message"]?.ToString();

            var list = service.GetProfiles();

            return View(list);
        }

        public ActionResult Show(string id)
        {
            TempData["UserId"] = id;

            if (id == null)
            {
                TempData["Message"] = "Id is not passed";
                return RedirectToAction("List");
            }
            var user = service.GetProfile(id);
            
            if (user == null)
            {
                TempData["Message"] = "There is no user with id " + id;
                return RedirectToAction("List");
            }

            ViewBag.Message = TempData["Message"]?.ToString();
            return View(user);
        }

        [ChildActionOnly]
        public ActionResult Accounts(string id)
        {
            if (id != null)
            {
                var accounts = service.GetDebitAccountsByProfile(id);
                var accountsViewList = Mapper.Map<IEnumerable<DebitAccountDTO>, IEnumerable<DebitAccountViewModel>>(accounts);

                return View(accountsViewList);
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateDebitAccount(string id)
        {
            if (id == null)
                return RedirectToAction("List");

            var debitAcc = new DebitAccountViewModel();
            debitAcc.ClientProfileId = id;

            return View(debitAcc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitAccount(DebitAccountViewModel debitAcc)
        {
            if (ModelState.IsValid)
            {
                var debitAccDto = Mapper.Map<DebitAccountViewModel, DebitAccountDTO>(debitAcc);
                service.CreateDebitAccount(debitAccDto);

                TempData["Message"] = "New debit account was created";
                return RedirectToAction("Show", new { id = debitAcc.ClientProfileId});
            }

            return View();
        }
        
        // create credit card
        // payments
        [HttpGet]
        public ActionResult CreateDebitCard(string id)
        {
            var accounts = service.GetDebitAccountsByProfile(id, true);

            if (accounts.Count() == 0)
            {
                TempData["Message"] = "Profile has no available debit accounts. Create a new account before creating card";

                return RedirectToAction("Show", new {id = id});
            }

            var items = new List<SelectListItem>();
            foreach (var account in accounts)
                items.Add(new SelectListItem {Selected = false,
                    Text = "Account id: " + account.AccountNumber + ", sum on the account: " + account.Sum + ", currency: " + account.Currency,
                    Value = account.AccountNumber.ToString()});

            ViewBag.DropDownListItems = items;
            
            return View();
        }

        [HttpGet]
        public ActionResult CardsList(string id)
        {
            try
            {
                var accounts = service.GetCardsByProfile(id);
                return PartialView(accounts);
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            if (TempData["UserId"] != null)
                    return RedirectToAction("Show", new { id = TempData["UserId"] });

                return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitCard(CardViewModel card)
        {
            if (ModelState.IsValid)
            {
                var depositCardDto = Mapper.Map<CardViewModel, CardDto>(card);
                service.CreateCard(depositCardDto);

                TempData["Message"] = "The new card was successfully created";

                if (TempData["UserId"] != null)
                    return RedirectToAction("Show", new { id = TempData["UserId"] });

                return RedirectToAction("List");
            }

            return View(card);
        }

        [HttpGet]
        public ActionResult EditDebitAccount(int? id)
        {
            try
            {
                var debitAcc = service.GetDebitAccount(id);
                var debitAccViewModel = Mapper.Map<DebitAccountDTO, DebitAccountViewModel>(debitAcc);

                return View(debitAccViewModel);
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }
            if (TempData["UserId"] != null)
                return RedirectToAction("Show", new { id = TempData["UserId"] });

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDebitAccount(DebitAccountViewModel debitAcc)
        {
            if (ModelState.IsValid)
            {
                var debitAccDto = Mapper.Map<DebitAccountViewModel, DebitAccountDTO>(debitAcc);
                service.UpdateDebitAccount(debitAccDto);

                return RedirectToAction("Show", new { id = debitAcc.ClientProfileId });
            }

            return View();
        }


        [HttpGet]
        public ActionResult DeleteAccount(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = "Account's number was not passed";
                return RedirectToAction("List");
            }

            if (service.IsAccountExist(id.Value))
            {
                ViewBag.AccountId = id;
                return View();
            }

            ViewBag.Message = "This account is not exist";
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult DeleteCard(string id)
        {
            if (id == null)
            {
                ViewBag.Message = "Card's name was not passed";
                return View("List");
            }

            if (service.IsCardExist(id))
            {
                ViewBag.AccountId = id;
                return View();
            }

            ViewBag.Message = "This card is not exist";
            return View("List");
        }

        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccountConfirmed(int? id)
        {
            try
            {
                service.DeleteAccount(id.Value);
                TempData["Message"] = "Account " + id + " was successfully deleted";
            }
            catch
            {
                TempData["Message"] = "Account " + id + " has binded card / operation / delete request  and shouldn't be deleted";
            }

            if (TempData["UserId"] != null)
                return RedirectToAction("Show", new { id = TempData["UserId"] });

            return RedirectToAction("List");
        }

        [HttpPost, ActionName("DeleteCard")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCardConfirmed(string id)
        {
            service.DeleteCard(id);
            TempData["Message"] = "Card " + id + " was successfully deleted";

            if (TempData["UserId"] != null)
                return RedirectToAction("Show", new {id = TempData["UserId"]});

            return RedirectToAction("List");
        }
    }
}