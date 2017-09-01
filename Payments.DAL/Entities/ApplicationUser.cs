using Microsoft.AspNet.Identity.EntityFramework;

namespace Payments.DAL.Entities
{
    // custom ApplicationUser for authorization
    public class ApplicationUser : IdentityUser
    {
        // one-to-one relationship with additional information about user
        public virtual ClientProfile ClientProfile { get; set; }
    }
}