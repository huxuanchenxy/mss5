using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Interface
{
    public interface IRoleRepo<T> where T:BaseEntity
    {
        Task<MSSResult<RoleView>> GetPageByParm(RoleQueryParm parm);
        Task<Role> GetByID(int Id);
        Task<bool> IsNameRepeat(string name);
        Task<int> Add(RoleStrActions roleStrActions);

        Task<int> Update(RoleStrActions roleStrActions);

        Task<int> Delete(string[] ids);

        Task<List<Role>> GetAll();
        Task<List<RoleAction>> GetRoleActionAll();
    }
}
