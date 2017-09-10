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
using Payments.Common.NLog;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
    [Authorize(Roles = "user")]
    public class CardsController : Controller
    {
        private ICardsService service;

        public CardsController(ICardsService serv)
        {
            NLog.LogDebug(this.GetType(), "CardsController constructor");

            service = serv;
        }

        public ActionResult Index()
        {
            NLog.LogInfo(this.GetType(), "Method Index execution");

            string userId = User.Identity.GetUserId();
            try
            {
                var cardsDto = service.GetCardsByProfile(userId);
                var cards = Mapper.Map<IEnumerable<CardDto>, IEnumerable<CardViewModel>>(cardsDto);

                ViewBag.Message = TempData?["Message"];
                return View(cards);
            }
            catch (ValidationException e)
            {
                NLog.LogError(this.GetType(), "Exception: " + e.Message);

                ViewBag.Message = e.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateCard()
        {
            NLog.LogInfo(this.GetType(), "Method CreateCard execution");

            var userId = User.Identity.GetUserId();
            

            var accounts = service.GetDebitAccountsByProfile(userId, true);

            if (!accounts.Any())
            {
                TempData["Message"] = "Profile has no available debit accounts. Create a new account before creating card";

                return RedirectToAction("Index");
            }

            var items = new List<SelectListItem>();
            foreach (var account in accounts)
                items.Add(new SelectListItem
                {
                    Selected = false,
                    Text = "Account id: " + account.AccountNumber + ", sum on the account: " + account.Sum + ", currency: " + account.Currency,
                    Value = account.AccountNumber.ToString()
                });

            ViewBag.DropDownListItems = items;

            return View();
        }

        // create new debit card
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCard(CardViewModel card)
        {
            NLog.LogDebug(this.GetType(), "CreateCard method execution");

            if (ModelState.IsValid)
            {
                var accountId = User.Identity.GetUserId();

                var depositCardDto = Mapper.Map<CardViewModel, CardDto>(card);
                try
                {
                    service.CreateCard(depositCardDto, accountId);
                    TempData["Message"] = "The new card was successfully created";
                }
                catch (ValidationException e)
                {
                    NLog.LogError(this.GetType(), "Exception: " + e.Message);

                    TempData["Message"] = e.Message;
                }
                return RedirectToAction("Index");
            }

            return View(card);
        }
    }
}