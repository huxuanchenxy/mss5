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
using static MSS.API.Common.MyDictionary;

namespace MSS.API.Core.V1.Business
{
    public interface IUploadFileService
    {
        Task<ApiResult> Save(int type, int systemResource, List<IFormFile> file);
        Task<ApiResult> Save(List<UploadFileRelation> ufrs);
        Task<ApiResult> Delete(int id);
        Task<ApiResult> GetByID(int id);
        Task<ApiResult> ListByIDs(string ids);
        Task<ApiResult> ListByEntity(int[] entitys, SystemResource sr, UploadShowType ust);
        Task<ApiResult> ListByEntity(int[] entitys, SystemResource sr);
        Task<ApiResult> ListByEqp(int id);
        Task<ApiResult> ListAll();
        Task<ApiResult> CascaderByIDs(string ids);

        Task<ApiResult> FileIsExist(int id);
    }
    public class UploadFileService : IUploadFileService
    {
        //private readonly ILogger<UserService> _logger;
        private readonly IUploadFileRepo<UploadFile> _uploadFileRepo;

        public UploadFileService(IUploadFileRepo<UploadFile> uploadFileRepo)
        {
            //_logger = logger;
            _uploadFileRepo = uploadFileRepo;
        }

        public async Task<ApiResult> Save(int type,int systemResource, List<IFormFile> file)
        {
            ApiResult ret = new ApiResult();
            UploadFile uf = new UploadFile();
            try
            {
                PDFHelper pdf = new PDFHelper();
                uf.FilePath = pdf.GetSavePDFPath(file, type, systemResource);
                uf.FileName = file.FirstOrDefault().FileName;
                ret.data = await _uploadFileRepo.Save(uf);
                // 当数据库插入不成功时，则不上传文件
                if (ret.data!=null)
                {
                    pdf.SavePDF(file.FirstOrDefault(), uf.FilePath);
                }
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> Save(List<UploadFileRelation> ufrs)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _uploadFileRepo.Save(ufrs);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> Delete(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                PDFHelper pdf = new PDFHelper();
                var file = await _uploadFileRepo.GetByID(id);
                if (file != null)
                {
                    ret.data = await _uploadFileRepo.Delete(id);
                    // 数据库删除记录成功后删除文件
                    if ((int)ret.data>0)
                    {
                        pdf.DeletePDF(file.FilePath);
                    }
                }
                else
                {
                    ret.code = Code.DataIsnotExist;
                    ret.msg = "所要删除的文档不存在";
                }
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _uploadFileRepo.GetByID(id);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListByIDs(string ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<UploadFile> ufs = await _uploadFileRepo.ListByIDs(ids);
                if (ufs != null)
                {
                    ret.data = UploadFileHelper.ListShow(ufs);
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

        public async Task<ApiResult> CascaderByIDs(string ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<UploadFile> ufs = await _uploadFileRepo.ListByIDs(ids);
                ret.data = UploadFileHelper.CascaderShow(ufs);
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListByEntity(int[] entitys, SystemResource sr, UploadShowType ust)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<object> objs = new List<object>();
                List<UploadFile> ufs = await _uploadFileRepo.ListByEntity(entitys,sr);
                if (ufs != null && ufs.Count() > 0)
                {
                    IEnumerable<IGrouping<int, UploadFile>> groupAction = ufs.GroupBy(a => a.Entity);
                    foreach (IGrouping<int, UploadFile> group in groupAction)
                    {
                        if (ust== UploadShowType.Cascader)
                        {
                            objs.Add(new {
                                Entity= group.Key,
                                UploadFiles= JsonConvert.SerializeObject(UploadFileHelper.CascaderShow(group.ToList()))
                            });
                        }
                        else if (ust == UploadShowType.List)
                        {
                            objs.Add(new
                            {
                                Entity = group.Key,
                                UploadFiles = JsonConvert.SerializeObject(UploadFileHelper.ListShow(group.ToList()))
                            });
                        }
                    }
                }
                ret.data = objs;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListByEqp(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<UploadFile> ufs = await _uploadFileRepo.ListByEntity(new int[] { id},SystemResource.Eqp);
                if (ufs!=null)
                {
                    ret.data = UploadFileHelper.TimeLineShow(ufs);
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

        public async Task<ApiResult> ListAll()
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _uploadFileRepo.ListAll();
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> ListByEntity(int[] entitys, SystemResource sr)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<UploadFile> ufs = await _uploadFileRepo.ListByEntity(entitys, sr);
                ret.data = ufs;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> FileIsExist(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                UploadFile uf = await _uploadFileRepo.GetByID(id);
                ret.data = File.Exists((BASEFILE + uf.FilePath).Replace('/', '\\'));
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
