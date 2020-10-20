using CSRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MSS.API.Common.Utility
{
    public class AuthHelper : IAuthHelper
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;

        public AuthHelper(IHttpContextAccessor httpContextAccessor, IDistributedCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }
        public int GetUserId()
        {
            int userid = 1;//TODO 接通token后继续调试

            //try
            //{
            //    string token = string.Empty;
            //    var context = _httpContextAccessor.HttpContext;
            //    var head = context.Request.Headers["Authorization"];
            //    if (!string.IsNullOrEmpty(head))
            //    {
            //        if (head.ToString().IndexOf("Bearer") >= 0)
            //        {
            //            token = head.ToString().Replace("Bearer", "").Trim();

            //            userid = int.Parse(_cache.GetString(token));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message.ToString());
            //}
            return userid;
        }
    }
}
