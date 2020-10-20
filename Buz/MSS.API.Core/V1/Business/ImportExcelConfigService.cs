using CSRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MSS.API.Common;
using MSS.API.Common.Utility;
using MSS.API.Dao.Implement;
using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MSS.API.Common.Const;

namespace MSS.API.Core.V1.Business
{
    public interface IImportExcelConfigService
    {
        Task<ApiResult> Save(ImportExcelConfig obj);
        Task<ApiResult> Update(ImportExcelConfig obj);
        Task<ApiResult> Delete(string ids);
        Task<ApiResult> GetByID(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> GetPageByParm(ImportExcelConfigParm parm);

        Task<ApiResult> ListClass();
        Task<ApiResult> GetClassByID(int id);

        Task<ApiResult> ListLogByParm(ImportExcelLogParm parm);
    }
    public class ImportExcelConfigService : IImportExcelConfigService
    {
        //private readonly ILogger<UserService> _logger;
        private readonly IImportExcelConfigRepo<ImportExcelConfig> _importExcelConfigRepo;
        private readonly IDistributedCache _cache;
        private IConfiguration _configuration { get; }
        private readonly int _userID;

        public ImportExcelConfigService(IImportExcelConfigRepo<ImportExcelConfig> importExcelConfigRepo,
            IAuthHelper auth, IDistributedCache cache, IConfiguration configuration)
        {
            //_logger = logger;
            _importExcelConfigRepo = importExcelConfigRepo;
            _cache = cache;
            _configuration = configuration;
            _userID = auth.GetUserId();
        }
        #region ImportExcelConfig
        public async Task<ApiResult> Save(ImportExcelConfig obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ImportExcelConfig has=await _importExcelConfigRepo.GetByFileName(obj.FileName);
                if (has != null)
                {
                    ret.code = Code.DataIsExist;
                    ret.msg = "已存在相同的导入文件名，不允许重复添加";
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    obj.UpdatedTime = dt;
                    obj.CreatedTime = dt;
                    obj.UpdatedBy = _userID;
                    obj.CreatedBy = _userID;
                    ret.data = await _importExcelConfigRepo.Save(obj);
                }
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> Update(ImportExcelConfig obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ImportExcelConfig et = await _importExcelConfigRepo.GetByID(obj.Id);
                if (et!=null)
                {
                    if (et.FileName!=obj.FileName)
                    {
                        ImportExcelConfig has = await _importExcelConfigRepo.GetByFileName(obj.FileName);
                        if (has != null)
                        {
                            ret.code = Code.DataIsExist;
                            ret.msg = "已存在相同的导入文件名，不允许重复添加";
                            return ret;
                        }
                    }
                    DateTime dt = DateTime.Now;
                    obj.UpdatedTime = dt;
                    obj.UpdatedBy = _userID;
                    ret.data = await _importExcelConfigRepo.Update(obj);
                }
                else
                {
                    ret.code = Code.DataIsnotExist;
                    ret.msg = "所要修改的数据不存在";
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> Delete(string ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _importExcelConfigRepo.Delete(ids.Split(','));
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        
        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _importExcelConfigRepo.GetByID(id);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> GetAll()
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _importExcelConfigRepo.GetAll();
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> GetPageByParm(ImportExcelConfigParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.page = parm.page == 0 ? 1 : parm.page;
                parm.rows = parm.rows == 0 ? PAGESIZE : parm.rows;
                parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                ret.data = await _importExcelConfigRepo.GetPageByParm(parm);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        #endregion

        #region ImportExcelClass
        public async Task<ApiResult> ListClass()
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _importExcelConfigRepo.ListClass();
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        public async Task<ApiResult> GetClassByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _importExcelConfigRepo.GetClassByID(id);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        #endregion

        #region ImportExcelLog
        public async Task<ApiResult> ListLogByParm(ImportExcelLogParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.page = parm.page == 0 ? 1 : parm.page;
                parm.rows = parm.rows == 0 ? PAGESIZE : parm.rows;
                parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                ret.data = await _importExcelConfigRepo.ListLogByParm(parm);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        #endregion

    }
}
