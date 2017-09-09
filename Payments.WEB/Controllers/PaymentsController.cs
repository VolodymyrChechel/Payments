using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
    [Authorize(Roles = "user")]
    public class PaymentsController : Controller
    {
        private IPaymentsService service;

        public PaymentsController(IPaymentsService serv)
        {
            service = serv;
        }

        public ActionResult Index(string sort)
        {
            ViewBag.Message = TempData?["Message"];

            try
            {
                var userId = User.Identity.GetUserId();
                var listDto = service.GetPayments(userId, sort);

                var list = Mapper.Map<IEnumerable<PaymentDTO>, IEnumerable<PaymentViewModel>>(listDto);
                ViewBag.Selected = sort;

                return View(list);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = e.Message;
            }
            
            return View();
        }

        // create payment without certain account
        [HttpGet]
        public ActionResult CreatePayment()
        {
            var id = User.Identity.GetUserId();

            try
            {
                var accounts = service.GetDebitAccountsByProfile(id);

                var items = new List<SelectListItem>();
                foreach (var account in accounts)
                    items.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = "Account id: " + account.AccountNumber + ", sum on the account: " + account.Sum +
                               ", currency: " + account.Currency,
                        Value = account.AccountNumber.ToString()
                    });

                ViewBag.DropDownListItems = items;

                return View();
            }

            catch (ValidationException e)
            {
                TempData["Message"] = e.Message;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePayment(PaymentViewModel payment)
        {
            if (ModelState.IsValid)
            {
                var paymentDto = Mapper.Map<PaymentViewModel, PaymentDTO>(payment);
                try
                {
                    service.CreatePayment(paymentDto);

                    TempData["Message"] = "Payment from account " + payment.AccountAccountNumber +
                                          " was sent to processing";
                    
                }
                catch (ValidationException e)
                {
                    TempData["Message"] = e.Message;
                }

                return RedirectToAction("Index");
            }

            return View(payment);
        }
    }
}