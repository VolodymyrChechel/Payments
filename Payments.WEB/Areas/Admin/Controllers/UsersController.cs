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
    public class UsersController : Controller
    {
        private IManageService service;

        public UsersController(IManageService serv)
        {
            service = serv;
        }

        public ActionResult Index()
        {
            return View();
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

        public ActionResult Accounts(string id)
        {
            var accounts = service.GetDebitAccountsByProfile(id);
            var accountsViewList = Mapper.Map<IEnumerable<DebitAccountDTO>, IEnumerable<DebitAccountViewModel>>(accounts);

            return View(accountsViewList);
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
    }
}