using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class RequestsController : Controller
    {
        private IRequestsService service;

        public UsersController(IRequestsService serv)
        {
            service = serv;
        }

        // GET: Admin/Requests
        public ActionResult Index()
        {
            return View();
        }
    }
}