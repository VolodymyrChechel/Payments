using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Payments.Common.NLog;
using Payments.DAL.Entities;

namespace Payments.DAL.EF
{
    // context for database
    public class PaymentsContext : IdentityDbContext<ApplicationUser>
    {
        public PaymentsContext(string connectionString) : base(connectionString)
        {
            NLog.LogInfo(this.GetType(), "Constructor PaymentsContext execution");

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PaymentsContext>());
        }

        public IDbSet<ClientProfile> ClientProfiles { get; set; }
        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<CreditAccount> CreditAccounts { get; set; }
        public IDbSet<DebitAccount> DebitAccounts { get; set; }
        public IDbSet<Payment> Payments { get; set; }
        public IDbSet<UnblockAccountRequest> UnblockAccountRequests { get; set; }
    }
}