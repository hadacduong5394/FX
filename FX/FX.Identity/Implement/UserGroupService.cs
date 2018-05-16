using FX.Context.IdentityDomain;
using FX.Data.DBFactory;
using FX.Data.RepositoryPattern;
using FX.Identity.Interface;

namespace FX.Identity.Implement
{
    public class UserGroupService : BaseService<UserGroup, string>, IUserGroupService
    {
        public UserGroupService(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}