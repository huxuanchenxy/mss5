using MSS.API.Common;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Business
{
    public interface IDictionaryService
    {
        Task<ApiResult> GetSubByCode(int pid);
        Task<ApiResult> GetSubByCodeOrder(int pid);
        Task<ApiResult> GetTwoCascader(int pid);
        Task<ApiResult> GetByParent(int pid);
        //Task<MSSResult<DictionaryView>> GetPageByParm(DictionaryQueryParm parm);
        //Task<MSSResult> GetByID(int id);

        //Task<MSSResult> Add(Dictionary Dictionary);
        //Task<MSSResult> Update(Dictionary Dictionary);
        //Task<MSSResult> Delete(string ids);
        //Task<MSSResult> GetSubByCode(string code);
    }
}
