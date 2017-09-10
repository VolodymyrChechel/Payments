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
using Payments.Common.NLog;
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
            NLog.LogInfo(this.GetType(), "Method Login execution");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            NLog.LogInfo(this.GetType(), "Method Login POST execution");

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
            NLog.LogInfo(this.GetType(), "Method Register execution");

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
            NLog.LogInfo(this.GetType(), "Method Register POST execution");

            await SetInitialDataAsync();
            
            if (ModelState.IsValid)
            {
                UserDTO userDto = Mapper.Map<RegisterModel, UserDTO>(model);
                userDto.Role = "user";

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
            NLog.LogInfo(this.GetType(), "Method LogOff POST execution");

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult SuccessRegister()
        {
            NLog.LogInfo(this.GetType(), "Method SuccessRegister execution");

            return View();
        }

        private async Task SetInitialDataAsync()
        {
            NLog.LogInfo(this.GetType(), "Method SetInitialDataAsync execution");

            var userList = new List<UserDTO>
            {
                new UserDTO
                {
                    Email = "billmontana@gmail.com",
                    FirstName = "Bill",
                    SecondName = "Montana",
                    Patronymic = "Bob",
                    Birthday = new DateTime(1979, 11, 5),
                    Role = "user",
                    PhoneNumber = "380995572156",
                    Password = "ad46D_ewr3",
                    VAT = "5479183527"
                },
                new UserDTO
                {
                    Email = "nicktesla@gmail.com",
                    FirstName = "Nick",
                    SecondName = "Tesla",
                    Patronymic = "Elon",
                    Birthday = new DateTime(1959, 7, 25),
                    Role = "user",
                    PhoneNumber = "380997519634",
                    Password = "ad46D_ewr3",
                    VAT = "4103587036"
                },
                new UserDTO
                {
                    Email = "marinatrump@gmail.com",
                    FirstName = "Marina",
                    SecondName = "Trump",
                    Patronymic = "Donald",
                    Birthday = new DateTime(1995, 11, 19),
                    Role = "user",
                    PhoneNumber = "380687519633",
                    Password = "ad46D_ewr3",
                    VAT = "6300076592"
                }
            };

            await UserService.SetInitialData(new UserDTO()
            {
                Email = "admin@admin.com",
                Password = "ad46D_ewr3",
                Role = "admin"
            },
            new UserDTO
            {
                Email = "employee@employee.com",
                Password = "ad46D_ewr3",
                Role = "employee"
            },
            userList, new List<string> {"admin", "employee", "user"}); 
        }
    }
}