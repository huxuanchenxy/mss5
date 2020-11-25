using Microsoft.AspNetCore.Mvc;
using MSS.API.Common;
using MSS.API.Core.V1.Business;
using MSS.API.Model.DTO;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _RoleService;
        public RoleController(IRoleService RoleService)

        {
            //_logger = logger;
            //_mediator = mediator;
            //_cache = cache;
            _RoleService = RoleService;

        }
        [HttpGet("QueryList")]
        public async Task<ActionResult<ApiResult>> GetPageByParm([FromQuery] RoleQueryParm parm)
        {
            ApiResult resp = new ApiResult();
            var ret = await _RoleService.GetPageByParm(parm);
            if (ret.code==(int)ErrType.OK)
            {
                var data = new { rows = ret.data, total = ret.relatedData };
                resp.code = Code.Success;
                resp.data = data;
                return resp;
            }
            else
            {
                resp.code = Code.Failure;
                resp.msg = ret.msg;
                return resp;
            }
        }

        //TODO 测试
        [HttpGet("ActionTree")]
        public async Task<ActionResult<ApiResult>> GetActionTree()
        {
            ApiResult resp = new ApiResult();
            var ret = await _RoleService.GetActionTree();
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetByID(int id)
        {
            ApiResult resp = new ApiResult();
            var ret = await  _RoleService.GetByID(id);
            if (ret.code == (int)ErrType.OK)
            {
                var data = new { selectedAction = ret.data, role = ret.relatedData };
                resp.code = Code.Success;
                resp.data = data;
                return resp;
            }
            else
            {
                resp.code = Code.Failure;
                resp.msg = ret.msg;
                return resp;
            }
        }
        [HttpPost("Add")]
        public async Task<ActionResult<ApiResult>> Add(RoleStrActions roleStrActions)
        {
            await _RoleService.Add(roleStrActions);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResult>> Update(RoleStrActions roleStrActions)
        {
            await _RoleService.Update(roleStrActions);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }
        [HttpDelete("{ids}")]
        public async Task<ActionResult<ApiResult>> Delete(string ids)
        {
            await _RoleService.Delete(ids);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }
        [HttpGet("All")]
        public async Task<ActionResult<ApiResult>> GetAll()
        {
            ApiResult resp = new ApiResult();
            var ret = await _RoleService.GetAll();
            resp.code = Code.Success;
            resp.data = ret.data;
            return resp;
        }
    }
}