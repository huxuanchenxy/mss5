using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Interface
{
    public interface IActionRepo<T> where T:BaseEntity
    {
        Task<MSSResult<ActionView>> GetPageByParm(ActionQueryParm parm);
        Task<ActionInfo> GetByID(int Id);

        Task<int> Add(ActionInfo action);

        Task<int> Update(ActionInfo action);

        Task<int> Delete(string[] ids);

        Task<List<ActionInfo>> GetAll();

        Task<List<ActionInfo>> GetMenu();

        Task<List<ActionInfo>> GetByActionGroup(string[] groupIDs);
        Task<List<ActionAll>> GetActionAll();
        Task<List<ActionAll>> GetActionByUser(int userID);
        Task<List<RoleActions>> GetActionByRoles(int[] roleIDs);

        Task<List<int>> GetActionIDByRoleID(int roleID);
    }
}
