using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS.API.Core.V1.Business;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using MSS.API.Common;
using MSS.API.Common.Utility;
using System.IO;
using static MSS.API.Common.MyDictionary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadFileService _uploadService;
        public UploadController(IUploadFileService uploadService)
        {
            _uploadService = uploadService;

        }
        [HttpPost]
        [RequestSizeLimit(52428800)]
        public ActionResult Save([FromForm]MyData myData,List<IFormFile> file)
        {
            var ret = _uploadService.Save(myData.type, myData.systemResource, file);
            return Ok(ret.Result);
            //return Ok("");
        }
        public class MyData
        {
            public int type { get; set; }
            public int systemResource { get; set; }
        }
        [HttpPost("SaveList")]
        public ActionResult Save(FilesByEntity filesByEntity)
        {
            //List<UploadFileRelation> ufrs = JsonConvert.DeserializeObject<List<UploadFileRelation>>(list);
            var ret = _uploadService.Save(ObjectToList(filesByEntity));
            return Ok(ret.Result);
            //return Ok("");
        }

        [HttpPost("SaveListV2")]
        public async Task<ActionResult<ApiResult>> SaveV2(FilesRelation filesByEntity)
        {
            //List<UploadFileRelation> ufrs = JsonConvert.DeserializeObject<List<UploadFileRelation>>(list);
            var ret = new ApiResult();
            return ret;
            //return Ok("");
        }
        public class FilesByEntity
        {
            public int entity { get; set; }
            public int systemResource { get; set; }
            public string files { get; set; }
        }


        public class FilesRelation
        {
            public int entityId { get; set; }
            public int systemResource { get; set; }
            public List<FilesObj> filesObj { get; set; }
        }

        public class FilesObj
        {
            public int fileId { get; set; }
            public int fileType { get; set; }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ret = _uploadService.Delete(id);
            return Ok(ret.Result);
        }

        [HttpGet("{ids}")]
        public ActionResult ListByIDs(string ids)
        {
            var ret = _uploadService.ListByIDs(ids);
            return Ok(ret.Result);
        }

        [HttpGet("Cascader/{ids}")]
        public ActionResult CascaderByIDs(string ids)
        {
            var ret = _uploadService.CascaderByIDs(ids);
            return Ok(ret.Result);
        }

        [HttpGet("ListByEntity/{entitys}/{sr}/{ust}")]
        public ActionResult ListByEntity(string entitys, SystemResource sr, UploadShowType ust)
        {
            var ret = _uploadService.ListByEntity(entitys.Split(',').Select(a=>Convert.ToInt32(a)).ToArray(), sr, ust);
            return Ok(ret.Result);
        }

        [HttpGet("ListByEntity2/{entitys}/{sr}")]
        public ActionResult ListByEntity2(string entitys, SystemResource sr)
        {
            var ret = _uploadService.ListByEntity(entitys.Split(',').Select(a => Convert.ToInt32(a)).ToArray(), sr);
            return Ok(ret.Result);
        }


        [HttpGet("ListByEqp/{id}")]
        public ActionResult ListByEqp(int id)
        {
            var ret = _uploadService.ListByEqp(id);
            return Ok(ret.Result);
        }

        [HttpPost("Download/{id}")]
        public ActionResult Download(int id)
        {

            UploadFile ret = (UploadFile)_uploadService.GetByID(id).Result.data;

            var url = (FilePath.BASEFILE + ret.FilePath).Replace('/', '\\');
            if (System.IO.File.Exists(url))
            {
                var stream = System.IO.File.OpenRead(url);
                return File(stream, "application/octet-stream", ret.FileName);
            }
            else
            {
                return File(new byte[] { }, "application/octet-stream");
            }
        }

        [HttpGet("FileIsExist/{id}")]
        public ActionResult FileIsExist(int id)
        {
            var ret = _uploadService.FileIsExist(id);
            return Ok(ret.Result);
        }

        private List<UploadFileRelation> ObjectToList(FilesByEntity fe)
        {
            List<UploadFileRelation> ret = new List<UploadFileRelation>();
            if (fe.files == "[]")
            {
                UploadFileRelation ufr = new UploadFileRelation();
                ufr.Entity = fe.entity;
                ufr.SystemResource = fe.systemResource;
                ret.Add(ufr);
            }
            else
            {
                JArray arr = JsonConvert.DeserializeObject<JArray>(fe.files);
                foreach (var item in arr)
                {
                    foreach (var id in item["ids"].ToString().Split(','))
                    {
                        UploadFileRelation ufr = new UploadFileRelation();
                        ufr.Entity = fe.entity;
                        ufr.SystemResource = fe.systemResource;
                        ufr.Type = Convert.ToInt32(item["type"]);
                        ufr.File = Convert.ToInt32(id);
                        ret.Add(ufr);
                    }
                }
            }
            return ret;
        }
    }
}