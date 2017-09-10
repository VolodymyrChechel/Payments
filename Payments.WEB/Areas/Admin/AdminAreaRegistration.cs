using System.Web.Mvc;
using Payments.Common.NLog;

namespace Payments.WEB.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            NLog.LogInfo(this.GetType(), "Method RegisterArea execution");

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "List", id = UrlParameter.Optional }
            );
        }
    }
}