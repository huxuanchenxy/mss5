using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Business
{
    public interface IRoleService
    {
        Task<MSSResult<RoleView>> GetPageByParm(RoleQueryParm parm);
        Task<MSSResult<int>> GetByID(int id);

        Task<MSSResult> Add(RoleStrActions roleStrActions);
        Task<MSSResult> Update(RoleStrActions roleStrActions);
        Task<MSSResult> Delete(string ids);
        Task<MSSResult> GetAll();
    }
}
