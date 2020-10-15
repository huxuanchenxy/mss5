using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Business
{
    public interface IActionGroupService
    {
        Task<MSSResult<ActionGroupView>> GetPageByParm(ActionGroupQueryParm parm);
        Task<MSSResult> GetByID(int id);

        Task<MSSResult> Add(ActionGroup actionGroup);
        Task<MSSResult> Update(ActionGroup actionGroup);
        Task<MSSResult> Delete(string ids);
        Task<MSSResult> GetAll();
    }
}
