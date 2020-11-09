using Microsoft.AspNetCore.Mvc;
using MSS.API.Common;
using MSS.API.Common.Utility;
using MSS.API.Core.V1.Business;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrgController : ControllerBase
    {
        private readonly IOrgService _orgService;
        private readonly int _userId;
        private readonly IAuthHelper _authHelper;
        public OrgController(IOrgService orgService, IAuthHelper authHelper)

        {
            _orgService = orgService;
            _authHelper = authHelper;
            _userId = _authHelper.GetUserId();
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResult>> Get()
        {
            var ret = await _orgService.GetAllOrg();
            return ret;
        }

        [HttpGet("curorg")]
        public async Task<ActionResult<ApiResult>> GetByUserID()
        {
            int userId = _userId;
            var ret = await _orgService.GetOrgByUserID(userId);
            return ret;
        }

        [HttpGet("topnode/{id}")]
        public async Task<ActionResult<ApiResult>> GetTopNodeUserID(int id)
        {
            int userId = id;
            var ret = await _orgService.GetTopNodeByUserID(userId);
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult>> Get(int id)
        {
            var ret = await _orgService.GetOrgByIDs(new List<int>{id});
            return ret;
        }

        // 添加
        [HttpPost]
        public async Task<ActionResult<ApiResult>> AddOrgNode(OrgTree node)
        {
            node.CreatedBy = _userId;
            node.CreatedTime = DateTime.Now;
            node.IsDel = false;
            var ret = await _orgService.AddOrgNode(node);
            return ret;
        }

        // 更新
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult>> UpdateOrgNode(int id, OrgTree node)
        {
            node.UpdatedBy = _userId;
            node.UpdatedTime = DateTime.Now;
            var ret = await _orgService.UpdateOrgNode(node);
            // return CreatedAtRoute("GetById", new { id = id }, node);
            return ret;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            OrgTree node = new OrgTree();
            node.UpdatedBy = _userId;
            node.UpdatedTime = DateTime.Now;
            node.Id = id;
            var ret = await _orgService.DeleteOrgNode(node);

            return ret;
        }

        // 组织节点绑定用户
        [HttpPost("user")]
        public async Task<ActionResult<ApiResult>> BindOrgUser(OrgUserView nodeView)
        {
            nodeView.CreatedBy = _userId;
            nodeView.CreatedTime = DateTime.Now;
            var ret = await _orgService.BindOrgNodeUsers(nodeView);
            return ret;
        }
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ApiResult>> getOrgUser(int id)
        {
            var ret = await _orgService.GetOrgNodeUsers(id);
            return ret;
        }
        [HttpGet("user/all/{id}")]
        public async Task<ActionResult<ApiResult>> getCanSelectedUser(int id)
        {
            var ret = await _orgService.GetCanSelectedUsers(id);
            return ret;
        }
        [HttpGet("nodetype")]
        public async Task<ActionResult<ApiResult>> getNodeType()
        {
            var ret = await _orgService.GetNodeType();
            return ret;
        }
        [HttpGet("node/{id}")]
        public async Task<ActionResult<ApiResult>> getOrgNode(int id)
        {
            var ret = await _orgService.GetOrgNode(id);
            return ret;
        }
        [HttpGet("ListNodeByNodeType/{nodeType}")]
        public async Task<ActionResult<ApiResult>> ListNodeByNodeType(int nodeType)
        {
            var ret = await _orgService.ListNodeByNodeType(nodeType);
            return ret;
        }
    }
}