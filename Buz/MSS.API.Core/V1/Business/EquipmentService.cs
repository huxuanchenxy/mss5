using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MSS.API.Model.DTO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MSS.API.Common;
using static MSS.API.Common.Const;
using static MSS.API.Common.FilePath;
using Microsoft.AspNetCore.Http;
using MSS.API.Common.Utility;
using System.IO;
using MSS.Common.Consul;
using System.Data;
using MSS.API.Dao.Implement;

namespace MSS.API.Core.V1.Business
{
    public interface IEquipmentService
    {
        Task<ApiResult> Import(IFormFile file);
        Task<ApiResult> Save(Equipment eqp);
        Task<ApiResult> Update(Equipment eqp);
        Task<ApiResult> Delete(string ids);
        Task<ApiResult> GetPageByParm(EqpQueryParm parm);
        Task<ApiResult> GetByID(int id);
        Task<ApiResult> GetDetailByID(int id);
        Task<ApiResult> ListByPosition(int location, int locationBy, int eqpType);
        Task<ApiResult> ListByTopOrg(int topOrg, int line, int location = 0, int locationBy = 0);
        Task<ApiResult> ListByEqpType(string ids);
        Task<ApiResult> ListByTeam(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> CountAllEqp();

        // 
        Task<ApiResult> ListEqpByIDs(EqpQueryByIDParm parm);
    }
    public class EquipmentService :IEquipmentService
    {
        //private readonly ILogger<UserService> _logger;
        private readonly IEquipmentRepo<Equipment> _eqpRepo;
        private readonly IUploadFileRepo<UploadFile> _uploadFileRepo;
        private readonly IImportExcelConfigRepo<ImportExcelConfig> _importExcelConfigRepo;

        private readonly int userID;

