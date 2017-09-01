using Microsoft.AspNet.Identity;
using Payments.DAL.Entities;

namespace Payments.DAL.Identity
{
    // class is used to manage users
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            
        }
    }
}