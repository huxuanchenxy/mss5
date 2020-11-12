using Microsoft.AspNetCore.Mvc;
using MSS.API.Common;
using MSS.API.Core.V1.Business;
using System.Threading.Tasks;

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
        public async Task<ActionResult<ApiResult>> GetSubByCode(int code)
        {
            ApiResult resp = new ApiResult();
            var ret = await _DictionaryService.GetSubByCode(code);
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpGet("SubCodeByOrder/{code}")]
        public async Task<ActionResult<ApiResult>> GetSubByCodeOrder(int code)
        {
            ApiResult resp = new ApiResult();
            var ret = await _DictionaryService.GetSubByCodeOrder(code);
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpGet("GetTwoCascader/{code}")]
        public async Task<ActionResult<ApiResult>> GetTwoCascader(int code)
        {
            ApiResult resp = new ApiResult();
            var ret = await _DictionaryService.GetTwoCascader(code);
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpGet("BusinessType/{code}")]
        public async Task<ActionResult<ApiResult>> GetByParent(int code)
        {
            ApiResult resp = new ApiResult();
            var ret = await _DictionaryService.GetByParent(code);
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }
        
    }
}