        public EquipmentService(IEquipmentRepo<Equipment> eqpRepo, IAuthHelper auth, 
            IImportExcelConfigRepo<ImportExcelConfig> importExcelConfigRepo, 
            IUploadFileRepo<UploadFile> uploadFileRepo)
        {
            //_logger = logger;
            _eqpRepo = eqpRepo;
            _importExcelConfigRepo = importExcelConfigRepo;
            _uploadFileRepo = uploadFileRepo;
            userID = auth.GetUserId();
        }
        public async Task<ApiResult> Import(IFormFile file)
        {
            ApiResult ret = new ApiResult();
            ret.code = Code.ImportError;
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                ImportExcelConfig oneConfig = await _importExcelConfigRepo.GetByFileName(fileName);
                ImportExcelClass oneClass = await _importExcelConfigRepo.GetClassByID(oneConfig.ClassID);
                ImportExcelHelper importExcelHelper = new ImportExcelHelper(oneClass.FullName, oneClass.AssemblyName);
                string[] config = oneConfig.Config.Split(',');
                string[] required = oneConfig.Required.Split(',');
                string errMsg = "";
                ImportExcelLog log = new ImportExcelLog();
                DataTable dt = importExcelHelper.GetData(file, config, required,userID, ref log, ref errMsg);
                if (errMsg!="")
                {
                    ret.msg = errMsg;
                    return ret;
                }
                DateTime now = DateTime.Now;
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    i++;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (row["online_date"].ToString() == "") errMsg += "第" + i + "行上线时间必填";
                        else if (row["medium_repair"].ToString() == "") errMsg += "第" + i + "行中修频率必填";
                        else if (row["large_repair"].ToString() == "") errMsg += "第" + i + "行大修频率必填";
                        if (errMsg != "")
                        {
                            ret.msg = errMsg;
                            return ret;
                        }
                        else
                        {
                            DateTime onLine = Convert.ToDateTime(row["online_date"]);
                            int mediumRepair = Convert.ToInt32(row["medium_repair"]);
                            int largeRepair = Convert.ToInt32(row["large_repair"]);
                            row["next_medium_repair_date"] = onLine.AddDays(mediumRepair);
                            row["next_large_repair_date"] = onLine.AddDays(largeRepair);
                        }
                        //没有导入的非字符串字段需要默认值
                        if (row[col].ToString() == "")
                        {
                            string typeName= col.DataType.Name;
                            switch (typeName)
                            {
                                case "Int32":
                                case "Double":
                                    row[col] = 0;
                                    break;
                                case "DateTime":
                                    row[col] = new DateTime(1970,1,1);
                                    break;
                            }
                        }
                    }
                    //系统赋值
                    row["created_by"] = userID;
                    row["updated_by"] = userID;
                    row["created_time"] = now;
                    row["updated_time"] = now;
                    row["is_del"] = 0;
                }
                log.CreatedBy = userID;
                log.CreatedTime = now;
                dt.TableName = "equipment_copy";
                using (TransactionScope scope = new TransactionScope())
                {
                    ret.data = _importExcelConfigRepo.BulkLoad(dt);
                    await _importExcelConfigRepo.SaveLog(log);
                    scope.Complete();
                }
                ret.code = Code.Success;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> Save(Equipment eqp)
        {
            ApiResult ret = new ApiResult();
            try
            {
                if (await _eqpRepo.CodeIsRepeat(eqp.Code))
                {
                    ret.code = Code.DataIsExist;
                    ret.msg = "设备图纸编号不可重复";
                }
                DateTime dt = DateTime.Now;
                eqp.UpdatedTime = dt;
                eqp.CreatedTime = dt;
                eqp.UpdatedBy = userID;
                eqp.CreatedBy = userID;
                eqp.NextLargeRepairDate = eqp.Online.AddDays(eqp.LargeRepair);
                eqp.NextMediumRepairDate = eqp.Online.AddDays(eqp.MediumRepair);
                ret.data = await _eqpRepo.Save(eqp);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> Update(Equipment eqp)
        {
            ApiResult ret = new ApiResult();
            try
            {
                Equipment et = await _eqpRepo.GetByID(eqp.ID);
                if (et!=null)
                {
                    if (eqp.Code!=et.Code && await _eqpRepo.CodeIsRepeat(eqp.Code))
                    {
                        ret.code = Code.DataIsExist;
                        ret.msg = "设备图纸编号不可重复";
                    }
                    DateTime dt = DateTime.Now;
                    eqp.UpdatedTime = dt;
                    eqp.UpdatedBy = userID;
                    if (et.NextMediumRepairDate == DateTime.MinValue && et.NextLargeRepairDate == DateTime.MinValue)
                    {
                        eqp.NextLargeRepairDate = eqp.Online.AddDays(eqp.LargeRepair);
                        eqp.NextMediumRepairDate = eqp.Online.AddDays(eqp.MediumRepair);
                    }
                    ret.data = await _eqpRepo.Update(eqp);
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
                ret.data = await _eqpRepo.Delete(ids.Split(','),userID);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> GetPageByParm(EqpQueryParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.SearchTopOrg= await getTopOrgByUser();
                if (parm.SearchTopOrg != 0)
                {
                    parm.page = parm.page == 0 ? 1 : parm.page;
                    parm.rows = parm.rows == 0 ? PAGESIZE : parm.rows;
                    parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                    parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                    EqpView ev = await _eqpRepo.GetPageByParm(parm);
                    List<Equipment> eqps = ev.rows;
                    List<AllArea> laa = await _eqpRepo.GetAllArea();
                    foreach (var item in eqps)
                    {
                        var tmp = laa.Where(a => a.Tablename == item.LocationBy && a.ID == item.Location)
                            .FirstOrDefault();
                        if (tmp!=null) item.LocationName = tmp.AreaName;
                    }
                    ret.data = ev;
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
        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                Equipment e = await _eqpRepo.GetByID(id);
                var list = await _uploadFileRepo.ListByEntity(new int[] { id},MyDictionary.SystemResource.Eqp);
                if (list != null)
                {
                    e.FileIDs = JsonConvert.SerializeObject(UploadFileHelper.ListShow(list));
                }
                ret.data = e;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
        public async Task<ApiResult> GetDetailByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                Equipment e = await _eqpRepo.GetDetailByID(id);
                List<AllArea> laa = await _eqpRepo.GetAllArea();
                e.LocationName = laa.Where(a => a.Tablename == e.LocationBy && a.ID == e.Location)
                    .FirstOrDefault().AreaName;
                var list = await _uploadFileRepo.ListByEntity(new int[] { id},MyDictionary.SystemResource.Eqp);
                if (list!=null)
                {
                    e.FileIDs = JsonConvert.SerializeObject(UploadFileHelper.ListShow(list));
                }
                ret.data = e;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListByPosition(int location, int locationBy,int eqpType)
        {
            ApiResult ret = new ApiResult();
            try
            {
                int? topOrg = await getTopOrgByUser();
                if (topOrg != 0)
                {
                    ret.data = await _eqpRepo.ListByPosition(location, locationBy, eqpType, topOrg);
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
        public async Task<ApiResult> ListByTopOrg(int topOrg,int line,int location=0,int locationBy=0)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _eqpRepo.ListByTopOrg(new List<int>() { topOrg },line, location, locationBy);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> ListByEqpType(string ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _eqpRepo.ListByEqpType(ids);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        public async Task<ApiResult> ListByTeam(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _eqpRepo.ListByTeam(id);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> GetAll()
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _eqpRepo.GetAll();
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        private async Task<int?> getTopOrgByUser()
        {
            //var _services = await _consulServiceProvider.GetServiceAsync("AuthService");
            var _services = "http://localhost:8981";
            IHttpClientHelper<ApiResult> h = new HttpClientHelper<ApiResult>();
            ApiResult r = await h.GetSingleItemRequest(_services + "/api/v1/user/" + userID);
            JObject jobj = JsonConvert.DeserializeObject<JObject>(r.data.ToString());
            if ((bool)jobj["is_super"])
            {
                return null;
            }
            else
            {
                //_services = await _consulServiceProvider.GetServiceAsync("OrgService");
                _services = "http://localhost:8082";
                IHttpClientHelper<ApiResult> httpHelper = new HttpClientHelper<ApiResult>();
                ApiResult result = await httpHelper.GetSingleItemRequest(_services + "/api/v1/org/topnode/" + userID);
                if (result.data != null)
                {
                    JObject obj = JsonConvert.DeserializeObject<JObject>(result.data.ToString());
                    return Convert.ToInt32(obj["id"]);
                }
                return 0;
            }
        }

        public async Task<ApiResult> CountAllEqp()
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _eqpRepo.CountAllEqp();
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListEqpByIDs(EqpQueryByIDParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.page = parm.page == 0 ? 1 : parm.page;
                parm.rows = parm.rows == 0 ? PAGESIZE : parm.rows;
                parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                EqpView ev = await _eqpRepo.ListEqpByIDs(parm);
                List<Equipment> eqps = ev.rows;
                List<AllArea> laa = await _eqpRepo.GetAllArea();
                foreach (var item in eqps)
                {
                    var tmp = laa.Where(a => a.Tablename == item.LocationBy && a.ID == item.Location)
                        .FirstOrDefault();
                    if (tmp != null) item.LocationName = tmp.AreaName;
                }
                ret.data = ev;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }
    }
}
