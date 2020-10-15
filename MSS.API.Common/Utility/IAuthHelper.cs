using CSRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MSS.API.Common.Utility
{
    public interface IAuthHelper
    {
       int GetUserId();
    }
}
