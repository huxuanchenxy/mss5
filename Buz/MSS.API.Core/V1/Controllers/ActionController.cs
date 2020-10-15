using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult GetPageByParm([FromQuery] ActionQueryParm parm)
        {
            //ActionQueryParm parm = new ActionQueryParm();

            var ret = _ActionService.GetPageByParm(parm).Result;
            if (ret.code==(int)ErrType.OK)
            {
                var data = new { rows = ret.data, total = ret.relatedData };
                var resp = new { code = ret.code, data = data };
                return Ok(resp);
            }
            else
            {
                var resp = new { code = ret.code, msg = ret.msg };
                return Ok(resp);
            }
        }
        [HttpGet("{id}")]
        public ActionResult GetByID(int id)
        {
            var resp = _ActionService.GetByID(id);
            //var resp = _ActionService.GetStrByID(id);
            return Ok(resp.Result);
        }
        [HttpPost("Add")]
        public ActionResult Add(ActionInfo action)
        {
            var resp = _ActionService.Add(action);
            return Ok(resp.Result);
        }
        [HttpPut("Update")]
        public ActionResult Update(ActionInfo action)
        {
            var resp = _ActionService.Update(action);
            return Ok(resp.Result);
        }
        [HttpDelete("{ids}")]
        public ActionResult Delete(string ids)
        {
            var resp = _ActionService.Delete(ids);
            return Ok(resp.Result);
        }
        [HttpGet("All")]
        public ActionResult GetAll()
        {
            var resp = _ActionService.GetAll();
            return Ok(resp.Result);
        }

        [HttpGet("Menu")]
        public ActionResult GetMenu()
        {
            var resp = _ActionService.GetMenu();
            return Ok(resp.Result);
        }

        [HttpGet("ActionTree")]
        public ActionResult GetActionTree()
        {
            var resp = _ActionService.GetActionTree();
            return Ok(resp.Result);
        }
    }
}