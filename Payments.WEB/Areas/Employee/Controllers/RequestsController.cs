using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.Common.NLog;

namespace Payments.WEB.Areas.Employee.Controllers
{
    [Authorize(Roles = "employee, admin")]
    public class RequestsController : Controller
    {
        private IRequestsService service;

        public RequestsController(IRequestsService serv)
        {
            NLog.LogInfo(this.GetType(), "Constructor RequestsController execution");

            service = serv;
        }

        // show 
        public ActionResult List()
        {
            NLog.LogInfo(this.GetType(), "Method List execution");

            var requestList = service.GetRequestsList();

            ViewBag.Message = TempData?["Message"];
            return View(requestList);
        }

        public ActionResult Accept(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Accept execution");

            try
            {
                service.AcceptRequest(id);
                TempData["Message"] = "Reqeust " + id + " was accepted";
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("List");
        }

        public ActionResult Reject(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Reject execution");

            try
            {
                service.RejectRequest(id);
                TempData["Message"] = "Reqeust " + id + " was rejected";
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                TempData["Message"] = e.Message;
            }

            return RedirectToAction("List");
        }
    }
}