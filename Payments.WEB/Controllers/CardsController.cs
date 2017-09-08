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
    public class CardsController : Controller
    {
        private ICardsService service;

        public CardsController(ICardsService serv)
        {
            service = serv;
        }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            try
            {
                var cardsDto = service.GetCardsByProfile(userId);
                var cards = Mapper.Map<IEnumerable<CardDto>, IEnumerable<CardViewModel>>(cardsDto);

                return View(cards);
            }
            catch (ValidationException e)
            {
                ViewBag.Message = e.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateCard()
        {
            var userId = User.Identity.GetUserId();

            var accounts = service.GetDebitAccountsByProfile(userId, true);

            if (accounts.Count() == 0)
            {
                ViewBag.Message = "Profile has no available debit accounts. Create a new account before creating card";

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
            if (ModelState.IsValid)
            {
                var accountId = User.Identity.GetUserId();

                var depositCardDto = Mapper.Map<CardViewModel, CardDto>(card);
                try
                {
                    service.CreateCard(depositCardDto, accountId);
                    TempData["Message"] = "The new card was successfully created";
                    return RedirectToAction("Index");
                }
                catch (ValidationException e)
                {
                    ViewBag["Message"] = e.Message;
                }
            }

            return View(card);
        }
    }
}