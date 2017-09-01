using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.WEB.Models;

namespace Payments.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO {Email = model.Email, Password = model.Password};
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Login or password is wrong");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            RegisterModel initialInput = new RegisterModel()
            {
                Birthday = DateTime.Parse("11/11/1950"),
                FirstName = "Vasil",
                SecondName = "Panteenko",
                Patronymic = "Maximovych",
                Email = "mail@mail.com",
                PhoneNumber = "380665244398",
                VAT = "1234567890"

            };
            return View(initialInput);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = Mapper.Map<RegisterModel, UserDTO>(model);

                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await UserService.Authenticate(userDto);
                    
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    
                    return View("SuccessRegister");
                }
                    
                else
                    ModelState.AddModelError(operationDetails.Prop, operationDetails.Message);
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult SuccessRegister()
        {
            return View();
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO()
            {
                Email = "admin@admin.com",
                Password = "ad46D_ewr3",
                Role = "admin"
            }, new List<string> {"admin", "employee", "user"}); 
        }
    }
}