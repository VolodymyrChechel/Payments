using System;
using System.ComponentModel;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Payments.BLL.Interfaces;
using Payments.WEB.App_Start;

[assembly: OwinStartup(typeof(Payments.WEB.Startup))]
namespace Payments.WEB
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }

        private IUserService CreateUserService()
        {
            return (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
        }
    }
}