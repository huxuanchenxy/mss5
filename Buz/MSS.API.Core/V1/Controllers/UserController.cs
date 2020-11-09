using Microsoft.AspNetCore.Mvc;
using MSS.API.Common;
using MSS.API.Core.V1.Business;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core.V1.Controllers
{
    [Route("api/v1/[controller]")]
    //[EnableCors("AllowAll")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService UserService)

        {
            //_logger = logger;
            //_mediator = mediator;
            //_cache = cache;
            _service = UserService;

        }

        [HttpGet("GetPageList")]
        public async Task<ActionResult<ApiResult>> GetPageList([FromQuery] UserParm parm)
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.GetPageList(parm);

            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "获取分页数据User失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        [HttpGet()]
        public async Task<ActionResult<ApiResult>> GetByID()
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.GetByID(1);//TODO: 上线改成0

            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "获取单个数据User失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }



        [HttpPost]
        public async Task<ActionResult<ApiResult>> Save(User obj)
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.Save(obj);
            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "新增数据User失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        [HttpPut]
        public async Task<ActionResult<ApiResult>> Update(User obj)
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.Update(obj);
            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "更新数据User失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        [HttpDelete("{ids}")]
        public async Task<ActionResult<ApiResult>> Delete(string ids)
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.Delete(ids);
            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "删除数据User失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        [HttpGet("All")]
        public ActionResult GetAll()
        {
            var resp = _service.GetAll();
            return Ok(resp.Result);
        }

        [HttpPut("ChangePwd/{oldPwd}/{newPwd}")]
        public ActionResult ChangePwd(string oldPwd,string newPwd)
        {
            var resp = _service.ChangePwd(oldPwd, newPwd);
            return Ok(resp.Result);
        }


        [HttpPut("ResetPwd/{ids}")]
        public ActionResult ResetPwd(string ids)
        {
            var resp = _service.ResetPwd(ids);
            return Ok(resp.Result);
        }

        [HttpGet("Login/{acc}/{pwd}")]//弃用已改成token方式登录
        public ActionResult Login(string acc,string pwd)
        {
            var resp = _service.CheckUserLogin(acc,pwd).Result;
            //if (resp.code == (int)ErrType.OK)
            //{
            //    User u = (User)resp.relatedData;
            //    HttpContext.Session.SetInt32("UserID", u.id);
            //    HttpContext.Session.SetString("UserName", u.user_name);
            //}
            return Ok(resp);
        }

        [HttpGet("GetMenu")]
        public string GetMenu()
        {
            //int userID = (int)HttpContext.Session.GetInt32("UserID");
            //int userID = 1;
            //var resp = _UserService.GetByID(userID);
            //User u = (User)resp.Result.data;
            //MSSResult<MenuTree> ret = new MSSResult<MenuTree>();
            //if (u.is_super)
            //{
            //    ret = _UserService.GetMenu().Result;
            //}
            //else
            //{
            //    ret = _UserService.GetMenu(userID).Result;
            //}
            MSSResult<MenuTree> ret = _service.GetMenu().Result;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            int i = 0;
            foreach (var item in ret.data)
            {
                i++;
                //strJson.Append("\"" + item.path + "\": " + JsonConvert.SerializeObject(item));
                strJson.Append("\"" + item.order + "\": " + JsonConvert.SerializeObject(item));
                if (i < ret.data.Count()) strJson.Append(",");
            }
            strJson.Append("}");
            return strJson.ToString();
        }

        [HttpGet("GetAction")]
        public ActionResult GetAction()
        {
            var resp = _service.GetActionByUser();
            return Ok(resp.Result);
        }

    }
}