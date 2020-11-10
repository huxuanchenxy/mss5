using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Interface
{
    public interface IActionGroupRepo<T> where T:BaseEntity
    {
        Task<MSSResult<ActionGroupView>> GetPageByParm(ActionGroupQueryParm parm);
        Task<int> Add(ActionGroup actionGroup);

        Task<int> Update(ActionGroup actionGroup);

        Task<int> Delete(string[] ids);
        Task<ActionGroup> GetByID(int Id);

        Task<List<ActionGroup>> GetAll();
    }
}
