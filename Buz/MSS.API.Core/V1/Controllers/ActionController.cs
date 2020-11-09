using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS.API.Common;
using MSS.API.Core.Infrastructure;
using MSS.API.Core.V1.Business;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IActionService _ActionService;
        public ActionController(IActionService ActionService)

        {
            //_logger = logger;
            //_mediator = mediator;
            //_cache = cache;
            _ActionService = ActionService;

        }
        [HttpGet("QueryList")]
        public async Task<ActionResult<ApiResult>> GetPageByParm([FromQuery] ActionQueryParm parm)
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionService.GetPageByParm(parm);
            if (ret.code==(int)ErrType.OK)
            {
                var data = new { rows = ret.data, total = ret.relatedData };
                //var resp = new { code = ret.code, data = data };
                resp.code = Code.Success;
                resp.data = data;
                return resp;
            }
            else
            {
                resp.code = 0;
                resp.msg = ret.msg;
                return resp;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetByID(int id)
        {
            ApiResult resp = new ApiResult();
            var ret =  await _ActionService.GetByID(id);
            if (ret != null)
            {
                resp.code = 0;
                resp.data = ret.data;
            }
            return resp;
        }
        [HttpPost("Add")]
        public async Task<ActionResult<ApiResult>> Add(ActionInfo action)
        {
            await _ActionService.Add(action);
            ApiResult resp = new ApiResult();
            resp.code = 0;
            return resp;
        }
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResult>> Update(ActionInfo action)
        {
            await _ActionService.Update(action);
            ApiResult resp = new ApiResult();
            resp.code = 0;
            return resp;
        }
        [HttpDelete("{ids}")]
        public async Task<ActionResult<ApiResult>> Delete(string ids)
        {
            await _ActionService.Delete(ids);
            ApiResult resp = new ApiResult();
            resp.code = 0;
            return resp;
        }
        [HttpGet("All")]
        public async Task<ActionResult<ApiResult>> GetAll()
        {
            ApiResult resp = new ApiResult();
            var ret =  await _ActionService.GetAll();
            if (ret != null)
            {
                resp.code = 0;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpGet("ActionTree")]
        public async Task<ActionResult<ApiResult>> GetActionTree()
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionService.GetActionTree();
            if (ret != null)
            {
                resp.code = 0;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpGet("Menu")]
        public async Task<ActionResult<ApiResult>> GetMenu()
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionService.GetMenu();
            if (ret != null)
            {
                resp.code = 0;
                resp.data = ret.data;
            }
            return resp;
        }
    }
}