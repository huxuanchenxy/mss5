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
    public class ActionGroupController : ControllerBase
    {
        private readonly IActionGroupService _ActionGroupService;
        public ActionGroupController(IActionGroupService ActionGroupService)

        {
            //_logger = logger;
            //_mediator = mediator;
            //_cache = cache;
            _ActionGroupService = ActionGroupService;

        }
        [HttpGet("QueryList")]
        public async Task<ActionResult<ApiResult>> GetPageByParm([FromQuery] ActionGroupQueryParm parm)
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionGroupService.GetPageByParm(parm);
            if (ret.code==(int)ErrType.OK)
            {
                var data = new { rows = ret.data, total = ret.relatedData };
                resp.code = 0;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> GetByID(int id)
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionGroupService.GetByID(id);
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ApiResult>> Add(ActionGroup actionGroup)
        {
            await _ActionGroupService.Add(actionGroup);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResult>> Update(ActionGroup actionGroup)
        {
            await _ActionGroupService.Update(actionGroup);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }

        [HttpGet("All")]
        public async Task<ActionResult<ApiResult>> GetAll()
        {
            ApiResult resp = new ApiResult();
            var ret = await _ActionGroupService.GetAll();
            if (ret != null)
            {
                resp.code = Code.Success;
                resp.data = ret.data;
            }
            return resp;
        }

        [HttpDelete("{ids}")]
        public async Task<ActionResult<ApiResult>> Delete(string ids)
        {
            await _ActionGroupService.Delete(ids);
            ApiResult resp = new ApiResult();
            resp.code = Code.Success;
            return resp;
        }
    }
}