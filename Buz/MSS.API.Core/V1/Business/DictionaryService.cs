using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using MSS.API.Common.Utility;
using MSS.API.Common;

namespace MSS.API.Core.V1.Business
{
    public class DictionaryService: IDictionaryService
    {
        //private readonly ILogger<DictionaryService> _logger;
        private readonly IDictionaryRepo<DictionaryTree> _DictionaryRepo;

        //private readonly int userID;
        public DictionaryService(IDictionaryRepo<DictionaryTree> DictionaryRepo, IAuthHelper auth)
        {
            //_logger = logger;
            _DictionaryRepo = DictionaryRepo;

            //userID = auth.GetUserId();
        }
        public async Task<ApiResult> GetSubByCode(int pid)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                mRet.data = await _DictionaryRepo.GetSubByCode(pid);
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> GetSubByCodeOrder(int pid)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                mRet.data = await _DictionaryRepo.GetSubByCodeOrder(pid);
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> GetTwoCascader(int pid)
        {
            ApiResult mRet = new ApiResult();
            List<cascader> cascaders = new List<cascader>();
            try
            {
                List<DictionaryTree> dts= await _DictionaryRepo.GetSubByCodeOrder(pid);
                List<DictionaryTree> dts2= await _DictionaryRepo.GetSubByCodesOrder(dts.Select(a=>a.Id));
                foreach (var item in dts)
                {
                    cascader c = new cascader();
                    c.value = item.Id;
                    c.label = item.name;
                    c.children = new List<cascader>();
                    foreach (var item2 in dts2.Where(a=>a.parent_id==item.Id))
                    {
                        cascader c2 = new cascader();
                        c2.value = item2.Id;
                        c2.label = item2.name;
                        c.children.Add(c2);
                    }
                    cascaders.Add(c);
                }
                mRet.data = cascaders;
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> GetByParent(int pid)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                mRet.data = await _DictionaryRepo.GetByParent(pid);
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }




    }
}
