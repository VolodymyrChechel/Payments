using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RequestsController : Controller
    {
        // GET: Admin/Requests
        public ActionResult Index()
        {
            return View();
        }
    }
}