using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Interfaces;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
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
            var accountsListDto = service.GetAccountsByUserId(userId);
            var accountsList =
                Mapper.Map<IEnumerable<DebitAccountDTO>, IEnumerable<DebitAccountViewModel>>(accountsListDto);

            return View(accountsList);
        }

        public ActionResult UnblockAccountRequest(string id)
        {
            
            return Index();
        }
    }
}