using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Payments.BLL.Interfaces;

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
            var user = service.GetProfile(id);

            if (user == null)
            {
                return RedirectToAction("List");
            }

            return View(user);
        }
    }
}