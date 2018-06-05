using FX.Context.IdentityDomain;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace FX.Context
{
    public class DbConnection : IdentityDbContext<ApplicationUser>
    {
        public DbConnection() : base("ConnectionString")
        {
        }
    }
}