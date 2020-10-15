using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Business
{
    public interface IActionService
    {
        Task<MSSResult<ActionView>> GetPageByParm(ActionQueryParm parm);
        Task<MSSResult> GetByID(int id);

        Task<MSSResult> Add(ActionInfo action);
        Task<MSSResult> Update(ActionInfo action);
        Task<MSSResult> Delete(string ids);
        Task<MSSResult> GetAll();
        Task<MSSResult> GetMenu();
        Task<MSSResult<ActionTree>> GetActionTree();
    }
}
