using FX.Context.IdentityDomain;
using FX.Data.DBFactory;
using FX.Data.RepositoryPattern;
using FX.Identity.Interface;

namespace FX.Identity.Implement
{
    public class RoleGroupService : BaseService<RoleGroup, int>, IRoleGroupService
    {
        public RoleGroupService(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}