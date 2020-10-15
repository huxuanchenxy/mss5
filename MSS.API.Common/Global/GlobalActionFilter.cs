using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MSS.API.Common.Utility;
using static MSS.API.Common.Const;
using CSRedis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace MSS.API.Common.Global
{
    public class GlobalActionFilter: IActionFilter
    {
        IAuthHelper _authhelper;
        private readonly IDistributedCache _cache;

        public GlobalActionFilter(IAuthHelper authhelper, IDistributedCache cache)
        {
            _authhelper = authhelper;
            _cache = cache;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var head = context.HttpContext.Request.Headers["Authorization"];
            //if (!string.IsNullOrEmpty(head))
            //{
            //    string controllername = context.RouteData.Values["Controller"].ToString();
            //    string actionname = context.RouteData.Values["Action"].ToString();
            //    string methodname = context.HttpContext.Request.Method.ToString();

            //    var userid = _authhelper.GetUserId();
            //    if (userid == -1)
            //    {
            //        context.Result = new JsonResult(new { Code = Code.DataIsnotExist, Msg = "不属于本系统的人员" });
            //    }

            //    string url = ("/" + controllername + "/" + actionname).ToLower();

            //    List<User> users = JsonConvert.DeserializeObject<List<User>>(_cache.GetString(REDIS_AUTH_KEY_USER));
            //    User user = users.Where(a => a.id == userid).FirstOrDefault();
            //    //List<ActionInfo> white = new List<ActionInfo>();
            //    //List<ActionInfo> black = new List<ActionInfo>();
            //    if (!user.is_super)
            //    {
            //        List<RoleAction> ras = JsonConvert.DeserializeObject<List<RoleAction>>(_cache.GetString(REDIS_AUTH_KEY_ROLEACTION));
            //        var tmp = ras.Where(a => a.role_id == user.role_id);
            //        if (tmp != null)
            //        {
            //            List<int> actionIds = tmp.Select(a => a.action_id).ToList();
            //            List<ActionInfo> actions = JsonConvert.DeserializeObject<List<ActionInfo>>(_cache.GetString(REDIS_AUTH_KEY_ACTIONINFO));
            //            //foreach (ActionInfo item in actions)
            //            //{
            //            //    if (actionIds.Where(a => a == item.id).Count() == 0)
            //            //    //{
            //            //    //    white.Add(item);
            //            //    //}
            //            //    //else
            //            //    {
            //            //        black.Add(item);
            //            //    }
            //            //}
            //            if (actions.Where(a => (a.request_url).ToLower() == url).Count() > 0 || methodname == "GET")
            //            {
            //                return;
            //            }
            //        }
            //        context.Result = new JsonResult(new { Code = Code.Failure, Msg = "您没有权限访问该接口" });
            //    }
            //}
            //else
            //{
            //    context.Result = new JsonResult(new { Code = Code.Failure, Msg = "您没有权限访问该接口" });
            //}

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public abstract class BaseEntity
        {
            public int id { get; set; }
            public DateTime created_time { get; set; }
            public int created_by { get; set; }
            public DateTime updated_time { get; set; }
            public int updated_by { get; set; }
        }

        public class User : BaseEntity
        {

            public string acc_name { get; set; }


            public string user_name { get; set; }


            public string password { get; set; }


            public int random_num { get; set; }
            public string job_number { get; set; }
            public string position { get; set; }
            public string id_card { get; set; }
            public DateTime birth { get; set; }
            public bool sex { get; set; }
            public string mobile { get; set; }
            public string email { get; set; }
            public string id_photo { get; set; }
            /// <summary>
            /// 存在没有任何权限，但下拉能选中的人员
            /// </summary>
            public int? role_id { get; set; }
            public bool is_del { get; set; }
            public bool is_super { get; set; }
        }

        public class RoleAction
        {
            public int id { get; set; }
            public int role_id { get; set; }
            public int action_id { get; set; }
        }

        public class ActionInfo : BaseEntity
        {
            public string action_name { get; set; }
            public string request_url { get; set; }

            public string description { get; set; }

            public int action_order { get; set; }

            public string icon { get; set; }

            public int level { get; set; }

            public int? group_id { get; set; }

            public int? parent_menu { get; set; }
        }

    }
}
