using FX.Context.IdentityDomain;
using FX.Data.RepositoryPattern;
using System.Collections.Generic;

namespace FX.Identity.Interface
{
    public interface IGroupService : IBaseService<Group, int>
    {
        bool CheckContainName(int comId, int id, string name);

        IList<Group> GetbyFilter(int comId, string keyWord, int currentPage, int pageSize, out int total);

        bool Create(Group entity, IList<int> roleIds, out string message);

        bool Update(Group entity, IList<int> roleIds, out string message);

        bool Delete(int groupId, out string message);

        IList<Role> GetRolesOfGroup(int groupId);
    }
}