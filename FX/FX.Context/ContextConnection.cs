using FX.Context.IdentityDomain;
using System.Data.Entity;

namespace FX.Context
{
    public abstract class ContextConnection : DbConnection
    {
        public ContextConnection() : base()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> ApplicationRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<CompanyInfo> Companies { get; set; }
    }
}