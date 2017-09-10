using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payments.Common.NLog;

namespace Payments.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NLog.LogInfo(this.GetType(), "Method Index execution");

            return View();
        }

        public ActionResult NotFound()
        {
            NLog.LogInfo(this.GetType(), "Method NotFound execution");
            
            return View();
        }
    }
}