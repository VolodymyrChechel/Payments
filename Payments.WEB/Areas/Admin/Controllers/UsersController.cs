using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Interfaces;
using Payments.WEB.Areas.Admin.Models;

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
            var list = service.GetProfiles();

            return View(list);
        }

        public ActionResult Show(string id)
        {
            var user = service.GetProfile(id);

            if (user == null)
            {
                return RedirectToAction("List");
            }
            
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
                ViewBag.Message = "Profile has no debit accounts yet";
                return RedirectToAction("List");
            }

            var items = new List<SelectListItem>();
            foreach (var account in accounts)
                items.Add(new SelectListItem {Selected = false,
                    Text = "Account id: " + account.AccountNumber + ", sum on the account: " + account.Sum + ", currency: " + account.Currency,
                    Value = account.AccountNumber});

            ViewBag.DropDownListItems = items;

            return View();
        }

        [HttpGet]
        public ActionResult EditDebitAccount(int? id)
        {
            var debitAcc = service.GetDebitAccount(id);
            var debitAccViewModel = Mapper.Map<DebitAccountDTO, DebitAccountViewModel>(debitAcc);

            return View(debitAccViewModel);
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
                return View("List");
            }

            if(service.IsAccountExist(id.Value))
            {
                ViewBag.AccountId = id;
                return View();
            }

            return View("List");
        }

        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            service.DeleteAccount(id.Value);

            return RedirectToAction("List");
        }
    }
}