using Microsoft.AspNetCore.Mvc;
using MSS.API.Core.V1.Business;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _DictionaryService;
        public DictionaryController(IDictionaryService DictionaryService)

        {
            //_logger = logger;
            //_mediator = mediator;
            //_cache = cache;
            _DictionaryService = DictionaryService;

        }
        [HttpGet("SubCode/{code}")]
        public ActionResult GetSubByCode(int code)
        {
            var resp = _DictionaryService.GetSubByCode(code);
            return Ok(resp.Result);
        }

        [HttpGet("SubCodeByOrder/{code}")]
        public ActionResult GetSubByCodeOrder(int code)
        {
            var resp = _DictionaryService.GetSubByCodeOrder(code);
            return Ok(resp.Result);
        }

        [HttpGet("GetTwoCascader/{code}")]
        public ActionResult GetTwoCascader(int code)
        {
            var resp = _DictionaryService.GetTwoCascader(code);
            return Ok(resp.Result);
        }

        [HttpGet("BusinessType/{code}")]
        public ActionResult GetByParent(int code)
        {
            var resp = _DictionaryService.GetByParent(code);
            return Ok(resp.Result);
        }
        //[HttpGet("QueryList")]
        //public ActionResult GetPageByParm([FromQuery] DictionaryQueryParm parm)
        //{
        //    var ret = _DictionaryService.GetPageByParm(parm).Result;
        //    if (ret.code==(int)ErrType.OK)
        //    {
        //        var data = new { rows = ret.data, total = ret.relatedData };
        //        var resp = new { code = ret.code, data = data };
        //        return Ok(resp);
        //    }
        //    else
        //    {
        //        var resp = new { code = ret.code, msg = ret.msg };
        //        return Ok(resp);
        //    }
        //}
        //[HttpGet("{id}")]
        //public ActionResult GetByID(int id)
        //{
        //    var resp = _DictionaryService.GetByID(id);
        //    return Ok(resp.Result);
        //}
        //[HttpPost("Add")]
        //public ActionResult Add(Dictionary Dictionary)
        //{
        //    var resp = _DictionaryService.Add(Dictionary);
        //    return Ok(resp.Result);
        //}
        //[HttpPut("Update")]
        //public ActionResult Update(Dictionary Dictionary)
        //{
        //    var resp = _DictionaryService.Update(Dictionary);
        //    return Ok(resp.Result);
        //}
        //[HttpDelete("{ids}")]
        //public ActionResult Delete(string ids)
        //{
        //    var resp = _DictionaryService.Delete(ids);
        //    return Ok(resp.Result);
        //}
        //[HttpGet("SubCode/{code}")]
        //public ActionResult GetSubByCode(string code)
        //{
        //    var resp = _DictionaryService.GetSubByCode(code);
        //    return Ok(resp.Result);
        //}
    }
}