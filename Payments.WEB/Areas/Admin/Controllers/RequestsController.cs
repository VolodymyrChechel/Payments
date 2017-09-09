using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;

namespace Payments.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class RequestsController : Controller
    {
        private IRequestsService service;

        public RequestsController(IRequestsService serv)
        {
            service = serv;
        }

        // show 
        public ActionResult List()
        {
            var requestList = service.GetRequestsList();

            ViewBag.Message = TempData?["Message"];
            return View(requestList);
        }

        public ActionResult Accept(string id)
        {
            try
            {
                service.AcceptRequest(id);
                TempData["Message"] = "Reqeust " + id + " was accepted";
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("List");
        }

        public ActionResult Reject(string id)
        {
            try
            {
                service.RejectRequest(id);
                TempData["Message"] = "Reqeust " + id + " was rejected";
            }
            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;
            }

            return RedirectToAction("List");
        }
    }
}