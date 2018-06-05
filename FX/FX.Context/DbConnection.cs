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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>()
                        .HasKey(n => new { n.UserId, n.RoleId })
                        .ToTable("ApplicationUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                        .HasKey(n => n.UserId)
                        .ToTable("ApplicationUserLogins");

            modelBuilder.Entity<IdentityUserClaim>()
                        .HasKey(n => n.UserId)
                        .ToTable("ApplicationUserClaims");

            modelBuilder.Entity<Role>()
                        .HasMany<Group>(s => s.Groups)
                        .WithMany(c => c.Roles)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("RoleId");
                            cs.MapRightKey("GroupId");
                            cs.ToTable("RoleGroups");
                        });
        }
    }
}